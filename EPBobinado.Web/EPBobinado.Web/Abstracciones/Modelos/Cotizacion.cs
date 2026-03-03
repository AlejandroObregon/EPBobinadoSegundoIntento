using System.ComponentModel.DataAnnotations;

namespace Abstracciones.Modelos
{
    public class CotizacionBase
    {
        [Required(ErrorMessage = "La orden es requerida")]
        [Range(1, int.MaxValue, ErrorMessage = "Orden inválida")]
        public int OrdenId { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "El total debe ser mayor o igual a 0")]
        public decimal? Total { get; set; }

        [Required(ErrorMessage = "El estado de aprobación es requerido")]
        public bool Aprobada { get; set; }
    }

    public class CotizacionRequest : CotizacionBase { }

    public class CotizacionResponse : CotizacionBase
    {
        public int Id { get; set; }
        public DateTime CreadoEn { get; set; }
        public OrdenServicioResponse? Orden { get; set; }
    }
}