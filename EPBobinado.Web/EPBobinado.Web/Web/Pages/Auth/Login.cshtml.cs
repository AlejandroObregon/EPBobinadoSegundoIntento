using Abstracciones.Interfaces.Reglas;
using Abstracciones.Modelos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace Web.Pages.Auth
{
    public class LoginModel : PageModel
    {
        private readonly IConfiguracion _config;
        private readonly IHttpClientFactory _httpClientFactory;

        public LoginModel(IConfiguracion config, IHttpClientFactory httpClientFactory)
        {
            _config = config;
            _httpClientFactory = httpClientFactory;
        }

        [BindProperty]
        public LoginRequest Credenciales { get; set; } = new();

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            var endpoint = _config.ObtenerMetodo("ApiEndPointsUsuario", "Login");

            var client = _httpClientFactory.CreateClient();
            var response = await client.PostAsJsonAsync(endpoint, Credenciales);

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError(string.Empty, "Correo o contraseña incorrectos.");
                return Page();
            }

            var json = await response.Content.ReadAsStringAsync();
            var opciones = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var usuario = JsonSerializer.Deserialize<UsuarioResponse>(json, opciones);

            if (usuario == null)
            {
                ModelState.AddModelError(string.Empty, "Error al iniciar sesión. Intente de nuevo.");
                return Page();
            }

            // Guardar sesión
            HttpContext.Session.SetString("UsuarioId", usuario.Id.ToString());
            HttpContext.Session.SetString("UsuarioNombre", usuario.Nombre);
            HttpContext.Session.SetString("UsuarioEmail", usuario.Email);
            HttpContext.Session.SetString("UsuarioNivel", usuario.RolId.ToString()); // número para comparar
            HttpContext.Session.SetString("UsuarioRolNombre", ResolverRol(usuario.RolId));

            return RedirectToPage("/HomePage/HomePage");
        }

        // RolId según BD: 1=Cliente, 2=Admin, 1002=Técnico
        private static string ResolverRol(int rolId) => rolId switch
        {
            1 => "Cliente",
            2 => "Administrador",
            1002 => "Técnico",
            _ => "Usuario"
        };
    }

    public class LoginRequest
    {
        [Required(ErrorMessage = "El correo es requerido")]
        [EmailAddress(ErrorMessage = "Formato de correo inválido")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "La contraseña es requerida")]
        public string Password { get; set; } = string.Empty;
    }
}