using Abstracciones.Interfaces.Reglas;
using Abstracciones.Modelos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace Web.Pages.Cotizaciones
{
    public class EliminarModel : PageModel
    {
        private readonly IConfiguracion _config;

        public CotizacionResponse? Cotizacion { get; set; }

        public EliminarModel(IConfiguracion config)
        {
            _config = config;
        }

        public async Task OnGetAsync(int id)
        {
            if (id <= 0) return;

            string endpoint = _config.ObtenerMetodo("ApiEndPointsCotizacion", "ObtenerPorId");
            using var client = new HttpClient();
            var respuesta = await client.GetAsync(string.Format(endpoint, id));
            if (!respuesta.IsSuccessStatusCode) return;

            var json = await respuesta.Content.ReadAsStringAsync();
            var opciones = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            Cotizacion = JsonSerializer.Deserialize<CotizacionResponse>(json, opciones);
        }

        public async Task<IActionResult> OnPostEliminarAsync(int id)
        {
            if (id <= 0) return NotFound();

            string endpoint = _config.ObtenerMetodo("ApiEndPointsCotizacion", "Eliminar");
            using var client = new HttpClient();
            var respuesta = await client.DeleteAsync(string.Format(endpoint, id));

            if (respuesta.IsSuccessStatusCode)
            {
                TempData["MensajeExito"] = "Cotización eliminada correctamente.";
                return RedirectToPage("Listado");
            }

            return StatusCode((int)respuesta.StatusCode);
        }
    }
}