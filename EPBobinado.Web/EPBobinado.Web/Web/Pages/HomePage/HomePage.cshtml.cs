using Abstracciones.Modelos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace Web.Pages.HomePage
{
    public class HomePageModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _config;

        public HomePageModel(IHttpClientFactory httpClientFactory, IConfiguration config)
        {
            _httpClientFactory = httpClientFactory;
            _config = config;
        }

        // ── Métricas ─────────────────────────────────────────────
        public int TotalMotores { get; set; }
        public int OrdenesActivas { get; set; }
        public int OrdenesCompletadas { get; set; }
        public int OrdenesPendientes { get; set; }

        // ── Datos para las secciones ──────────────────────────────
        public List<OrdenServicioResponse> UltimasOrdenes { get; set; } = new();
        public List<OrdenServicioResponse> TodasLasOrdenes { get; set; } = new();
        public List<MotorResponse> MisMotores { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            var opciones = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var client = _httpClientFactory.CreateClient();

            // Obtener UsuarioId de sesión para filtrar si es un cliente
            int.TryParse(HttpContext.Session.GetString("UsuarioId"), out int usuarioId);
            int.TryParse(HttpContext.Session.GetString("UsuarioNivel"), out int rol);

            // Los clientes tienen su propio dashboard
            if (rol == 1)
                return Redirect("/ClienteHome");

            // ── 1. Motores ────────────────────────────────────────
            try
            {
                var endpoint = ObtenerUrl("ApiEndPointsMotor", "Obtener");
                var resp = await client.GetAsync(endpoint);
                if (resp.IsSuccessStatusCode)
                {
                    var motores = JsonSerializer.Deserialize<List<MotorResponse>>(
                        await resp.Content.ReadAsStringAsync(), opciones) ?? new();

                    // Si es cliente (RolId=1) mostrar solo sus motores, admins/técnicos ven todos
                    MisMotores = rol == 1
                        ? motores.Where(m => m.UsuarioId == usuarioId).ToList()
                        : motores;
                    TotalMotores = MisMotores.Count;
                }
            }
            catch { }

            // ── 2. Órdenes de servicio ────────────────────────────
            try
            {
                var endpoint = ObtenerUrl("ApiEndPointsOrdenServicio", "Obtener");
                var resp = await client.GetAsync(endpoint);
                if (resp.IsSuccessStatusCode)
                {
                    var ordenes = JsonSerializer.Deserialize<List<OrdenServicioResponse>>(
                        await resp.Content.ReadAsStringAsync(), opciones) ?? new();

                    // Si es cliente filtrar por sus motores
                    TodasLasOrdenes = rol == 1
                        ? ordenes.Where(o => MisMotores.Any(m => m.Id == o.MotorId)).ToList()
                        : ordenes;

                    var estadosActivos = new[] { "Pendiente", "En diagnóstico", "En reparación" };
                    OrdenesActivas = TodasLasOrdenes.Count(o => estadosActivos.Contains(o.Estado));
                    OrdenesCompletadas = TodasLasOrdenes.Count(o => o.Estado == "Completado");
                    OrdenesPendientes = TodasLasOrdenes.Count(o => o.Estado == "Pendiente");

                    // Las 5 más recientes con estado activo
                    UltimasOrdenes = TodasLasOrdenes
                        .Where(o => estadosActivos.Contains(o.Estado))
                        .OrderByDescending(o => o.CreadoEn)
                        .Take(5)
                        .ToList();
                }
            }
            catch { }

            return Page();
        }

        // ── Helper ────────────────────────────────────────────────
        private string ObtenerUrl(string seccionKey, string metodoNombre)
        {
            var section = _config.GetSection(seccionKey);
            var urlBase = section.GetValue<string>("UrlBase")?.Trim() ?? "";
            foreach (var m in section.GetSection("Metodos").GetChildren())
            {
                if (string.Equals(m.GetValue<string>("Nombre"), metodoNombre, StringComparison.OrdinalIgnoreCase))
                    return $"{urlBase.TrimEnd('/')}/{(m.GetValue<string>("Valor") ?? "").TrimStart('/')}";
            }
            return "";
        }
    }
}