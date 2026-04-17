using Abstracciones.Modelos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net;
using System.Text.Json;
using Web.Pages;

namespace Web.Pages.Reportes
{
    public class ReportesModel : PageModelBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _config;

        public ReportesModel(IHttpClientFactory httpClientFactory, IConfiguration config)
        {
            _httpClientFactory = httpClientFactory;
            _config = config;
        }

        public List<OrdenServicioResponse> Ordenes { get; set; } = new();
        public List<MotorResponse> Motores { get; set; } = new();
        public List<UsuarioResponse> Usuarios { get; set; } = new();

        // ── KPIs globales ─────────────────────────────────────────────
        public int TotalOrdenes => Ordenes.Count;
        public int OrdenesActivas => Ordenes.Count(o => o.Estado is "Pendiente" or "En diagnóstico" or "En reparación");
        public int OrdenesCompletadas => Ordenes.Count(o => o.Estado == "Completado");
        public int TotalMotores => Motores.Count;
        public int TotalClientes => Usuarios.Count(u => u.RolId == 1);
        public int TotalTecnicos => Usuarios.Count(u => u.RolId == 1002);

        public IActionResult OnPostReporte()
        {
            var pdf = new NReco.PdfGenerator.HtmlToPdfConverter();
            var wc = new System.Net.WebClient();
            var html = wc.DownloadString("https://localhost:7225/Reportes");
            var pdfByte = pdf.GeneratePdf(html);
            return File(pdfByte, "application/pdf", "reporteGeneral.pdf");
        }

        // ── JSONs para Órdenes ────────────────────────────────────────
        public string OrdenesJsonCompleto => BuildJson(
            Ordenes.Select(o => new {
                id = o.Id,
                estado = o.Estado,
                tecnico = o.Tecnico ?? "Sin asignar",
                creadoEn = o.CreadoEn.ToString("yyyy-MM-dd")
            }));

        // ── JSONs para Motores ────────────────────────────────────────
        public string MotoresPorModeloJson => BuildJson(
            Motores.Where(m => m.Modelo != null)
                   .GroupBy(m => m.Modelo!.Nombre)
                   .OrderByDescending(g => g.Count()).Take(8)
                   .Select(g => new { label = g.Key, value = g.Count() }));

        public string MotoresDistribucionJson
        {
            get
            {
                var grupos = Motores.Where(m => m.Modelo != null)
                    .GroupBy(m => m.Modelo!.Nombre)
                    .OrderByDescending(g => g.Count()).ToList();
                var top5 = grupos.Take(5).Select(g => new { label = g.Key, value = g.Count() }).ToList();
                var otros = grupos.Skip(5).Sum(g => g.Count());
                if (otros > 0) top5.Add(new { label = "Otros", value = otros });
                return BuildJson(top5);
            }
        }

        public string MotoresPorDuenoJson => BuildJson(
            Motores.Where(m => m.Usuario?.Nombre != null)
                   .GroupBy(m => m.Usuario!.Nombre)
                   .OrderByDescending(g => g.Count()).Take(8)
                   .Select(g => new { label = g.Key, value = g.Count() }));

        // ── JSONs para Inventario ─────────────────────────────────────
        public string ProductosStockJson { get; private set; } = "[]";
        public string ProductosBajosJson { get; private set; } = "[]";
        public string ProductosPorCategoriaJson { get; private set; } = "[]";
        public int TotalProductos { get; private set; }
        public int ProductosBajoStock { get; private set; }

