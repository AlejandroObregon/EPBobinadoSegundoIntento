using Abstracciones.Modelos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net;
using System.Text.Json;

namespace Web.Pages.DiagnosticoTecnico
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

        public List<DiagnosticoTecnicoResponse> Diagnosticos { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            var section = _config.GetSection("ApiEndPointsDiagnosticoTecnico");
            var urlBase = (section.GetValue<string>("UrlBase") ?? "").Trim();

            string? endpoint = null;
            foreach (var m in section.GetSection("Metodos").GetChildren())
            {
                if (string.Equals(m.GetValue<string>("Nombre"), "Obtener", StringComparison.OrdinalIgnoreCase))
                {
                    endpoint = $"{urlBase.TrimEnd('/')}/{(m.GetValue<string>("Valor") ?? "").TrimStart('/')}";
                    break;
                }
            }

            if (string.IsNullOrWhiteSpace(endpoint))
            {
                ModelState.AddModelError(string.Empty, "No se encontró la configuración del endpoint.");
                return Page();
            }

            var client = _httpClientFactory.CreateClient();
            using var response = await client.GetAsync(endpoint);

            if (response.IsSuccessStatusCode && response.StatusCode == HttpStatusCode.OK)
            {
                var opciones = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                Diagnosticos = JsonSerializer.Deserialize<List<DiagnosticoTecnicoResponse>>(
                    await response.Content.ReadAsStringAsync(), opciones) ?? new();
            }
            else if (response.StatusCode != HttpStatusCode.NoContent)
            {
                ModelState.AddModelError(string.Empty, "No se pudieron obtener los diagnósticos técnicos.");
            }

            return Page();
        }
    }
}