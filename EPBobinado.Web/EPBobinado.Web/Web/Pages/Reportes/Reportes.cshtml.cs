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

        // ── Datos crudos ──────────────────────────────────────────────
        public List<OrdenServicioResponse> Ordenes { get; set; } = new();
        public List<MotorResponse> Motores { get; set; } = new();
        public List<UsuarioResponse> Usuarios { get; set; } = new();

        // ── KPIs rápidos ─────────────────────────────────────────────
        public int TotalOrdenes => Ordenes.Count;
        public int OrdenesActivas => Ordenes.Count(o => o.Estado is "Pendiente" or "En diagnóstico" or "En reparación");
        public int OrdenesCompletadas => Ordenes.Count(o => o.Estado == "Completado");
        public int TotalMotores => Motores.Count;
        public int TotalClientes => Usuarios.Count(u => u.RolId == 1);
        public int TotalTecnicos => Usuarios.Count(u => u.RolId == 1002);

        // ── Exporte de Reporte en PDF ────────────────

        public IActionResult OnPostReporte()
        {
            var pdf = new NReco.PdfGenerator.HtmlToPdfConverter();
            string url = "https://localhost:7225/Reportes";
            System.Net.WebClient wc = new System.Net.WebClient();
            string html = wc.DownloadString(url);

            var pdfByte = pdf.GeneratePdf(html);
            return File(pdfByte, "application/pdf", "reporteGeneral.pdf");
        }

        // ── Series para gráficos (JSON para pasar a JS) ────────────────

        // Órdenes por estado → dona
        public string OrdenesEstadoJson => BuildJson(
            Ordenes.GroupBy(o => o.Estado)
                   .Select(g => new { label = g.Key, value = g.Count() }));

        // Órdenes por mes (últimos 6 meses) → línea
        public string OrdenesPorMesJson
        {
            get
            {
                var hoy = DateTime.Today;
                var meses = Enumerable.Range(0, 6)
                    .Select(i => hoy.AddMonths(-5 + i))
                    .ToList();

                var data = meses.Select(m => new
                {
                    label = m.ToString("MMM yyyy"),
                    value = Ordenes.Count(o => o.CreadoEn.Year == m.Year && o.CreadoEn.Month == m.Month)
                });
                return BuildJson(data);
            }
        }

        // Órdenes por técnico → barras
        public string OrdenesPorTecnicoJson => BuildJson(
            Ordenes.Where(o => o.Tecnico != null)
                   .GroupBy(o => o.Tecnico!.Nombre)
                   .OrderByDescending(g => g.Count())
                   .Take(8)
                   .Select(g => new { label = g.Key, value = g.Count() }));

        // Motores por modelo → barras horizontales
        public string MotoresPorModeloJson => BuildJson(
            Motores.Where(m => m.Modelo != null)
                   .GroupBy(m => m.Modelo!.Nombre)
                   .OrderByDescending(g => g.Count())
                   .Take(8)
                   .Select(g => new { label = g.Key, value = g.Count() }));

        // Productos: stock actual por categoría → barras apiladas
        // (usamos Producto endpoint — se pasará al client como JSON en el view)
        public string ProductosStockJson { get; private set; } = "[]";
        public string ProductosBajosJson { get; private set; } = "[]";
        public int TotalProductos { get; private set; }
        public int ProductosBajoStock { get; private set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var auth = VerificarSesion(2, 1002); // solo admin y técnico
            if (auth != null) return auth;

            var opciones = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var client = _httpClientFactory.CreateClient();

            await CargarAsync<OrdenServicioResponse>(client, opciones, "ApiEndPointsOrdenServicio",
                data => Ordenes = data);

            await CargarAsync<MotorResponse>(client, opciones, "ApiEndPointsMotor",
                data => Motores = data);

            await CargarAsync<UsuarioResponse>(client, opciones, "ApiEndPointsUsuario",
                data => Usuarios = data);

            // Productos — sección separada para construir los JSONs de stock
            try
            {
                var url = ObtenerUrl("ApiEndPointsProducto", "Obtener");
                var resp = await client.GetAsync(url);
                if (resp.IsSuccessStatusCode && resp.StatusCode == HttpStatusCode.OK)
                {
                    var productos = JsonSerializer.Deserialize<List<ProductoResponse>>(
                        await resp.Content.ReadAsStringAsync(), opciones) ?? new();

                    TotalProductos = productos.Count;
                    ProductosBajoStock = productos.Count(p => p.Activo && p.Stock <= p.StockMinimo);

                    // Stock por categoría
                    ProductosStockJson = BuildJson(
                        productos.Where(p => p.Activo)
                                 .GroupBy(p => string.IsNullOrWhiteSpace(p.Categoria) ? "Sin categoría" : p.Categoria)
                                 .OrderByDescending(g => g.Sum(p => p.Stock))
                                 .Take(8)
                                 .Select(g => new { label = g.Key, value = g.Sum(p => p.Stock) }));

                    // Productos con stock más bajo (top 6)
                    ProductosBajosJson = BuildJson(
                        productos.Where(p => p.Activo)
                                 .OrderBy(p => p.Stock)
                                 .Take(6)
                                 .Select(p => new { label = p.Nombre.Length > 20 ? p.Nombre[..20] + "…" : p.Nombre, value = p.Stock }));
                }
            }
            catch { }

            return Page();
        }

        // ── Helpers ───────────────────────────────────────────────────
        private async Task CargarAsync<T>(HttpClient client, JsonSerializerOptions opt,
            string seccionKey, Action<List<T>> setter)
        {
            try
            {
                var resp = await client.GetAsync(ObtenerUrl(seccionKey, "Obtener"));
                if (resp.IsSuccessStatusCode && resp.StatusCode == HttpStatusCode.OK)
                {
                    var data = JsonSerializer.Deserialize<List<T>>(
                        await resp.Content.ReadAsStringAsync(), opt) ?? new();
                    setter(data);
                }
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

    // DTO mínimo de Producto para esta página
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