using Abstracciones.Interfaces.Reglas;
using Abstracciones.Modelos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Json;
using System.Text.Json;

namespace Web.Pages.DiagnosticoInicial
{
    public class EditarModel : PageModel
    {
        private readonly IConfiguracion _config;
        private readonly IHttpClientFactory _httpClientFactory;

        public EditarModel(IConfiguracion config, IHttpClientFactory httpClientFactory)
        {
            _config = config;
            _httpClientFactory = httpClientFactory;
        }

        [BindProperty]
        public DiagnosticoInicialRequest Diagnostico { get; set; }

        public List<OrdenServicioResponse> Ordenes { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            if (id <= 0) return NotFound();

            var endpoint = _config.ObtenerMetodo("ApiEndPointsDiagnosticoInicial", "ObtenerPorId");
            var client = _httpClientFactory.CreateClient();
            var respuesta = await client.GetAsync(string.Format(endpoint, id));

            if (!respuesta.IsSuccessStatusCode) return NotFound();

            var opciones = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var response = JsonSerializer.Deserialize<DiagnosticoInicialResponse>(
                await respuesta.Content.ReadAsStringAsync(), opciones);

            if (response == null) return NotFound();

            Diagnostico = new DiagnosticoInicialRequest
            {
                OrdenId = response.OrdenId,
                Descripcion = response.Descripcion
            };

            ViewData["DiagnosticoId"] = response.Id;
            ViewData["CreadoEn"] = response.CreadoEn.ToString("dd/MM/yyyy HH:mm");

            await CargarSelectsAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            if (!ModelState.IsValid)
            {
                await CargarSelectsAsync();
                return Page();
            }

            var endpoint = _config.ObtenerMetodo("ApiEndPointsDiagnosticoInicial", "Editar");
            var client = _httpClientFactory.CreateClient();
            var respuesta = await client.PutAsJsonAsync(string.Format(endpoint, id), Diagnostico);

            if (!respuesta.IsSuccessStatusCode)
            {
                ModelState.AddModelError(string.Empty, "Error al actualizar el diagnóstico inicial.");
                await CargarSelectsAsync();
                return Page();
            }

            TempData["MensajeExito"] = "Diagnóstico inicial actualizado correctamente.";
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