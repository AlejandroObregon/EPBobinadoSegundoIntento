using Abstracciones.Interfaces.Reglas;
using Abstracciones.Modelos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace Web.Pages.Motores
{
    public class EliminarModel : PageModel
    {
        private readonly IConfiguracion _config;

        public MotorResponse Motor { get; set; } = default!;

        public EliminarModel(IConfiguracion config)
        {
            _config = config;
        }

        public async Task OnGetAsync(int id)
        {
            if (id <= 0)
                return;

            string endpoint = _config.ObtenerMetodo("ApiEndPointsMotor", "ObtenerPorId");
            using var client = new HttpClient();
            var respuesta = await client.GetAsync(string.Format(endpoint, id));
            if (!respuesta.IsSuccessStatusCode) return;

            var json = await respuesta.Content.ReadAsStringAsync();
            var opciones = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            Motor = JsonSerializer.Deserialize<MotorResponse>(json, opciones)!;
        }

        public async Task<IActionResult> OnPostEliminarAsync(int id)
        {
            if (id <= 0)
                return NotFound();

            string endpoint = _config.ObtenerMetodo("ApiEndPointsMotor", "Eliminar");
            using var client = new HttpClient();
            var respuesta = await client.DeleteAsync(string.Format(endpoint, id));

            if (respuesta.IsSuccessStatusCode)
            {
                TempData["MensajeExito"] = "Motor eliminado correctamente.";
                return RedirectToPage("Listado");
            }

            return StatusCode((int)respuesta.StatusCode);
        }
    }
}