using System.ComponentModel.DataAnnotations;

namespace Abstracciones.Modelos
{
    public class DireccionBase
    {
        [Required(ErrorMessage = "La provincia es requerida")]
        [StringLength(100, ErrorMessage = "La provincia debe ser menor a 100 caracteres")]
        public string Provincia { get; set; }

        [Required(ErrorMessage = "El cantón es requerido")]
        [StringLength(100, ErrorMessage = "El cantón debe ser menor a 100 caracteres")]
        public string Canton { get; set; }

        [Required(ErrorMessage = "El distrito es requerido")]
        [StringLength(100, ErrorMessage = "El distrito debe ser menor a 100 caracteres")]
        public string Distrito { get; set; }

        [StringLength(20, ErrorMessage = "El código postal debe ser menor a 20 caracteres")]
        public string? CodigoPostal { get; set; }

        [StringLength(255, ErrorMessage = "La descripción debe ser menor a 255 caracteres")]
        public string? Descripcion { get; set; }
    }

    public class DireccionRequest : DireccionBase { }

    public class DireccionResponse : DireccionBase
    {
        public int Id { get; set; }
        public DateTime CreadoEn { get; set; }
    }
}