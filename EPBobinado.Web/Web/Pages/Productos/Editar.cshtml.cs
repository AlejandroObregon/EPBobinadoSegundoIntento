using Abstracciones.Interfaces.Reglas;
using Abstracciones.Modelos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Json;
using System.Text.Json;

namespace Web.Pages.Producto
{
    public class EditarModel : PageModelBase
    {
        private readonly IConfiguracion _config;

        public EditarModel(IConfiguracion config)
        {
            _config = config;
        }

        [BindProperty]
        public ProductoRequest Producto { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            if (id <= 0)
                return NotFound();

            var endpoint = _config.ObtenerMetodo("ApiEndPointsProducto", "ObtenerPorId");

            using var client = new HttpClient();
            var respuesta = await client.GetAsync(string.Format(endpoint, id));
            if (!respuesta.IsSuccessStatusCode)
                return NotFound();

            var json = await respuesta.Content.ReadAsStringAsync();
            var opciones = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            // Se obtiene ProductoResponse y se mapea a ProductoRequest para el form
            var response = JsonSerializer.Deserialize<ProductoResponse>(json, opciones);
            if (response == null)
                return NotFound();

            Producto = new ProductoRequest
            {
                Nombre = response.Nombre,
                Categoria = response.Categoria,
                Precio = response.Precio,
                Stock = response.Stock,
                StockMinimo = response.StockMinimo,
                Activo = response.Activo
            };

            // Guardamos el Id en ViewData para usarlo en el POST
            ViewData["ProductoId"] = response.Id;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            if (!ModelState.IsValid)
                return Page();

            var endpoint = _config.ObtenerMetodo("ApiEndPointsProducto", "Editar");

            using var client = new HttpClient();
            var respuesta = await client.PutAsJsonAsync(string.Format(endpoint, id), Producto);

            if (!respuesta.IsSuccessStatusCode)
            {
                ModelState.AddModelError(string.Empty, "Error al actualizar el producto.");
                return Page();
            }

            TempData["MensajeExito"] = "Producto actualizado correctamente.";
            return RedirectToPage("Listado");
        }
    }
}