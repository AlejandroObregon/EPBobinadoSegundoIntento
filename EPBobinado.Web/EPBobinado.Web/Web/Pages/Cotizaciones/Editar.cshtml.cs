using Abstracciones.Interfaces.Reglas;
using Abstracciones.Modelos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Json;
using System.Text.Json;

namespace Web.Pages.Cotizaciones
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
        public CotizacionRequest Cotizacion { get; set; } = new();

        public List<OrdenServicioResponse> Ordenes { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            if (id <= 0) return NotFound();

            var endpoint = _config.ObtenerMetodo("ApiEndPointsCotizacion", "ObtenerPorId");
            var client = _httpClientFactory.CreateClient();
            var respuesta = await client.GetAsync(string.Format(endpoint, id));

            if (!respuesta.IsSuccessStatusCode) return NotFound();

            var json = await respuesta.Content.ReadAsStringAsync();
            var opciones = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var response = JsonSerializer.Deserialize<CotizacionResponse>(json, opciones);

            if (response == null) return NotFound();

            Cotizacion = new CotizacionRequest
            {
                OrdenId = response.OrdenId,
                Total = response.Total,
                Aprobada = response.Aprobada
            };

            ViewData["CotizacionId"] = response.Id;
            ViewData["CreadoEn"] = response.CreadoEn.ToString("dd/MM/yyyy");

            await CargarOrdenesAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            if (!ModelState.IsValid)
            {
                await CargarOrdenesAsync();
                return Page();
            }

            var endpoint = _config.ObtenerMetodo("ApiEndPointsCotizacion", "Editar");
            var client = _httpClientFactory.CreateClient();
            var respuesta = await client.PutAsJsonAsync(string.Format(endpoint, id), Cotizacion);

            if (!respuesta.IsSuccessStatusCode)
            {
                ModelState.AddModelError(string.Empty, "Error al actualizar la cotización.");
                await CargarOrdenesAsync();
                return Page();
            }

            TempData["MensajeExito"] = "Cotización actualizada correctamente.";
            return RedirectToPage("Listado");
        }

        private async Task CargarOrdenesAsync()
        {
            try
            {
                var endpoint = _config.ObtenerMetodo("ApiEndPointsOrdenServicio", "Obtener");
                var client = _httpClientFactory.CreateClient();
                var resp = await client.GetAsync(endpoint);
                if (resp.IsSuccessStatusCode)
                {
                    var json = await resp.Content.ReadAsStringAsync();
                    var opciones = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    Ordenes = JsonSerializer.Deserialize<List<OrdenServicioResponse>>(json, opciones) ?? new();
                }
            }
            catch { Ordenes = new(); }
        }
    }
}