using Abstracciones.Modelos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net;
using System.Text.Json;

namespace Web.Pages.DiagnosticoInicial
{
    public class ListadoModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _config;

        public ListadoModel(IHttpClientFactory httpClientFactory, IConfiguration config)
        {
            _httpClientFactory = httpClientFactory;
            _config = config;
        }

        public List<DiagnosticoInicialResponse> Diagnosticos { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            var section = _config.GetSection("ApiEndPointsDiagnosticoInicial");
            var urlBase = (section.GetValue<string>("UrlBase") ?? "").Trim();

            if (string.IsNullOrWhiteSpace(urlBase))
            {
                ModelState.AddModelError(string.Empty, "No se encontró la configuración de endpoints.");
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
                ModelState.AddModelError(string.Empty, "No se encontró el método 'Obtener'.");
                return Page();
            }

            var client = _httpClientFactory.CreateClient();
            using var response = await client.GetAsync(endpoint);

            if (response.IsSuccessStatusCode && response.StatusCode == HttpStatusCode.OK)
            {
                var opciones = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                Diagnosticos = JsonSerializer.Deserialize<List<DiagnosticoInicialResponse>>(
                    await response.Content.ReadAsStringAsync(), opciones) ?? new();
            }
            else if (response.StatusCode == HttpStatusCode.NoContent)
            {
                Diagnosticos = new();
            }
            else
            {
                ModelState.AddModelError(string.Empty, "No se pudieron obtener los diagnósticos iniciales.");
            }

            return Page();
        }
    }
}