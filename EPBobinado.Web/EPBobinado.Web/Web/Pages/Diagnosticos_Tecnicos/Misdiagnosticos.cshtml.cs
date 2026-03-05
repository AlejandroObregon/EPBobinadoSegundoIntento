using Abstracciones.Modelos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net;
using System.Text.Json;

namespace Web.Pages.DiagnosticoTecnico
{
    public class MisDiagnosticosModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _config;

        public MisDiagnosticosModel(IHttpClientFactory httpClientFactory, IConfiguration config)
        {
            _httpClientFactory = httpClientFactory;
            _config = config;
        }

        public List<DiagnosticoTecnicoResponse> Diagnosticos { get; set; } = new();
        public string NombreTecnico { get; set; } = "";
        public int TecnicoId { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            // Leer técnico de la sesión
            var idStr = HttpContext.Session.GetString("UsuarioId");
            if (string.IsNullOrWhiteSpace(idStr) || !int.TryParse(idStr, out int tecnicoId))
                return RedirectToPage("/Auth/Login");

            TecnicoId = tecnicoId;
            NombreTecnico = HttpContext.Session.GetString("UsuarioNombre") ?? "Técnico";

            // Obtener TODOS los diagnósticos y filtrar por TecnicoId de la orden
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
                return Page();

            var client = _httpClientFactory.CreateClient();
            using var response = await client.GetAsync(endpoint);

            if (response.IsSuccessStatusCode && response.StatusCode == HttpStatusCode.OK)
            {
                var opciones = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var todos = JsonSerializer.Deserialize<List<DiagnosticoTecnicoResponse>>(
                    await response.Content.ReadAsStringAsync(), opciones) ?? new();

                // Filtrar: solo los que pertenecen a órdenes donde TecnicoId == usuario en sesión
                Diagnosticos = todos
                    .Where(d => d.Orden?.TecnicoId == TecnicoId)
                    .OrderByDescending(d => d.CreadoEn)
                    .ToList();
            }

            return Page();
        }
    }
}