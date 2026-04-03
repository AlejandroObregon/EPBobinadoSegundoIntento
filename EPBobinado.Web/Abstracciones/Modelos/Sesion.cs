// Abstracciones/Modelos/Sesion.cs
using System.ComponentModel.DataAnnotations;

namespace Abstracciones.Modelos
{
    public class SesionBase
    {
        [Required(ErrorMessage = "El usuario es requerido")]
        [Range(1, int.MaxValue, ErrorMessage = "Usuario inválido")]
        public int UsuarioId { get; set; }

        [Required(ErrorMessage = "El token es requerido")]
        [StringLength(255, ErrorMessage = "El token debe ser menor a 255 caracteres")]
        public string Token { get; set; }

        public DateTime? UltimaActividad { get; set; }

        [Required(ErrorMessage = "El estado activa es requerido")]
        public bool Activa { get; set; }
    }

    public class SesionRequest : SesionBase { }

    public class SesionResponse : SesionBase
    {
        public int Id { get; set; }
        public DateTime Inicio { get; set; }
        public UsuarioResponse? Usuario { get; set; }
    }
}