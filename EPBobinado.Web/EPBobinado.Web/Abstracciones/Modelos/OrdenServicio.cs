using System.ComponentModel.DataAnnotations;

namespace Abstracciones.Modelos
{
    public class FutureDateAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value is DateTime fecha)
                return fecha > DateTime.Now;
            return false;
        }
    }
    public class OrdenServicioBase
    {

        [Required(ErrorMessage = "El estado es requerido")]
        [StringLength(50, ErrorMessage = "El estado debe ser menor a 50 caracteres")]
        public string Estado { get; set; }
        public string? Descripcion { get; set; }
        [Range(0, double.MaxValue, ErrorMessage = "El costo debe ser mayor o igual a 0")]
        public decimal? Costo { get; set; }
        [DataType(DataType.DateTime)]
        [FutureDate(ErrorMessage = "La fecha debe ser posterior al día actual.")]
        public DateTime? FechaCita { get; set; }
    }

    public class OrdenServicioRequest : OrdenServicioBase
    {
        public int? MotorId { get; set; }
        public int? TecnicoId { get; set; }
        public int? UsuarioId { get; set; }
    }

    public class OrdenServicioResponse : OrdenServicioBase
    {
        public int Id { get; set; }
        public DateTime CreadoEn { get; set; }
        public string? Modelo { get; set; }
        public string? NumeroSerie { get; set; }
        public string? Tecnico { get; set; }
        public string? Cliente { get; set; }
        public int? IdCliente { get; set; }
        public int? MotorId { get; set; }
        public int? IdTecnico { get; set; }
    }

    
}