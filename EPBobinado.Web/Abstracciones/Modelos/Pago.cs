using System.ComponentModel.DataAnnotations;

namespace Abstracciones.Modelos
{
    public class PagoBase
    {
        [Required(ErrorMessage = "La factura es requerida")]
        [Range(1, int.MaxValue, ErrorMessage = "Factura inválida")]
        public int FacturaId { get; set; }

        [Required(ErrorMessage = "El monto es requerido")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El monto debe ser mayor a 0")]
        public decimal Monto { get; set; }

        [StringLength(50, ErrorMessage = "El método de pago debe ser menor a 50 caracteres")]
        public string? MetodoPago { get; set; }
    }

    public class PagoRequest : PagoBase { }

    public class PagoResponse : PagoBase
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public FacturaResponse? Factura { get; set; }
    }
}