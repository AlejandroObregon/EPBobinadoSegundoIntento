using Abstracciones.Interfaces.Reglas;
using Abstracciones.Modelos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Json;
using System.Text.Json;

namespace Web.Pages.Proveedores
{
    public class EditarModel : PageModelBase
    {
        private readonly IConfiguracion _config;
        private readonly IHttpClientFactory _httpClientFactory;

        public EditarModel(IConfiguracion config, IHttpClientFactory httpClientFactory)
        {
            _config = config;
            _httpClientFactory = httpClientFactory;
        }

        [BindProperty]
        public ProveedorRequest Proveedor { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            if (id <= 0)
                return NotFound();

            var endpoint = _config.ObtenerMetodo("ApiEndPointsProveedor", "ObtenerPorId");
            var client = _httpClientFactory.CreateClient();
            var respuesta = await client.GetAsync(string.Format(endpoint, id));

            if (!respuesta.IsSuccessStatusCode)
                return NotFound();

            var json = await respuesta.Content.ReadAsStringAsync();
            var opciones = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            var response = JsonSerializer.Deserialize<ProveedorResponse>(json, opciones);
            if (response == null)
                return NotFound();

            Proveedor = new ProveedorRequest
            {
                Nombre = response.Nombre,
                Contacto = response.Contacto
            };

            ViewData["ProveedorId"] = response.Id;

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

            var endpoint = _config.ObtenerMetodo("ApiEndPointsProveedor", "Editar");
            var client = _httpClientFactory.CreateClient();
            var respuesta = await client.PutAsJsonAsync(string.Format(endpoint, id), Proveedor);

            if (!respuesta.IsSuccessStatusCode)
            {
                ModelState.AddModelError(string.Empty, "Error al actualizar el Proveedor.");
                await CargarSelectsAsync();
                return Page();
            }

            TempData["MensajeExito"] = "Proveedor actualizado correctamente.";
            return RedirectToPage("Listado");
        }

        private async Task CargarSelectsAsync()
        {
            var opciones = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var client = _httpClientFactory.CreateClient();
        }
    }
}