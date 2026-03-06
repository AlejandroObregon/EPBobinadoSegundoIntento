using Abstracciones.Modelos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net;
using System.Text.Json;

namespace Web.Pages.Producto
{
    public class CatalogoModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _config;

        public CatalogoModel(IHttpClientFactory httpClientFactory, IConfiguration config)
        {
            _httpClientFactory = httpClientFactory;
            _config = config;
        }

        // Solo productos activos — el filtro por categoría/precio/búsqueda
        // se hace en el cliente con JS, por eso exponemos la lista completa al View
        public List<ProductoResponse> Productos { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            // Redirigir si no hay sesión activa
            var usuarioId = HttpContext.Session.GetString("UsuarioId");
            if (string.IsNullOrWhiteSpace(usuarioId))
                return RedirectToPage("/Auth/Login");

            // Construir endpoint
            var section = _config.GetSection("ApiEndPointsProducto");
            var urlBase = (section.GetValue<string>("UrlBase") ?? "").Trim();

            if (string.IsNullOrWhiteSpace(urlBase))
            {
                ModelState.AddModelError(string.Empty, "No se encontró la configuración de endpoints de Producto.");
                return Page();
            }

            string? endpoint = null;
            foreach (var m in section.GetSection("Metodos").GetChildren())
            {
                if (string.Equals(m.GetValue<string>("Nombre"), "Obtener", StringComparison.OrdinalIgnoreCase))
                {
                    var valor = m.GetValue<string>("Valor") ?? "";
                    endpoint = $"{urlBase.TrimEnd('/')}/{valor.TrimStart('/')}";
                    break;
                }
            }

            if (string.IsNullOrWhiteSpace(endpoint))
            {
                ModelState.AddModelError(string.Empty, "No se encontró el método 'Obtener' en la configuración.");
                return Page();
            }

            // Llamar a la API
            var client = _httpClientFactory.CreateClient();
            using var response = await client.GetAsync(endpoint);

            if (response.IsSuccessStatusCode && response.StatusCode == HttpStatusCode.OK)
            {
                var json = await response.Content.ReadAsStringAsync();
                var opciones = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var todos = JsonSerializer.Deserialize<List<ProductoResponse>>(json, opciones) ?? new();

                // El catálogo solo muestra productos activos con stock > 0 o que aún estén activos
                // (productos agotados se muestran igualmente para visibilidad, el View pone overlay)
                Productos = todos.Where(p => p.Activo).OrderBy(p => p.Nombre).ToList();
            }
            else if (response.StatusCode == HttpStatusCode.NoContent)
            {
                Productos = new();
            }
            else
            {
                ModelState.AddModelError(string.Empty, "No se pudieron cargar los productos del catálogo.");
            }

            return Page();
        }
    }
}