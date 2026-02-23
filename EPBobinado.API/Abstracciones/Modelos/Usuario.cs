// Abstracciones/Modelos/Usuario.cs
using System.ComponentModel.DataAnnotations;

namespace Abstracciones.Modelos
{
    public class UsuarioBase
    {
        [Required(ErrorMessage = "La cédula es requerida")]
        [StringLength(20, ErrorMessage = "La cédula debe ser menor a 20 caracteres")]
        public string Cedula { get; set; }

        [Required(ErrorMessage = "El nombre es requerido")]
        [StringLength(100, ErrorMessage = "El nombre debe ser menor a 100 caracteres")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El email es requerido")]
        [EmailAddress(ErrorMessage = "Formato de email inválido")]
        [StringLength(100, ErrorMessage = "El email debe ser menor a 100 caracteres")]
        public string Email { get; set; }

        [Required(ErrorMessage = "La contraseña es requerida")]
        [StringLength(255, MinimumLength = 6, ErrorMessage = "La contraseña debe tener entre 6 y 255 caracteres")]
        public string PasswordHash { get; set; }

        [Required(ErrorMessage = "El rol es requerido")]
        [Range(1, int.MaxValue, ErrorMessage = "Rol inválido")]
        public int RolId { get; set; }

        public int? DireccionId { get; set; }

        [StringLength(20, ErrorMessage = "El teléfono debe ser menor a 20 caracteres")]
        [Phone(ErrorMessage = "Formato de teléfono inválido")]
        public string? Telefono { get; set; }

        [Required(ErrorMessage = "El estado activo es requerido")]
        public bool Activo { get; set; }
    }

    public class UsuarioRequest : UsuarioBase {
        public int RolId { get; set; }
        public int? DireccionId { get; set; }
    }

    public class UsuarioResponse : UsuarioBase
    {
        public int Id { get; set; }
        public DateTime CreadoEn { get; set; }
    }
}