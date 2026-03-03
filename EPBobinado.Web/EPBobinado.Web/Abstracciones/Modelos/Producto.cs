using System.ComponentModel.DataAnnotations;

namespace Abstracciones.Modelos
{
    public class ProductoBase
    {
        [Required(ErrorMessage = "La propiedad nombre es requerida")]
        public string Nombre { get; set; }

        [StringLength(50, ErrorMessage = "La propiedad categoria debe ser menor a 50 caracteres")]
        public string Categoria { get; set; }

        [Required(ErrorMessage = "La propiedad precio es requerida")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor a 0")]
        public decimal Precio { get; set; }

        [Required(ErrorMessage = "La propiedad stock es requerida")]
        [Range(0, int.MaxValue, ErrorMessage = "El stock no puede ser negativo")]
        public int Stock { get; set; }

        [Required(ErrorMessage = "La propiedad stock minimo es requerida")]
        [Range(0, int.MaxValue, ErrorMessage = "El stock mínimo no puede ser negativo")]
        public int StockMinimo { get; set; }

        [Required(ErrorMessage = "La propiedad activo es requerida")]
        public bool Activo { get; set; }
    }

    public class ProductoRequest : ProductoBase
    {
        //Integrar IdCategoria en caso de que se cree tabla de categorias
    }

    public class ProductoResponse : ProductoBase
    {
        public int Id { get; set; }
    }
}
