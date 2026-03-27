using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Json;
using System.Text.Json;
using Abstracciones.Modelos;
using Abstracciones.Interfaces.Reglas;

namespace Web.Pages.Ordenes
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
        public OrdenServicioRequest Orden { get; set; } = new();

        public List<MotorResponse> Motores { get; set; } = new();
        public List<UsuarioResponse> Tecnicos { get; set; } = new();
        public List<UsuarioResponse> Clientes { get; set; } = new();

        public async Task OnGetAsync() => await CargarSelectsAsync();

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await CargarSelectsAsync();
                return Page();
            }

            var endpoint = _config.ObtenerMetodo("ApiEndPointsOrdenServicio", "Agregar");
            var client = _httpClientFactory.CreateClient();
            var response = await client.PostAsJsonAsync(endpoint, Orden);

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError(string.Empty, "Error al registrar la orden de servicio.");
                await CargarSelectsAsync();
                return Page();
            }

            TempData["Success"] = "Orden de servicio registrada correctamente.";
            return RedirectToPage("Listado");
        }

        private async Task CargarSelectsAsync()
        {
            var opciones = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var client = _httpClientFactory.CreateClient();

            try
            {
                var resp = await client.GetAsync(_config.ObtenerMetodo("ApiEndPointsMotor", "Obtener"));
                if (resp.IsSuccessStatusCode)
                    Motores = JsonSerializer.Deserialize<List<MotorResponse>>(
                        await resp.Content.ReadAsStringAsync(), opciones) ?? new();
            }
            catch { Motores = new(); }

            try
            {
                var resp = await client.GetAsync(_config.ObtenerMetodo("ApiEndPointsUsuario", "Obtener"));
                if (resp.IsSuccessStatusCode)
                    Tecnicos = JsonSerializer.Deserialize<List<UsuarioResponse>>(
                        await resp.Content.ReadAsStringAsync(), opciones) ?? new();
            }
            catch { Tecnicos = new(); }

            try
            {
                var resp = await client.GetAsync(_config.ObtenerMetodo("ApiEndPointsUsuario", "Obtener"));
                if (resp.IsSuccessStatusCode)
                    Clientes = JsonSerializer.Deserialize<List<UsuarioResponse>>(
                        await resp.Content.ReadAsStringAsync(), opciones) ?? new();
            }
            catch { Clientes = new(); }
        }
    }
}