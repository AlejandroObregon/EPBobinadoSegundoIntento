using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Json;
using System.Text.Json;
using Abstracciones.Modelos;
using Abstracciones.Interfaces.Reglas;

namespace Web.Pages.DiagnosticoInicial
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
        public DiagnosticoInicialRequest Diagnostico { get; set; } = new();

        public List<OrdenServicioResponse> Ordenes { get; set; } = new();

        public async Task OnGetAsync() => await CargarSelectsAsync();

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await CargarSelectsAsync();
                return Page();
            }

            var endpoint = _config.ObtenerMetodo("ApiEndPointsDiagnosticoInicial", "Agregar");
            var client = _httpClientFactory.CreateClient();
            var response = await client.PostAsJsonAsync(endpoint, Diagnostico);

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError(string.Empty, "Error al registrar el diagnóstico inicial.");
                await CargarSelectsAsync();
                return Page();
            }

            // ── Cambio automático de estado: Pendiente → En diagnóstico ──────
            await ActualizarEstadoOrdenAsync(client, Diagnostico.OrdenId, "En diagnóstico");

            TempData["Success"] = "Diagnóstico inicial registrado. La orden pasó a \"En diagnóstico\" automáticamente.";
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

                // 1. Obtener la orden actual para preservar MotorId y TecnicoId
                var getUrl = string.Format(_config.ObtenerMetodo("ApiEndPointsOrdenServicio", "ObtenerPorId"), ordenId);
                var getResp = await client.GetAsync(getUrl);
                if (!getResp.IsSuccessStatusCode) return;

                var orden = System.Text.Json.JsonSerializer.Deserialize<OrdenServicioResponse>(
                    await getResp.Content.ReadAsStringAsync(), opciones);
                if (orden == null) return;

                // Solo actualizar si el estado actual es anterior (no sobreescribir estados avanzados)
                var estadosAnteriores = new[] { "Pendiente" };
                if (!estadosAnteriores.Contains(orden.Estado)) return;

                // 2. PUT con el nuevo estado
                var request = new OrdenServicioRequest
                {
                    MotorId = orden.MotorId,
                    TecnicoId = orden.TecnicoId,
                    Estado = nuevoEstado
                };
                var putUrl = string.Format(_config.ObtenerMetodo("ApiEndPointsOrdenServicio", "Editar"), ordenId);
                await client.PutAsJsonAsync(putUrl, request);
            }
            catch { /* No bloquear el flujo principal si falla el cambio de estado */ }
        }

    }
}