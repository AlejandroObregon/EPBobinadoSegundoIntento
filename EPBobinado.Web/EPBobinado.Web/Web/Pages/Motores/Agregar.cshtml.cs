using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Json;
using System.Text.Json;
using Abstracciones.Modelos;
using Abstracciones.Interfaces.Reglas;

namespace Web.Pages.Motores
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
        public MotorRequest Motor { get; set; } = new();

        public List<UsuarioResponse> Usuarios { get; set; } = new();
        public List<ModeloMotorResponse> Modelos { get; set; } = new();

        public async Task OnGetAsync()
        {
            await CargarSelectsAsync();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await CargarSelectsAsync();
                return Page();
            }

            var endpoint = _config.ObtenerMetodo("ApiEndPointsMotor", "Agregar");

            var client = _httpClientFactory.CreateClient();
            var response = await client.PostAsJsonAsync(endpoint, Motor);

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError(string.Empty, "Error al registrar el motor.");
                await CargarSelectsAsync();
                return Page();
            }

            TempData["Success"] = "Motor registrado correctamente.";
            return RedirectToPage("Listado");
        }

        private async Task CargarSelectsAsync()
        {
            var opciones = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var client = _httpClientFactory.CreateClient();

            // Cargar usuarios
            try
            {
                var endpointUsuarios = _config.ObtenerMetodo("ApiEndPointsUsuario", "Obtener");
                var respUsuarios = await client.GetAsync(endpointUsuarios);
                if (respUsuarios.IsSuccessStatusCode)
                {
                    var json = await respUsuarios.Content.ReadAsStringAsync();
                    Usuarios = JsonSerializer.Deserialize<List<UsuarioResponse>>(json, opciones) ?? new();
                }
            }
            catch { Usuarios = new(); }

            // Cargar modelos de motor
            try
            {
                var endpointModelos = _config.ObtenerMetodo("ApiEndPointsModeloMotor", "Obtener");
                var respModelos = await client.GetAsync(endpointModelos);
                if (respModelos.IsSuccessStatusCode)
                {
                    var json = await respModelos.Content.ReadAsStringAsync();
                    Modelos = JsonSerializer.Deserialize<List<ModeloMotorResponse>>(json, opciones) ?? new();
                }
            }
            catch { Modelos = new(); }
        }
    }
}