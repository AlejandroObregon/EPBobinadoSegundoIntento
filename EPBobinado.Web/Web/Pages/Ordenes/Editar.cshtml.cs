using Abstracciones.Interfaces.Reglas;
using Abstracciones.Modelos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Json;
using System.Text.Json;

namespace Web.Pages.Ordenes
{
    public class EditarModel : PageModel
    {
        private readonly IConfiguracion _config;
        private readonly IHttpClientFactory _httpClientFactory;

        public EditarModel(IConfiguracion config, IHttpClientFactory httpClientFactory)
        {
            _config = config;
            _httpClientFactory = httpClientFactory;
        }

        [BindProperty]
        public OrdenServicioRequest Orden { get; set; }

        public List<MotorResponse> Motores { get; set; } = new();
        public List<UsuarioResponse> Tecnicos { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            if (id <= 0) return NotFound();

            var endpoint = _config.ObtenerMetodo("ApiEndPointsOrdenServicio", "ObtenerPorId");
            var client = _httpClientFactory.CreateClient();
            var respuesta = await client.GetAsync(string.Format(endpoint, id));

            if (!respuesta.IsSuccessStatusCode) return NotFound();

            var opciones = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var response = JsonSerializer.Deserialize<OrdenServicioResponse>(
                await respuesta.Content.ReadAsStringAsync(), opciones);

            if (response == null) return NotFound();

            Orden = new OrdenServicioRequest
            {
                MotorId = response.MotorId,
                Estado = response.Estado,
                TecnicoId = response.IdTecnico
            };

            ViewData["OrdenId"] = response.Id;
            ViewData["CreadoEn"] = response.CreadoEn.ToString("dd/MM/yyyy HH:mm");

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

            var endpoint = _config.ObtenerMetodo("ApiEndPointsOrdenServicio", "Editar");
            var client = _httpClientFactory.CreateClient();
            var respuesta = await client.PutAsJsonAsync(string.Format(endpoint, id), Orden);

            if (!respuesta.IsSuccessStatusCode)
            {
                ModelState.AddModelError(string.Empty, "Error al actualizar la orden de servicio.");
                await CargarSelectsAsync();
                return Page();
            }

            TempData["MensajeExito"] = "Orden de servicio actualizada correctamente.";
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
        }
    }
}