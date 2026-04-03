using Abstracciones.Interfaces.Reglas;
using Abstracciones.Modelos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace Web.Pages.Producto
{
    public class EliminarModel : PageModel
    {
        private readonly IConfiguracion _config;

        public ProductoResponse Producto { get; set; } = default!;

        public EliminarModel(IConfiguracion config)
        {
            _config = config;
        }

        public async Task OnGetAsync(int id)
        {
            if (id <= 0)
                return;

            string endpoint = _config.ObtenerMetodo("ApiEndPointsProducto", "ObtenerPorId");
            using var client = new HttpClient();
            var respuesta = await client.GetAsync(string.Format(endpoint, id));
            if (!respuesta.IsSuccessStatusCode) return;

            var json = await respuesta.Content.ReadAsStringAsync();
            var opciones = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            Producto = JsonSerializer.Deserialize<ProductoResponse>(json, opciones)!;
        }

        public async Task<IActionResult> OnPostEliminarAsync(int id)
        {
            if (id <= 0)
                return NotFound();

            string endpoint = _config.ObtenerMetodo("ApiEndPointsProducto", "Eliminar");
            using var client = new HttpClient();
            var respuesta = await client.DeleteAsync(string.Format(endpoint, id));

            if (respuesta.IsSuccessStatusCode)
            {
                TempData["MensajeExito"] = "El producto se eliminó correctamente.";
                return RedirectToPage("Listado");
            }

            return StatusCode((int)respuesta.StatusCode);
        }
    }
}