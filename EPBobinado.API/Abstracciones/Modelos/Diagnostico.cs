using System.ComponentModel.DataAnnotations;

namespace Abstracciones.Modelos
{
    public class DiagnosticoBase
    {
        [Required(ErrorMessage = "La orden es requerida")]
        [Range(1, int.MaxValue, ErrorMessage = "Orden inválida")]
        public int OrdenId { get; set; }

        [Required(ErrorMessage = "La descripción es requerida")]
        public string Detalle { get; set; }
    }

    public class DiagnosticoRequest : DiagnosticoBase { }

    public class DiagnosticoResponse : DiagnosticoBase
    {
        public int Id { get; set; }
        public DateTime CreadoEn { get; set; }
        public OrdenServicioResponse? Orden { get; set; }
    }
}