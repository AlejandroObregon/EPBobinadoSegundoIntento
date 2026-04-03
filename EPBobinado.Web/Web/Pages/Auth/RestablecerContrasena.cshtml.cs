using Abstracciones.Interfaces.Reglas;
using Abstracciones.Modelos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace Web.Pages.Auth
{
    public class RestablecerContrasenaModel : PageModel
    {
        private readonly IConfiguracion _config;
        private readonly IHttpClientFactory _httpClientFactory;

        public RestablecerContrasenaModel(IConfiguracion config, IHttpClientFactory httpClientFactory)
        {
            _config = config;
            _httpClientFactory = httpClientFactory;
        }

        [BindProperty(SupportsGet = true)]
        public string Token { get; set; } = "";

        [BindProperty]
        [Required(ErrorMessage = "La nueva contraseña es requerida")]
        [MinLength(6, ErrorMessage = "Mínimo 6 caracteres")]
        public string NuevaContrasena { get; set; } = "";

        [BindProperty]
        [Required(ErrorMessage = "Confirmá tu contraseña")]
        public string ConfirmarContrasena { get; set; } = "";

        public bool TokenValido { get; set; }
        public bool Completado { get; set; }
        public int UsuarioId { get; set; }
        public string UsuarioEmail { get; set; } = "";

        public async Task<IActionResult> OnGetAsync()
        {
            if (string.IsNullOrWhiteSpace(Token))
                return Redirect("/Auth/Login");

            await ValidarTokenAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await ValidarTokenAsync();
                return Page();
            }

            if (NuevaContrasena != ConfirmarContrasena)
            {
                ModelState.AddModelError(nameof(ConfirmarContrasena), "Las contraseñas no coinciden.");
                await ValidarTokenAsync();
                return Page();
            }

            var opciones = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var client = _httpClientFactory.CreateClient();

            // 1. Validar token de nuevo en el POST
            var tokenData = await ObtenerTokenDataAsync(client, opciones);
            if (tokenData == null)
            {
                ModelState.AddModelError(string.Empty, "El enlace expiró o ya fue usado. Solicitá uno nuevo.");
                return Page();
            }

            // 2. Obtener usuario actual para preservar sus campos
            UsuarioResponse? usuario = null;
            try
            {
                var url = string.Format(_config.ObtenerMetodo("ApiEndPointsUsuario", "ObtenerPorId"), tokenData.UsuarioId);
                var resp = await client.GetAsync(url);
                if (resp.IsSuccessStatusCode)
                    usuario = JsonSerializer.Deserialize<UsuarioResponse>(
                        await resp.Content.ReadAsStringAsync(), opciones);
            }
            catch { }

            if (usuario == null)
            {
                ModelState.AddModelError(string.Empty, "No se encontró el usuario.");
                return Page();
            }

            // 3. Actualizar el usuario — enviar contraseña en texto plano.
            //    UsuarioFlujo.Editar() la hashea automáticamente (SHA256 lowercase).
            var request = new UsuarioRequest
            {
                Cedula = usuario.Cedula,
                Nombre = usuario.Nombre,
                Email = usuario.Email,
                PasswordHash = NuevaContrasena,
                RolId = usuario.RolId,
                DireccionId = usuario.DireccionId,
                Telefono = usuario.Telefono,
                Activo = usuario.Activo
            };

            var editUrl = string.Format(_config.ObtenerMetodo("ApiEndPointsUsuario", "Editar"), usuario.Id);
            var editResp = await client.PutAsJsonAsync(editUrl, request);

            if (!editResp.IsSuccessStatusCode)
            {
                ModelState.AddModelError(string.Empty, "Error al actualizar la contraseña. Intentá de nuevo.");
                await ValidarTokenAsync();
                return Page();
            }

            // 5. Marcar token como usado
            try
            {
                var usarUrl = _config.ObtenerMetodo("ApiEndPointsUsuario", "UsarTokenReset");
                await client.PostAsJsonAsync(usarUrl, new { token = Token });
            }
            catch { }

            Completado = true;
            return Page();
        }

        // ── Helpers ────────────────────────────────────────────────────
        private async Task ValidarTokenAsync()
        {
            var data = await ObtenerTokenDataAsync(_httpClientFactory.CreateClient(),
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            TokenValido = data != null;
            if (data != null)
            {
                UsuarioId = data.UsuarioId;
                UsuarioEmail = data.Email;
            }
        }

        private async Task<TokenResetData?> ObtenerTokenDataAsync(HttpClient client, JsonSerializerOptions opt)
        {
            try
            {
                var url = string.Format(
                    _config.ObtenerMetodo("ApiEndPointsUsuario", "ValidarTokenReset"),
                    Uri.EscapeDataString(Token));
                var resp = await client.GetAsync(url);
                if (!resp.IsSuccessStatusCode) return null;
                return JsonSerializer.Deserialize<TokenResetData>(
                    await resp.Content.ReadAsStringAsync(), opt);
            }
            catch { return null; }
        }

        public class TokenResetData
        {
            public int Id { get; set; }
            public int UsuarioId { get; set; }
            public string Email { get; set; } = "";
            public DateTime Expiracion { get; set; }
        }
    }
}