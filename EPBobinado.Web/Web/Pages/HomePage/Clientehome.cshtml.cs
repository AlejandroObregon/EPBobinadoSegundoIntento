using Abstracciones.Modelos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Cryptography;
using System.Text.Json;
using Web.Pages;

namespace Web.Pages
{
    public class ClienteHomeModel : PageModelBase
    {

        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _config;

        public ClienteHomeModel(IHttpClientFactory httpClientFactory, IConfiguration config)
        {
            _httpClientFactory = httpClientFactory;
            _config = config;
        }

        // ── Datos del cliente (heredados de PageModelBase) ───────────
        // UsuarioId, UsuarioRol, UsuarioNombre disponibles desde la base

        // ── Métricas ──────────────────────────────────────────────
        public int TotalMotores { get; set; }
        public int OrdenesActivas { get; set; }
        public int OrdenesCompletadas { get; set; }

        // ── Secciones ─────────────────────────────────────────────
        public List<MotorResponse> MisMotores { get; set; } = new();
        public List<OrdenServicioResponse> MisOrdenes { get; set; } = new();
        public List<ProductoResponse> ProductosDestacados { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {

            // Verificar sesión activa (solo clientes)
            var auth = VerificarSesion();
            if (auth != null) return auth;
            int usuarioId = UsuarioId;
            // Solo clientes aquí; admins/técnicos van al dashboard general
            if (UsuarioRol != 1)
                return Redirect("/HomePage/HomePage");

            // UsuarioNombre ya está disponible desde PageModelBase

            var opciones = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var client = _httpClientFactory.CreateClient();

            // ── 1. Motores del cliente ────────────────────────────
            try
            {
                var resp = await client.GetAsync(ObtenerUrl("ApiEndPointsMotor", "Obtener"));
                if (resp.IsSuccessStatusCode)
                {
                    var todos = JsonSerializer.Deserialize<List<MotorResponse>>(
                        await resp.Content.ReadAsStringAsync(), opciones) ?? new();
                    MisMotores = todos.Where(m => m.UsuarioId == UsuarioId).ToList();
                    TotalMotores = MisMotores.Count;
                }
            }
            catch { }

            // ── 2. Órdenes del cliente ────────────────────────────
            try
            {
                var resp = await client.GetAsync(ObtenerUrl("ApiEndPointsOrdenServicio", "Obtener"));
                if (resp.IsSuccessStatusCode)
                {
                    var todas = JsonSerializer.Deserialize<List<OrdenServicioResponse>>(
                        await resp.Content.ReadAsStringAsync(), opciones) ?? new();

                    // Filtrar solo órdenes de motores del cliente
                    MisOrdenes = todas
                        .Where(o => o.IdCliente == usuarioId)
                        .OrderByDescending(o => o.CreadoEn)
                        .Take(5)
                        .ToList();

                    var activos = new[] { "Pendiente", "En diagnóstico", "En reparación" };
                    OrdenesActivas = todas.Count(o => o.IdCliente == usuarioId && activos.Contains(o.Estado));
                    OrdenesCompletadas = todas.Count(o => o.IdCliente == usuarioId && o.Estado == "Completado");
                }
            }
            catch { }

            // ── 3. Productos destacados (6 activos) ───────────────
            try
            {
                var resp = await client.GetAsync(ObtenerUrl("ApiEndPointsProducto", "Obtener"));
                if (resp.IsSuccessStatusCode)
                {
                    var todos = JsonSerializer.Deserialize<List<ProductoResponse>>(
                        await resp.Content.ReadAsStringAsync(), opciones) ?? new();
                    ProductosDestacados = todos
                        .Where(p => p.Activo && p.Stock > 0)
                        .OrderBy(_ => Guid.NewGuid())   // orden aleatorio para "destacados"
                        .Take(6)
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