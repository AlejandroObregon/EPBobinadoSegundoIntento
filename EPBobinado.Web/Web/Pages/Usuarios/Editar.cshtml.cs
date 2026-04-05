using Abstracciones.Interfaces.Reglas;
using Abstracciones.Modelos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Json;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace Web.Pages.Usuarios
{
    public class EditarModel : PageModel
    {
        private readonly IConfiguracion _config;

        public EditarModel(IConfiguracion config)
        {
            _config = config;
        }

        [BindProperty]
        public UsuarioRequest Usuario { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            if (id <= 0)
                return NotFound();
            
            var endpoint = _config.ObtenerMetodo("ApiEndPointsUsuario", "ObtenerPorId");

            using var client = new HttpClient();
            var respuesta = await client.GetAsync(string.Format(endpoint, id));
            if (!respuesta.IsSuccessStatusCode)
                return NotFound();

            var json = await respuesta.Content.ReadAsStringAsync();
            var opciones = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            var response = JsonSerializer.Deserialize<UsuarioResponse>(json, opciones);
            if (response == null)
                return NotFound();

            Usuario = new UsuarioRequest
            {
                Cedula = response.Cedula,
                Nombre = response.Nombre,
                Email = response.Email,
                PasswordHash = response.PasswordHash,
                RolId = response.RolId,
                DireccionId = response.DireccionId,
                Telefono = response.Telefono,
                Activo = response.Activo
            };

            ViewData["UsuarioId"] = response.Id;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            if (!ModelState.IsValid)
                return Page();

            var endpoint = _config.ObtenerMetodo("ApiEndPointsUsuario", "Editar");
            Usuario.PasswordHash = HashearPassword(Usuario.PasswordHash);
            using var client = new HttpClient();
            var respuesta = await client.PutAsJsonAsync(string.Format(endpoint, id), Usuario);

            if (!respuesta.IsSuccessStatusCode)
            {
                ModelState.AddModelError(string.Empty, "Error al actualizar el usuario.");
                return Page();
            }

            TempData["MensajeExito"] = "Usuario actualizado correctamente.";
            return RedirectToPage("Listado");
        }
        private static string HashearPassword(string password)
        {
            using var sha = SHA256.Create();
            var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToHexString(bytes).ToLower();
        }

    }
}