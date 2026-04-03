using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Json;
using Abstracciones.Modelos;
using Abstracciones.Interfaces.Reglas;

namespace Web.Pages.Producto
{
    public class AgregarModel : PageModel
    {
        private readonly IConfiguracion _config;
        private readonly IHttpClientFactory _httpClientFactory;

        public AgregarModel(IConfiguracion config, IHttpClientFactory httpClientFactory)
        {
            _config = config;
            _httpClientFactory = httpClientFactory;
        }

        [BindProperty]
        public ProductoRequest Producto { get; set; } = new();

        public void OnGet()
        {
            // Defaults al cargar la página
            Producto.Activo = true;
            Producto.Stock = 0;
            Producto.StockMinimo = 0;
            Producto.Precio = 0.01m;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            var endpoint = _config.ObtenerMetodo("ApiEndPointsProducto", "Agregar");

            var client = _httpClientFactory.CreateClient();
            var response = await client.PostAsJsonAsync(endpoint, Producto);

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError(string.Empty, "Error al registrar el producto.");
                return Page();
            }

            TempData["Success"] = "Producto registrado correctamente.";
            return RedirectToPage("Listado");
        }
    }
}
