using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Json;
using System.Text.Json;
using Abstracciones.Modelos;
using Abstracciones.Interfaces.Reglas;

namespace Web.Pages.DiagnosticoTecnico
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
        public DiagnosticoTecnicoRequest Diagnostico { get; set; } = new();

        public List<OrdenServicioResponse> Ordenes { get; set; } = new();

        public async Task OnGetAsync() => await CargarSelectsAsync();

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await CargarSelectsAsync();
                return Page();
            }

            var endpoint = _config.ObtenerMetodo("ApiEndPointsDiagnosticoTecnico", "Agregar");
            var client = _httpClientFactory.CreateClient();
            var response = await client.PostAsJsonAsync(endpoint, Diagnostico);

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError(string.Empty, "Error al registrar el diagnóstico técnico.");
                await CargarSelectsAsync();
                return Page();
            }

            // ── Cambio automático de estado: En diagnóstico → En reparación ──
            await ActualizarEstadoOrdenAsync(client, Diagnostico.OrdenId, "En reparación");

            TempData["Success"] = "Diagnóstico técnico registrado. La orden pasó a \"En reparación\" automáticamente.";
            return RedirectToPage("Listado");
        }

        private async Task CargarSelectsAsync()
        {
            try
            {
                var opciones = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var client = _httpClientFactory.CreateClient();
                var resp = await client.GetAsync(_config.ObtenerMetodo("ApiEndPointsOrdenServicio", "Obtener"));
                if (resp.IsSuccessStatusCode)
                    Ordenes = JsonSerializer.Deserialize<List<OrdenServicioResponse>>(
                        await resp.Content.ReadAsStringAsync(), opciones) ?? new();
            }
            catch { Ordenes = new(); }
        }

        private async Task ActualizarEstadoOrdenAsync(HttpClient client, int ordenId, string nuevoEstado)
        {
            try
            {
                var opciones = new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true };

                var getUrl = string.Format(_config.ObtenerMetodo("ApiEndPointsOrdenServicio", "ObtenerPorId"), ordenId);
                var getResp = await client.GetAsync(getUrl);
                if (!getResp.IsSuccessStatusCode) return;

                var orden = System.Text.Json.JsonSerializer.Deserialize<OrdenServicioResponse>(
                    await getResp.Content.ReadAsStringAsync(), opciones);
                if (orden == null) return;

                // Solo avanzar si la orden está en un estado anterior
                var estadosAnteriores = new[] { "Pendiente", "En diagnóstico" };
                if (!estadosAnteriores.Contains(orden.Estado)) return;

                var request = new OrdenServicioRequest
                {
                    MotorId = orden.MotorId,
                    TecnicoId = orden.IdTecnico,
                    Estado = nuevoEstado
                };
                var putUrl = string.Format(_config.ObtenerMetodo("ApiEndPointsOrdenServicio", "Editar"), ordenId);
                await client.PutAsJsonAsync(putUrl, request);
            }
            catch { }
        }

    }
}