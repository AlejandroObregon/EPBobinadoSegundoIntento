using Abstracciones.Modelos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Web.Pages;
using System.Net;
using System.Text.Json;

namespace Web.Pages.Cliente
{
    public class MisOrdenesModel : PageModelBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _config;
        private readonly ILogger<MisOrdenesModel> _logger;

        public MisOrdenesModel(IHttpClientFactory httpClientFactory, IConfiguration config, ILogger<MisOrdenesModel> logger)
        {
            _httpClientFactory = httpClientFactory;
            _config = config;
            _logger = logger;
        }

        public class PagarRequest
        {
            public int FacturaId { get; set; }
            public decimal Monto { get; set; }
            public string? MetodoPago { get; set; }
        }

        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> OnPostPagar([FromBody] PagarRequest req)
        {
            var auth = VerificarSesion();
            if (auth != null) return auth;

            // Validación mínima
            if (req == null || req.FacturaId <= 0 || req.Monto <= 0)
                return BadRequest("Datos de pago inválidos");

            // Simular verificación de tarjeta: si MetodoPago == "Tarjeta" asumimos válido
            // En pruebas, si viene tarjeta y monto > 0 se marca como éxito

            var client = _httpClientFactory.CreateClient();
            var opciones = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            try
            {
                var pagoEndpoint = ObtenerUrl("ApiEndPointsPago", "Agregar");
                // Si no está configurado, devolver éxito simulado y no llamar
                if (string.IsNullOrWhiteSpace(pagoEndpoint))
                {
                    return new JsonResult(new { success = true, message = "Pago procesado (simulado)" });
                }

                var pagoReq = new Abstracciones.Modelos.PagoRequest
                {
                    FacturaId = req.FacturaId,
                    Monto = req.Monto,
                    MetodoPago = req.MetodoPago
                };

                var content = new StringContent(JsonSerializer.Serialize(pagoReq), System.Text.Encoding.UTF8, "application/json");
                var resp = await client.PostAsync(pagoEndpoint, content);
                if (resp.IsSuccessStatusCode)
                {
                    try
                    {
                        var ordenEndpoint = ObtenerUrl("ApiEndPointsOrdenServicio", "Editar", req.FacturaId);
                        if (!string.IsNullOrWhiteSpace(ordenEndpoint))
                        {
                            // Solo actualizar estado
                            var patchBody = new { Estado = "Cancelado" };
                            var patchContent = new StringContent(JsonSerializer.Serialize(patchBody), System.Text.Encoding.UTF8, "application/json");
                            var patchResp = await client.PutAsync(ordenEndpoint, patchContent);
                        }
                    }
                    catch { }

                    return new JsonResult(new { success = true, message = "Pago registrado" });
                }
                else
                {
                    var txt = await resp.Content.ReadAsStringAsync();
                    return new JsonResult(new { success = false, message = "Error al procesar el pago" });
                }
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
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
            var auth = VerificarSesion();
            if (auth != null) return auth;

            int usuarioId = UsuarioId;
            var opciones = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var client = _httpClientFactory.CreateClient();


            // 1. Obtener órdenes y filtrar solo las del cliente
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
                        .Where(o => o.IdCliente == usuarioId)
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

        private string ObtenerUrl(string seccionKey, string metodoNombre, params object[] args)
        {
            var section = _config.GetSection(seccionKey);
            var urlBase = section.GetValue<string>("UrlBase")?.Trim() ?? "";
            foreach (var m in section.GetSection("Metodos").GetChildren())
            {
                if (string.Equals(m.GetValue<string>("Nombre"), metodoNombre, StringComparison.OrdinalIgnoreCase))
                {
                    var valor = (m.GetValue<string>("Valor") ?? "").TrimStart('/');
                    var ruta = string.IsNullOrEmpty(valor) ? urlBase.TrimEnd('/') : $"{urlBase.TrimEnd('/')}/{valor}";
                    if (args != null && args.Length > 0)
                        return string.Format(ruta, args);
                    return ruta;
                }
            }
            return "";
        }
    }
}