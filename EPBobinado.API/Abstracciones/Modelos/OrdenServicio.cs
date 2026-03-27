using System.ComponentModel.DataAnnotations;

namespace Abstracciones.Modelos
{
    public class OrdenServicioBase
    {
        [Required(ErrorMessage = "El motor es requerido")]
        [Range(1, int.MaxValue, ErrorMessage = "Motor inválido")]
        public int MotorId { get; set; }

        [Required(ErrorMessage = "El estado es requerido")]
        [StringLength(50, ErrorMessage = "El estado debe ser menor a 50 caracteres")]
        public string Estado { get; set; }

    }

    public class OrdenServicioRequest : OrdenServicioBase {
        public int MotorId { get; set; }
        public int? TecnicoId { get; set; }
        public int? UsuarioId { get; set; }
    }

    public class OrdenServicioResponse : OrdenServicioBase
    {
        public int Id { get; set; }
        public DateTime CreadoEn { get; set; }
        public int MotorId { get; set; }
        public string? Tecnico { get; set; }
        public string? Cliente { get; set; }
    }
}