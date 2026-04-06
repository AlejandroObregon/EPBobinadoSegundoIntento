using Abstracciones.Modelos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net;
using System.Text.Json;

namespace Web.Pages.Pagos
{
    public class ListadoModel : PageModelBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _config;

        public ListadoModel(IHttpClientFactory httpClientFactory, IConfiguration config)
        {
            _httpClientFactory = httpClientFactory;
            _config = config;
        }

        public List<PagoResponse> Pagos { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            var auth = VerificarSesion(2);
            if (auth != null) return auth;

            var section = _config.GetSection("ApiEndPointsPago");
            var urlBase = (section.GetValue<string>("UrlBase") ?? "").Trim();

            if (string.IsNullOrWhiteSpace(urlBase))
            {
                ModelState.AddModelError(string.Empty, "No se encontró la configuración de endpoints de Pago.");
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

            var client = _httpClientFactory.CreateClient();
            using var response = await client.GetAsync(endpoint);

            if (response.IsSuccessStatusCode && response.StatusCode == HttpStatusCode.OK)
            {
                var json = await response.Content.ReadAsStringAsync();
                var opciones = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                Pagos = JsonSerializer.Deserialize<List<PagoResponse>>(json, opciones) ?? new();
            }
            else if (response.StatusCode == HttpStatusCode.NoContent)
            {
                Pagos = new();
            }
            else
            {
                ModelState.AddModelError(string.Empty, "No se pudieron obtener los pagos.");
            }

            return Page();
        }
    }
}
