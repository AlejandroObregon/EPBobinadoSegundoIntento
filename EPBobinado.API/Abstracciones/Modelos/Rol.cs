using System.ComponentModel.DataAnnotations;

namespace Abstracciones.Modelos
{
    public class RolBase
    {
        [Required(ErrorMessage = "El nombre del rol es requerido")]
        [StringLength(50, ErrorMessage = "El nombre debe ser menor a 50 caracteres")]
        public string Nombre { get; set; }

        [StringLength(255, ErrorMessage = "La descripción debe ser menor a 255 caracteres")]
        public string? Descripcion { get; set; }
    }

    public class RolRequest : RolBase { }

    public class RolResponse : RolBase
    {
        public int Id { get; set; }
    }
}