        // ── KPIs Ventas & Pagos (datos reales) ───────────────────────
        public string VentasPorMesJson { get; private set; } = "[]";
        public string PagosPorMetodoJson { get; private set; } = "[]";
        public string IngresosPorServicioJson { get; private set; } = "[]";
        public decimal VentasTotalMes { get; private set; }
        public decimal TicketPromedio { get; private set; }
        public int PagosPendientes { get; private set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var auth = VerificarSesion(2, 1002);
            if (auth != null) return auth;

            var opciones = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var client = _httpClientFactory.CreateClient();

            await CargarAsync<OrdenServicioResponse>(client, opciones, "ApiEndPointsOrdenServicio", d => Ordenes = d);
            await CargarAsync<MotorResponse>(client, opciones, "ApiEndPointsMotor", d => Motores = d);
            await CargarAsync<UsuarioResponse>(client, opciones, "ApiEndPointsUsuario", d => Usuarios = d);

            // ── Productos ─────────────────────────────────────────────
            try
            {
                var resp = await client.GetAsync(ObtenerUrl("ApiEndPointsProducto", "Obtener"));
                if (resp.IsSuccessStatusCode && resp.StatusCode == HttpStatusCode.OK)
                {
                    var productos = JsonSerializer.Deserialize<List<ProductoResponse>>(
                        await resp.Content.ReadAsStringAsync(), opciones) ?? new();

                    TotalProductos = productos.Count;
                    ProductosBajoStock = productos.Count(p => p.Activo && p.Stock <= p.StockMinimo);

                    ProductosStockJson = BuildJson(
                        productos.Where(p => p.Activo)
                                 .GroupBy(p => string.IsNullOrWhiteSpace(p.Categoria) ? "Sin categoría" : p.Categoria)
                                 .OrderByDescending(g => g.Sum(p => p.Stock)).Take(8)
                                 .Select(g => new { label = g.Key, value = g.Sum(p => p.Stock) }));

                    ProductosBajosJson = BuildJson(
                        productos.Where(p => p.Activo).OrderBy(p => p.Stock).Take(6)
                                 .Select(p => new {
                                     label = p.Nombre.Length > 20 ? p.Nombre[..20] + "…" : p.Nombre,
                                     value = p.Stock
                                 }));

                    ProductosPorCategoriaJson = BuildJson(
                        productos.Where(p => p.Activo)
                                 .GroupBy(p => string.IsNullOrWhiteSpace(p.Categoria) ? "Sin categoría" : p.Categoria)
                                 .Select(g => new {
                                     categoria = g.Key,
                                     totalStock = g.Sum(p => p.Stock),
                                     cantBajos = g.Count(p => p.Stock <= p.StockMinimo),
                                     items = g.Select(p => new {
                                         label = p.Nombre.Length > 22 ? p.Nombre[..22] + "…" : p.Nombre,
                                         value = p.Stock,
                                         minimo = p.StockMinimo
                                     }).OrderBy(p => p.value).ToList()
                                 }).ToList());
                }
            }
            catch { }

            // ── Pagos y Facturas (datos reales) ───────────────────────
            await CargarVentasAsync(client, opciones);

            return Page();
        }

