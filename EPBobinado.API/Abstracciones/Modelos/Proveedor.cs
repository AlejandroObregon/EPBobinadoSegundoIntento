using System.ComponentModel.DataAnnotations;

namespace Abstracciones.Modelos
{
    public class ProveedorBase
    {
        [Required(ErrorMessage = "El nombre del proveedor es requerido")]
        [StringLength(100, ErrorMessage = "El nombre debe ser menor a 100 caracteres")]
        public string Nombre { get; set; }

        [StringLength(100, ErrorMessage = "El contacto debe ser menor a 100 caracteres")]
        public string? Contacto { get; set; }

        public int? CreadoPor { get; set; }
    }

    public class ProveedorRequest : ProveedorBase { }

    public class ProveedorResponse : ProveedorBase
    {
        public int Id { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string? UsuarioCreador { get; set; }
    }
}