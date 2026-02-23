using System.ComponentModel.DataAnnotations;

namespace Abstracciones.Modelos
{
    public class FacturaBase
    {
        [Required(ErrorMessage = "La orden es requerida")]
        [Range(1, int.MaxValue, ErrorMessage = "Orden inválida")]
        public int OrdenId { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "El total debe ser mayor o igual a 0")]
        public decimal? Total { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "El impuesto debe ser mayor o igual a 0")]
        public decimal? Impuesto { get; set; }
    }

    public class FacturaRequest : FacturaBase { }

    public class FacturaResponse : FacturaBase
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public OrdenServicioResponse? Orden { get; set; }
    }
}