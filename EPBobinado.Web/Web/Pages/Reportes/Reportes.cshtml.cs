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

        // Todas las órdenes serializadas para que JS filtre en el cliente
        public string OrdenesJsonCompleto => BuildJson(
            Ordenes.Select(o => new {
                id = o.Id,
                estado = o.Estado,
                tecnico = o.Tecnico ?? "Sin asignar",
                creadoEn = o.CreadoEn.ToString("yyyy-MM-dd")
            }));

        // ── Motores ────────────────────────────────────────────────────────
        // Top 8 por cantidad (barra horizontal)
        public string MotoresPorModeloJson => BuildJson(
            Motores.Where(m => m.Modelo != null)
                   .GroupBy(m => m.Modelo!.Nombre)
                   .OrderByDescending(g => g.Count())
                   .Take(8)
                   .Select(g => new { label = g.Key, value = g.Count() }));

        // Distribución top-5 + Otros (doughnut)
        public string MotoresDistribucionJson
        {
            get
            {
                var grupos = Motores.Where(m => m.Modelo != null)
                    .GroupBy(m => m.Modelo!.Nombre)
                    .OrderByDescending(g => g.Count())
                    .ToList();

                var top5 = grupos.Take(5).Select(g => new { label = g.Key, value = g.Count() }).ToList();
                var otros = grupos.Skip(5).Sum(g => g.Count());
                if (otros > 0)
                    top5.Add(new { label = "Otros", value = otros });
                return BuildJson(top5);
            }
        }

        // Motores con más órdenes de servicio (requiere Motor en OrdenServicioResponse)
        // Si OrdenServicioResponse no tiene Motor, esta propiedad devolverá "[]"
        // Reemplazar MotoresMasServiciadosJson por esto:
        public string MotoresPorDuenoJson => BuildJson(
            Motores
                .Where(m => m.Usuario.Nombre != null)   
                .GroupBy(m => m.Usuario.Nombre)
                .OrderByDescending(g => g.Count())
                .Take(8)
                .Select(g => new { label = g.Key, value = g.Count() }));

        // ── Inventario ─────────────────────────────────────────────────────
        public string ProductosStockJson { get; private set; } = "[]";
        public string ProductosBajosJson { get; private set; } = "[]";
        // Dataset completo para filtrado en cliente (categoría + productos)
        public string ProductosPorCategoriaJson { get; private set; } = "[]";
        public int TotalProductos { get; private set; }
        public int ProductosBajoStock { get; private set; }

        // ── Datos quemados Ventas & Pagos ──────────────────────────────────
        public string VentasPorMesJson => BuildJson(new[]
        {
            new { label = "Oct 2025", value = 285000m },
            new { label = "Nov 2025", value = 412000m },
            new { label = "Dic 2025", value = 378000m },
            new { label = "Ene 2026", value = 520000m },
            new { label = "Feb 2026", value = 467000m },
            new { label = "Mar 2026", value = 615000m },
        });

        public string PagosPorMetodoJson => BuildJson(new[]
        {
            new { label = "Transferencia", value = 48 },
            new { label = "Efectivo",      value = 31 },
            new { label = "Tarjeta",       value = 15 },
            new { label = "SINPE",         value = 6  },
        });

        public string IngresosPorServicioJson => BuildJson(new[]
        {
            new { label = "Rebobinado completo",  value = 890000m },
            new { label = "Diagnóstico",          value = 340000m },
            new { label = "Mantenimiento",        value = 275000m },
            new { label = "Reparación eléctrica", value = 210000m },
            new { label = "Cambio rodamientos",   value = 185000m },
        });

        public decimal VentasTotalMes => 615000m;
        public decimal TicketPromedio => 41000m;
        public int PagosPendientes => 4;

        public async Task<IActionResult> OnGetAsync()
        {
            var auth = VerificarSesion(2, 1002);
            if (auth != null) return auth;

            var opciones = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var client = _httpClientFactory.CreateClient();

            await CargarAsync<OrdenServicioResponse>(client, opciones, "ApiEndPointsOrdenServicio", d => Ordenes = d);
            await CargarAsync<MotorResponse>(client, opciones, "ApiEndPointsMotor", d => Motores = d);
            await CargarAsync<UsuarioResponse>(client, opciones, "ApiEndPointsUsuario", d => Usuarios = d);

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

                    // Dataset completo por categoría para filtrado en JS
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

            return Page();
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
}