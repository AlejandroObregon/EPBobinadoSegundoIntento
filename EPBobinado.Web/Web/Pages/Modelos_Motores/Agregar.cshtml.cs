using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Json;
using Abstracciones.Modelos;
using Abstracciones.Interfaces.Reglas;

namespace Web.Pages.ModeloMotor
{
    public class AgregarModel : PageModelBase
    {
        private readonly IConfiguracion _config;
        private readonly IHttpClientFactory _httpClientFactory;

        public AgregarModel(IConfiguracion config, IHttpClientFactory httpClientFactory)
        {
            _config = config;
            _httpClientFactory = httpClientFactory;
        }

        [BindProperty]
        public ModeloMotorRequest Modelo { get; set; } = new();

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            var endpoint = _config.ObtenerMetodo("ApiEndPointsModeloMotor", "Agregar");

            var client = _httpClientFactory.CreateClient();
            var response = await client.PostAsJsonAsync(endpoint, Modelo);

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError(string.Empty, "Error al registrar el modelo de motor.");
                return Page();
            }

            TempData["Success"] = "Modelo de motor registrado correctamente.";
            return RedirectToPage("Listado");
        }
    }
}