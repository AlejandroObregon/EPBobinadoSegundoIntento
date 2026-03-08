using Abstracciones.Modelos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net;
using System.Text.Json;
using Web.Pages;

namespace Web.Pages.Cliente
{
    public class MiHistorialModel : PageModelBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _config;

        public MiHistorialModel(IHttpClientFactory httpClientFactory, IConfiguration config)
        {
            _httpClientFactory = httpClientFactory;
            _config = config;
        }

        // Cada moto con sus órdenes anidadas
        public List<HistorialMotor> Historial { get; set; } = new();

        // Totales globales
        public int TotalOrdenes => Historial.Sum(h => h.Ordenes.Count);
        public int TotalCompletadas => Historial.Sum(h => h.Ordenes.Count(o => o.Estado == "Completado"));
        public int TotalMotores => Historial.Count;

        public async Task<IActionResult> OnGetAsync()
        {
            var auth = VerificarSesion();
            if (auth != null) return auth;

            var opciones = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var client = _httpClientFactory.CreateClient();

            // 1. Obtener motores del cliente
            List<MotorResponse> misMotores = new();
            try
            {
                var resp = await client.GetAsync(ObtenerUrl("ApiEndPointsMotor", "Obtener"));
                if (resp.IsSuccessStatusCode && resp.StatusCode == HttpStatusCode.OK)
                {
                    var todos = JsonSerializer.Deserialize<List<MotorResponse>>(
                        await resp.Content.ReadAsStringAsync(), opciones) ?? new();
                    misMotores = todos.Where(m => m.UsuarioId == UsuarioId).ToList();
                }
            }
            catch { }

            if (!misMotores.Any()) return Page();

            // 2. Obtener todas las órdenes y filtrar por motores del cliente
            var misIds = misMotores.Select(m => m.Id).ToHashSet();
            List<OrdenServicioResponse> misOrdenes = new();
            try
            {
                var resp = await client.GetAsync(ObtenerUrl("ApiEndPointsOrdenServicio", "Obtener"));
                if (resp.IsSuccessStatusCode && resp.StatusCode == HttpStatusCode.OK)
                {
                    var todas = JsonSerializer.Deserialize<List<OrdenServicioResponse>>(
                        await resp.Content.ReadAsStringAsync(), opciones) ?? new();
                    misOrdenes = todas.Where(o => misIds.Contains(o.MotorId)).ToList();
                }
            }
            catch { }

            // 3. Armar el historial agrupado por motor
            Historial = misMotores
                .OrderByDescending(m => m.CreadoEn)
                .Select(m => new HistorialMotor
                {
                    Motor = m,
                    Ordenes = misOrdenes
                        .Where(o => o.MotorId == m.Id)
                        .OrderByDescending(o => o.CreadoEn)
                        .ToList()
                })
                .ToList();

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

    public class HistorialMotor
    {
        public MotorResponse Motor { get; set; } = null!;
        public List<OrdenServicioResponse> Ordenes { get; set; } = new();

        public int TotalOrdenes => Ordenes.Count;
        public int Completadas => Ordenes.Count(o => o.Estado == "Completado");
        public OrdenServicioResponse? UltimaOrden => Ordenes.FirstOrDefault();
    }
}