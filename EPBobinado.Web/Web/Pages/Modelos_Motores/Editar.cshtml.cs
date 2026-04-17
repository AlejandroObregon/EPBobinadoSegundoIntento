using Abstracciones.Interfaces.Reglas;
using Abstracciones.Modelos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Json;
using System.Text.Json;

namespace Web.Pages.ModeloMotor
{
    public class EditarModel : PageModelBase
    {
        private readonly IConfiguracion _config;

        public EditarModel(IConfiguracion config)
        {
            _config = config;
        }

        [BindProperty]
        public ModeloMotorRequest Modelo { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            if (id <= 0)
                return NotFound();

            var endpoint = _config.ObtenerMetodo("ApiEndPointsModeloMotor", "ObtenerPorId");

            using var client = new HttpClient();
            var respuesta = await client.GetAsync(string.Format(endpoint, id));
            if (!respuesta.IsSuccessStatusCode)
                return NotFound();

            var json = await respuesta.Content.ReadAsStringAsync();
            var opciones = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            var response = JsonSerializer.Deserialize<ModeloMotorResponse>(json, opciones);
            if (response == null)
                return NotFound();

            Modelo = new ModeloMotorRequest
            {
                Nombre = response.Nombre,
                Especificaciones = response.Especificaciones
            };

            ViewData["ModeloId"] = response.Id;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            if (!ModelState.IsValid)
                return Page();

            var endpoint = _config.ObtenerMetodo("ApiEndPointsModeloMotor", "Editar");

            using var client = new HttpClient();
            var respuesta = await client.PutAsJsonAsync(string.Format(endpoint, id), Modelo);

            if (!respuesta.IsSuccessStatusCode)
            {
                ModelState.AddModelError(string.Empty, "Error al actualizar el modelo de motor.");
                return Page();
            }

            TempData["MensajeExito"] = "Modelo de motor actualizado correctamente.";
            return RedirectToPage("Listado");
        }
    }
}