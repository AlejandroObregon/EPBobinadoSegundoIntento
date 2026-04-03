using Abstracciones.Modelos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Web.Pages;
using System.Net;
using System.Text.Json;

namespace Web.Pages.Cliente
{
    public class MisMotoresModel : PageModelBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _config;

        public MisMotoresModel(IHttpClientFactory httpClientFactory, IConfiguration config)
        {
            _httpClientFactory = httpClientFactory;
            _config = config;
        }

        public List<MotorResponse> Motores { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            var auth = VerificarSesion();
            if (auth != null) return auth;

            int usuarioId = UsuarioId;

            var section = _config.GetSection("ApiEndPointsMotor");
            var urlBase = (section.GetValue<string>("UrlBase") ?? "").Trim();
            string? endpoint = null;

            foreach (var m in section.GetSection("Metodos").GetChildren())
            {
                if (string.Equals(m.GetValue<string>("Nombre"), "Obtener", StringComparison.OrdinalIgnoreCase))
                {
                    var valor = m.GetValue<string>("Valor") ?? "";
                    endpoint = $"{urlBase.TrimEnd('/')}/{valor.TrimStart('/')}";
                    break;
                }
            }

            if (string.IsNullOrWhiteSpace(endpoint))
            {
                ModelState.AddModelError(string.Empty, "No se encontró el endpoint de motores.");
                return Page();
            }

            var client = _httpClientFactory.CreateClient();
            using var response = await client.GetAsync(endpoint);

            if (response.IsSuccessStatusCode && response.StatusCode == HttpStatusCode.OK)
            {
                var json = await response.Content.ReadAsStringAsync();
                var opciones = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var todos = JsonSerializer.Deserialize<List<MotorResponse>>(json, opciones) ?? new();

                // Solo los motores del cliente en sesión
                Motores = todos
                    .Where(m => m.UsuarioId == usuarioId)
                    .OrderByDescending(m => m.CreadoEn)
                    .ToList();
            }
            else if (response.StatusCode == HttpStatusCode.NoContent)
            {
                Motores = new();
            }
            else
            {
                ModelState.AddModelError(string.Empty, "No se pudieron obtener tus motos.");
            }

            return Page();
        }
    }
}