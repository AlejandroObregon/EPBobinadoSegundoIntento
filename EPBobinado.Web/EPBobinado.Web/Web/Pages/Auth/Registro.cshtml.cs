using Abstracciones.Interfaces.Reglas;
using Abstracciones.Modelos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Net.Http.Json;

namespace Web.Pages.Auth
{
    public class RegistroModel : PageModel
    {
        private readonly IConfiguracion _config;
        private readonly IHttpClientFactory _httpClientFactory;

        public RegistroModel(IConfiguracion config, IHttpClientFactory httpClientFactory)
        {
            _config = config;
            _httpClientFactory = httpClientFactory;
        }

        [BindProperty]
        public RegistroRequest Datos { get; set; } = new();

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            if (Datos.Password != Datos.ConfirmarPassword)
            {
                ModelState.AddModelError(string.Empty, "Las contraseñas no coinciden.");
                return Page();
            }

            // Armar UsuarioRequest con RolId = 2 (Cliente) automáticamente
            var request = new UsuarioRequest
            {
                Cedula = Datos.Cedula,
                Nombre = Datos.Nombre,
                Email = Datos.Email,
                PasswordHash = Datos.Password,
                Telefono = Datos.Telefono,
                RolId = 2,      // Cliente por defecto
                Activo = true
            };

            var endpoint = _config.ObtenerMetodo("ApiEndPointsUsuario", "Agregar");

            var client = _httpClientFactory.CreateClient();
            var response = await client.PostAsJsonAsync(endpoint, request);

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError(string.Empty, "Error al registrar el usuario. El correo o cédula ya pueden estar en uso.");
                return Page();
            }

            TempData["Success"] = "Cuenta creada correctamente. Ya puedes iniciar sesión.";
            return RedirectToPage("Login");
        }
    }

    public class RegistroRequest
    {
        [Required(ErrorMessage = "El nombre es requerido")]
        [StringLength(100, ErrorMessage = "El nombre debe ser menor a 100 caracteres")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "La cédula es requerida")]
        [StringLength(20, ErrorMessage = "La cédula debe ser menor a 20 caracteres")]
        public string Cedula { get; set; } = string.Empty;

        [Required(ErrorMessage = "El correo es requerido")]
        [EmailAddress(ErrorMessage = "Formato de correo inválido")]
        [StringLength(100, ErrorMessage = "El correo debe ser menor a 100 caracteres")]
        public string Email { get; set; } = string.Empty;

        [StringLength(20, ErrorMessage = "El teléfono debe ser menor a 20 caracteres")]
        [Phone(ErrorMessage = "Formato de teléfono inválido")]
        public string? Telefono { get; set; }

        [Required(ErrorMessage = "La contraseña es requerida")]
        [StringLength(255, MinimumLength = 6, ErrorMessage = "La contraseña debe tener entre 6 y 255 caracteres")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Confirma tu contraseña")]
        public string ConfirmarPassword { get; set; } = string.Empty;
    }
}