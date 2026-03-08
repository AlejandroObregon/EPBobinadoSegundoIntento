using Abstracciones.Interfaces.Reglas;
using Abstracciones.Modelos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Json;
using System.Text.Json;
using Web.Pages;

namespace Web.Pages.Cliente
{
    public class MiPerfilModel : PageModelBase
    {
        private readonly IConfiguracion _config;
        private readonly IHttpClientFactory _httpClientFactory;

        public MiPerfilModel(IConfiguracion config, IHttpClientFactory httpClientFactory)
        {
            _config = config;
            _httpClientFactory = httpClientFactory;
        }

        [BindProperty]
        public PerfilRequest Perfil { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            var auth = VerificarSesion();
            if (auth != null) return auth;

            var endpoint = string.Format(_config.ObtenerMetodo("ApiEndPointsUsuario", "ObtenerPorId"), UsuarioId);
            var client = _httpClientFactory.CreateClient();
            var resp = await client.GetAsync(endpoint);

            if (!resp.IsSuccessStatusCode)
            {
                ModelState.AddModelError(string.Empty, "No se pudo cargar tu perfil.");
                return Page();
            }

            var opciones = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var usuario = JsonSerializer.Deserialize<UsuarioResponse>(
                               await resp.Content.ReadAsStringAsync(), opciones);

            if (usuario == null) return NotFound();

            Perfil = new PerfilRequest
            {
                Nombre = usuario.Nombre,
                Email = usuario.Email,
                Cedula = usuario.Cedula,
                Telefono = usuario.Telefono ?? "",
                // Guardamos los campos que NO debe poder editar el cliente
                PasswordHash = usuario.PasswordHash,
                RolId = usuario.RolId,
                Activo = usuario.Activo
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var auth = VerificarSesion();
            if (auth != null) return auth;

            // Ignorar validación de campos protegidos (no vienen del form visible)
            ModelState.Remove("Perfil.PasswordHash");
            ModelState.Remove("Perfil.RolId");
            ModelState.Remove("Perfil.Activo");

            if (!ModelState.IsValid) return Page();

            var client = _httpClientFactory.CreateClient();
            var opciones = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            // SIEMPRE re-obtener datos protegidos desde la API — nunca confiar en hidden fields
            var getEndpoint = string.Format(_config.ObtenerMetodo("ApiEndPointsUsuario", "ObtenerPorId"), UsuarioId);
            var getResp = await client.GetAsync(getEndpoint);

            if (!getResp.IsSuccessStatusCode)
            {
                ModelState.AddModelError(string.Empty, "No se pudo verificar tu cuenta. Intenta de nuevo.");
                return Page();
            }

            var usuarioActual = JsonSerializer.Deserialize<UsuarioResponse>(
                await getResp.Content.ReadAsStringAsync(), opciones);

            if (usuarioActual == null)
            {
                ModelState.AddModelError(string.Empty, "No se pudo cargar tu cuenta.");
                return Page();
            }

            // Construir request con los datos editables del cliente + datos protegidos del API
            var request = new UsuarioRequest
            {
                Nombre = Perfil.Nombre,
                Email = Perfil.Email,
                Cedula = Perfil.Cedula,
                Telefono = string.IsNullOrWhiteSpace(Perfil.Telefono) ? null : Perfil.Telefono,
                // Datos protegidos — siempre del API, nunca del form
                PasswordHash = usuarioActual.PasswordHash,
                RolId = usuarioActual.RolId,
                Activo = usuarioActual.Activo,
                DireccionId = usuarioActual.DireccionId
            };

            var endpoint = string.Format(_config.ObtenerMetodo("ApiEndPointsUsuario", "Editar"), UsuarioId);
            var putResp = await client.PutAsJsonAsync(endpoint, request);

            if (!putResp.IsSuccessStatusCode)
            {
                var apiError = await putResp.Content.ReadAsStringAsync();
                ModelState.AddModelError(string.Empty,
                    $"Error {(int)putResp.StatusCode}: {apiError}");
                return Page();
            }

            HttpContext.Session.SetString("UsuarioNombre", Perfil.Nombre);
            HttpContext.Session.SetString("UsuarioEmail", Perfil.Email);

            TempData["Success"] = "¡Perfil actualizado correctamente!";
            return RedirectToPage("MiPerfil");
        }
    }

    public class PerfilRequest
    {
        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "El nombre es requerido")]
        [System.ComponentModel.DataAnnotations.StringLength(100)]
        public string Nombre { get; set; } = "";

        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "El correo es requerido")]
        [System.ComponentModel.DataAnnotations.EmailAddress(ErrorMessage = "Formato de correo inválido")]
        public string Email { get; set; } = "";

        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "La cédula es requerida")]
        [System.ComponentModel.DataAnnotations.StringLength(20)]
        public string Cedula { get; set; } = "";

        [System.ComponentModel.DataAnnotations.Phone(ErrorMessage = "Formato de teléfono inválido (ej: 8888-7777)")]
        public string? Telefono { get; set; }

        // Campos protegidos (ocultos en el form, no editables por el cliente)
        public string PasswordHash { get; set; } = "";
        public int RolId { get; set; }
        public bool Activo { get; set; }
    }
}