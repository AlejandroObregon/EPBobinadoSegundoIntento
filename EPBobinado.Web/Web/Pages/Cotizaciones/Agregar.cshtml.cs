// =====================================================
// Agregar.cshtml.cs
// =====================================================
using Abstracciones.Interfaces.Reglas;
using Abstracciones.Modelos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Json;
using System.Text.Json;

namespace Web.Pages.Cotizaciones
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
        public CotizacionRequest Cotizacion { get; set; } = new();

        public List<OrdenServicioResponse> Ordenes { get; set; } = new();

        public async Task OnGetAsync()
        {
            await CargarOrdenesAsync();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await CargarOrdenesAsync();
                return Page();
            }

            var endpoint = _config.ObtenerMetodo("ApiEndPointsCotizacion", "Agregar");
            var client = _httpClientFactory.CreateClient();
            var response = await client.PostAsJsonAsync(endpoint, Cotizacion);

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError(string.Empty, "Error al registrar la cotización.");
                await CargarOrdenesAsync();
                return Page();
            }

            TempData["MensajeExito"] = "Cotización registrada correctamente.";
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