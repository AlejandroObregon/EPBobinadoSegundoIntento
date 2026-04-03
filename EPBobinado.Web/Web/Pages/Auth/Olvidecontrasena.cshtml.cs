using Abstracciones.Interfaces.Reglas;
using Abstracciones.Modelos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using Web.Services;

namespace Web.Pages.Auth
{
    public class OlvideContrasenaModel : PageModel
    {
        private readonly IConfiguracion _config;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly EmailService _emailService;

        public OlvideContrasenaModel(IConfiguracion config,
            IHttpClientFactory httpClientFactory,
            EmailService emailService)
        {
            _config = config;
            _httpClientFactory = httpClientFactory;
            _emailService = emailService;
        }

        [BindProperty]
        [Required(ErrorMessage = "El correo es requerido")]
        [EmailAddress(ErrorMessage = "Formato de correo inválido")]
        public string Email { get; set; } = "";

        public bool Enviado { get; set; }

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            var opciones = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var client = _httpClientFactory.CreateClient();

            // 1. Buscar usuario por email
            UsuarioResponse? usuario = null;
            try
            {
                var url = _config.ObtenerMetodo("ApiEndPointsUsuario", "Obtener");
                var resp = await client.GetAsync(url);
                if (resp.IsSuccessStatusCode)
                {
                    var todos = JsonSerializer.Deserialize<List<UsuarioResponse>>(
                        await resp.Content.ReadAsStringAsync(), opciones) ?? new();
                    usuario = todos.FirstOrDefault(u =>
                        string.Equals(u.Email, Email, StringComparison.OrdinalIgnoreCase) && u.Activo);
                }
            }
            catch (Exception ex) { ModelState.AddModelError(string.Empty, $"[DEBUG] Error buscando usuario: {ex.Message}"); return Page(); }

            // Siempre mostrar mensaje genérico (no revelar si el email existe)
            Enviado = true;

            if (usuario == null) return Page();

            // 2. Generar token único
            var token = Convert.ToBase64String(System.Security.Cryptography.RandomNumberGenerator.GetBytes(48))
                             .Replace("+", "-").Replace("/", "_").Replace("=", "");
            var expiracion = DateTime.Now.AddMinutes(30);

            // 3. Guardar token en la API
            try
            {
                var urlToken = _config.ObtenerMetodo("ApiEndPointsUsuario", "CrearTokenReset");
                await client.PostAsJsonAsync(urlToken, new
                {
                    usuarioId = usuario.Id,
                    token,
                    expiracion
                });
            }
            catch (Exception ex) { ModelState.AddModelError(string.Empty, $"[DEBUG] Error guardando token: {ex.Message}"); return Page(); }

            // 4. Construir enlace de reset
            var baseUrl = $"{Request.Scheme}://{Request.Host}";
            var enlace = $"{baseUrl}/Auth/RestablecerContrasena?token={Uri.EscapeDataString(token)}";

            // 5. Enviar correo
            try
            {
                await _emailService.EnviarRecuperacionAsync(usuario.Email, usuario.Nombre, enlace);
            }
            catch (Exception ex) { ModelState.AddModelError(string.Empty, $"[DEBUG] Error enviando correo: {ex.Message}"); Enviado = false; return Page(); }

            return Page();
        }
    }
}