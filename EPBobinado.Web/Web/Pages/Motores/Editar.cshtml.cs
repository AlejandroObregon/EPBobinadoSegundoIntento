using Abstracciones.Interfaces.Reglas;
using Abstracciones.Modelos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Json;
using System.Text.Json;

namespace Web.Pages.Motores
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
        public MotorRequest Motor { get; set; }

        public List<UsuarioResponse> Usuarios { get; set; } = new();
        public List<ModeloMotorResponse> Modelos { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            if (id <= 0)
                return NotFound();

            var endpoint = _config.ObtenerMetodo("ApiEndPointsMotor", "ObtenerPorId");
            var client = _httpClientFactory.CreateClient();
            var respuesta = await client.GetAsync(string.Format(endpoint, id));

            if (!respuesta.IsSuccessStatusCode)
                return NotFound();

            var json = await respuesta.Content.ReadAsStringAsync();
            var opciones = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            var response = JsonSerializer.Deserialize<MotorResponse>(json, opciones);
            if (response == null)
                return NotFound();

            Motor = new MotorRequest
            {
                UsuarioId = response.UsuarioId,
                ModeloId = response.ModeloId,
                NumeroSerie = response.NumeroSerie
            };

            ViewData["MotorId"] = response.Id;

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

            var endpoint = _config.ObtenerMetodo("ApiEndPointsMotor", "Editar");
            var client = _httpClientFactory.CreateClient();
            var respuesta = await client.PutAsJsonAsync(string.Format(endpoint, id), Motor);

            if (!respuesta.IsSuccessStatusCode)
            {
                ModelState.AddModelError(string.Empty, "Error al actualizar el motor.");
                await CargarSelectsAsync();
                return Page();
            }

            TempData["MensajeExito"] = "Motor actualizado correctamente.";
            return RedirectToPage("Listado");
        }

        private async Task CargarSelectsAsync()
        {
            var opciones = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var client = _httpClientFactory.CreateClient();

            try
            {
                var endpointUsuarios = _config.ObtenerMetodo("ApiEndPointsUsuario", "Obtener");
                var resp = await client.GetAsync(endpointUsuarios);
                if (resp.IsSuccessStatusCode)
                {
                    var json = await resp.Content.ReadAsStringAsync();
                    Usuarios = JsonSerializer.Deserialize<List<UsuarioResponse>>(json, opciones) ?? new();
                }
            }
            catch { Usuarios = new(); }

            try
            {
                var endpointModelos = _config.ObtenerMetodo("ApiEndPointsModeloMotor", "Obtener");
                var resp = await client.GetAsync(endpointModelos);
                if (resp.IsSuccessStatusCode)
                {
                    var json = await resp.Content.ReadAsStringAsync();
                    Modelos = JsonSerializer.Deserialize<List<ModeloMotorResponse>>(json, opciones) ?? new();
                }
            }
            catch { Modelos = new(); }
        }
    }
}