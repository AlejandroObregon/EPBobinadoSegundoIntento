using Abstracciones.Modelos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net;
using System.Text.Json;

namespace Web.Pages.Cliente
{
    public class MisOrdenesModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _config;

        public MisOrdenesModel(IHttpClientFactory httpClientFactory, IConfiguration config)
        {
            _httpClientFactory = httpClientFactory;
            _config = config;
        }

        public List<OrdenServicioResponse> Ordenes { get; set; } = new();
        public List<MotorResponse> MisMotores { get; set; } = new();

        // Contadores por estado para las chips
        public int TotalPendiente => Ordenes.Count(o => o.Estado == "Pendiente");
        public int TotalDiagnostico => Ordenes.Count(o => o.Estado == "En diagnóstico");
        public int TotalReparacion => Ordenes.Count(o => o.Estado == "En reparación");
        public int TotalCompletado => Ordenes.Count(o => o.Estado == "Completado");
        public int TotalCancelado => Ordenes.Count(o => o.Estado == "Cancelado");

        public async Task<IActionResult> OnGetAsync()
        {
            var idStr = HttpContext.Session.GetString("UsuarioId");
            if (string.IsNullOrWhiteSpace(idStr))
                return Redirect("/Auth/Login");

            int.TryParse(idStr, out int usuarioId);
            var opciones = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var client = _httpClientFactory.CreateClient();

            // 1. Obtener los motores del cliente para filtrar órdenes
            try
            {
                var motorEndpoint = ObtenerUrl("ApiEndPointsMotor", "Obtener");
                var resp = await client.GetAsync(motorEndpoint);
                if (resp.IsSuccessStatusCode && resp.StatusCode == HttpStatusCode.OK)
                {
                    var todos = JsonSerializer.Deserialize<List<MotorResponse>>(
                        await resp.Content.ReadAsStringAsync(), opciones) ?? new();
                    MisMotores = todos.Where(m => m.UsuarioId == usuarioId).ToList();
                }
            }
            catch { }

            // 2. Obtener órdenes y filtrar solo las de los motores del cliente
            var misIds = MisMotores.Select(m => m.Id).ToHashSet();
            try
            {
                var ordenEndpoint = ObtenerUrl("ApiEndPointsOrdenServicio", "Obtener");
                var resp = await client.GetAsync(ordenEndpoint);
                if (resp.IsSuccessStatusCode && resp.StatusCode == HttpStatusCode.OK)
                {
                    var todas = JsonSerializer.Deserialize<List<OrdenServicioResponse>>(
                        await resp.Content.ReadAsStringAsync(), opciones) ?? new();
                    Ordenes = todas
                        .Where(o => misIds.Contains(o.MotorId))
                        .OrderByDescending(o => o.CreadoEn)
                        .ToList();
                }
                else if (resp.StatusCode == HttpStatusCode.NoContent)
                {
                    Ordenes = new();
                }
            }
            catch { }

            return Page();
        }

        private string ObtenerUrl(string seccionKey, string metodoNombre)
        {
            var section = _config.GetSection(seccionKey);
            var urlBase = section.GetValue<string>("UrlBase")?.Trim() ?? "";
            foreach (var m in section.GetSection("Metodos").GetChildren())
            {
                if (string.Equals(m.GetValue<string>("Nombre"), metodoNombre, StringComparison.OrdinalIgnoreCase))
                    return $"{urlBase.TrimEnd('/')}/{(m.GetValue<string>("Valor") ?? "").TrimStart('/')}";
            }
            return "";
        }
    }
}