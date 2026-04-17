using Abstracciones.Interfaces.Reglas;
using Abstracciones.Modelos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace Web.Pages.ModeloMotor
{
    public class EliminarModel : PageModelBase
    {
        private readonly IConfiguracion _config;

        public ModeloMotorResponse Modelo { get; set; } = default!;

        public EliminarModel(IConfiguracion config)
        {
            _config = config;
        }

        public async Task OnGetAsync(int id)
        {
            if (id <= 0)
                return;

            string endpoint = _config.ObtenerMetodo("ApiEndPointsModeloMotor", "ObtenerPorId");
            using var client = new HttpClient();
            var respuesta = await client.GetAsync(string.Format(endpoint, id));
            if (!respuesta.IsSuccessStatusCode) return;

            var json = await respuesta.Content.ReadAsStringAsync();
            var opciones = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            Modelo = JsonSerializer.Deserialize<ModeloMotorResponse>(json, opciones)!;
        }

        public async Task<IActionResult> OnPostEliminarAsync(int id)
        {
            if (id <= 0)
                return NotFound();

            string endpoint = _config.ObtenerMetodo("ApiEndPointsModeloMotor", "Eliminar");
            using var client = new HttpClient();
            var respuesta = await client.DeleteAsync(string.Format(endpoint, id));

            if (respuesta.IsSuccessStatusCode)
            {
                TempData["MensajeExito"] = "Modelo de motor eliminado correctamente.";
                return RedirectToPage("Listado");
            }

            return StatusCode((int)respuesta.StatusCode);
        }
    }
}