        private async Task CargarVentasAsync(HttpClient client, JsonSerializerOptions opciones)
        {
            var pagos = new List<PagoResponse>();
            var facturas = new List<FacturaResponse>();

            try
            {
                var rPagos = await client.GetAsync(ObtenerUrl("ApiEndPointsPago", "Obtener"));
                if (rPagos.IsSuccessStatusCode && rPagos.StatusCode == HttpStatusCode.OK)
                    pagos = JsonSerializer.Deserialize<List<PagoResponse>>(
                        await rPagos.Content.ReadAsStringAsync(), opciones) ?? new();
            }
            catch { }

            try
            {
                var rFact = await client.GetAsync(ObtenerUrl("ApiEndPointsFactura", "Obtener"));
                if (rFact.IsSuccessStatusCode && rFact.StatusCode == HttpStatusCode.OK)
                    facturas = JsonSerializer.Deserialize<List<FacturaResponse>>(
                        await rFact.Content.ReadAsStringAsync(), opciones) ?? new();
            }
            catch { }

            // ── KPIs ─────────────────────────────────────────────────
            var hoy = DateTime.Today;
            var inicioMes = new DateTime(hoy.Year, hoy.Month, 1);

            VentasTotalMes = pagos
                .Where(p => p.Fecha >= inicioMes)
                .Sum(p => p.Monto);

            // Ticket promedio: desde facturas si hay, sino desde pagos
            TicketPromedio = facturas.Any(f => f.Total.HasValue && f.Total > 0)
                ? facturas.Where(f => f.Total.HasValue && f.Total > 0).Average(f => f.Total!.Value)
                : pagos.Any()
                    ? pagos.Average(p => p.Monto)
                    : 0;

            // Facturas sin ningún pago asociado = pendientes
            var facturasConPago = pagos.Select(p => p.FacturaId).ToHashSet();
            PagosPendientes = facturas.Count(f => !facturasConPago.Contains(f.Id));

            // ── Ventas por mes (últimos 6 meses basado en pagos) ──────
            var meses = Enumerable.Range(0, 6)
                .Select(i => hoy.AddMonths(-5 + i))
                .ToList();

            VentasPorMesJson = BuildJson(
                meses.Select(m => new {
                    label = m.ToString("MMM yyyy"),
                    value = pagos
                        .Where(p => p.Fecha.Year == m.Year && p.Fecha.Month == m.Month)
                        .Sum(p => p.Monto)
                }));

            // ── Pagos por método (conteo de transacciones) ────────────
            PagosPorMetodoJson = BuildJson(
                pagos.Where(p => !string.IsNullOrWhiteSpace(p.MetodoPago))
                     .GroupBy(p => p.MetodoPago!)
                     .OrderByDescending(g => g.Count())
                     .Select(g => new { label = g.Key, value = g.Count() }));

            // ── Ingresos por tipo de servicio ─────────────────────────
            // Cruza facturas con el dict de órdenes ya cargadas (no depende de f.Orden)
            var ordenesEstado = Ordenes.ToDictionary(o => o.Id, o => o.Estado);
            var ingresosServ = facturas
                .Where(f => f.Total.HasValue && f.Total > 0)
                .GroupBy(f => ordenesEstado.TryGetValue(f.OrdenId, out var est) ? est : "Sin estado")
                .OrderByDescending(g => g.Sum(f => f.Total!.Value))
                .Take(6)
                .Select(g => new { label = g.Key, value = g.Sum(f => f.Total!.Value) })
                .ToList();

            // Si no hay facturas con datos, usar pagos agrupados por método como fallback
            IngresosPorServicioJson = ingresosServ.Any()
                ? BuildJson(ingresosServ)
                : BuildJson(pagos
                    .GroupBy(p => p.MetodoPago ?? "Sin método")
                    .Select(g => new { label = g.Key, value = g.Sum(p => p.Monto) }));
        }

        private async Task CargarAsync<T>(HttpClient client, JsonSerializerOptions opt,
            string seccionKey, Action<List<T>> setter)
        {
            try
            {
                var resp = await client.GetAsync(ObtenerUrl(seccionKey, "Obtener"));
                if (resp.IsSuccessStatusCode && resp.StatusCode == HttpStatusCode.OK)
                    setter(JsonSerializer.Deserialize<List<T>>(
                        await resp.Content.ReadAsStringAsync(), opt) ?? new());
            }
            catch { }
        }

        private string ObtenerUrl(string seccionKey, string metodoNombre)
        {
            var section = _config.GetSection(seccionKey);
            var urlBase = section.GetValue<string>("UrlBase")?.Trim() ?? "";
            foreach (var m in section.GetSection("Metodos").GetChildren())
                if (string.Equals(m.GetValue<string>("Nombre"), metodoNombre, StringComparison.OrdinalIgnoreCase))
                    return $"{urlBase.TrimEnd('/')}/{(m.GetValue<string>("Valor") ?? "").TrimStart('/')}";
            return "";
        }

        private static string BuildJson<T>(IEnumerable<T> data) =>
            System.Text.Json.JsonSerializer.Serialize(data);
    }

    // ── DTOs locales ──────────────────────────────────────────────
    public class ProductoResponse
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = "";
        public string Categoria { get; set; } = "";
        public int Stock { get; set; }
        public int StockMinimo { get; set; }
        public decimal Precio { get; set; }
        public bool Activo { get; set; }
    }

    public class PagoResponse
    {
        public int Id { get; set; }
        public int FacturaId { get; set; }
        public decimal Monto { get; set; }
        public string? MetodoPago { get; set; }
        public DateTime Fecha { get; set; }
    }

    public class FacturaResponse
    {
        public int Id { get; set; }
        public int OrdenId { get; set; }
        public decimal? Total { get; set; }
        public decimal? Impuesto { get; set; }
        public DateTime Fecha { get; set; }
        public OrdenServicioResponse? Orden { get; set; }
    }
}