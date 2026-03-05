using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Json;
using System.Text.Json;
using Abstracciones.Modelos;
using Abstracciones.Interfaces.Reglas;

namespace Web.Pages.DiagnosticoTecnico
{
    public class AgregarModel : PageModel
    {
        private readonly IConfiguracion _config;
        private readonly IHttpClientFactory _httpClientFactory;

        public AgregarModel(IConfiguracion config, IHttpClientFactory httpClientFactory)
        {
            _config = config;
            _httpClientFactory = httpClientFactory;
        }

        [BindProperty]
        public DiagnosticoTecnicoRequest Diagnostico { get; set; } = new();

        public List<OrdenServicioResponse> Ordenes { get; set; } = new();

        public async Task OnGetAsync() => await CargarSelectsAsync();

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await CargarSelectsAsync();
                return Page();
            }

            var endpoint = _config.ObtenerMetodo("ApiEndPointsDiagnosticoTecnico", "Agregar");
            var client = _httpClientFactory.CreateClient();
            var response = await client.PostAsJsonAsync(endpoint, Diagnostico);

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError(string.Empty, "Error al registrar el diagnóstico técnico.");
                await CargarSelectsAsync();
                return Page();
            }

            TempData["Success"] = "Diagnóstico técnico registrado correctamente.";
            return RedirectToPage("Listado");
        }

        private async Task CargarSelectsAsync()
        {
            try
            {
                var opciones = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var client = _httpClientFactory.CreateClient();
                var resp = await client.GetAsync(_config.ObtenerMetodo("ApiEndPointsOrdenServicio", "Obtener"));
                if (resp.IsSuccessStatusCode)
                    Ordenes = JsonSerializer.Deserialize<List<OrdenServicioResponse>>(
                        await resp.Content.ReadAsStringAsync(), opciones) ?? new();
            }
            catch { Ordenes = new(); }
        }
    }
}