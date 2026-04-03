using System.ComponentModel.DataAnnotations;

namespace Abstracciones.Modelos
{
    public class MovimientoInventarioBase
    {
        [Required(ErrorMessage = "El producto es requerido")]
        [Range(1, int.MaxValue, ErrorMessage = "Producto inválido")]
        public int ProductoId { get; set; }

        public int? OrdenId { get; set; }

        [Required(ErrorMessage = "El tipo de movimiento es requerido")]
        [RegularExpression("^(ENTRADA|SALIDA)$", ErrorMessage = "El tipo debe ser ENTRADA o SALIDA")]
        [StringLength(10)]
        public string Tipo { get; set; }

        [Required(ErrorMessage = "La cantidad es requerida")]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor a 0")]
        public int Cantidad { get; set; }
    }

    public class MovimientoInventarioRequest : MovimientoInventarioBase {
        [Required(ErrorMessage = "El producto es requerido")]
        public int ProductoId { get; set; }
        public int? OrdenId { get; set; }
    }

    public class MovimientoInventarioResponse : MovimientoInventarioBase
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public ProductoResponse? Producto { get; set; }
        public OrdenServicioResponse? Orden { get; set; }
    }
}