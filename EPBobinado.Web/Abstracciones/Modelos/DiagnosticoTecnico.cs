using System.ComponentModel.DataAnnotations;

namespace Abstracciones.Modelos
{
    public class DiagnosticoTecnicoBase
    {
        [Required(ErrorMessage = "La orden es requerida")]
        [Range(1, int.MaxValue, ErrorMessage = "Orden inválida")]
        public int OrdenId { get; set; }

        [Required(ErrorMessage = "El detalle es requerido")]
        public string Detalle { get; set; }
    }

    public class DiagnosticoTecnicoRequest : DiagnosticoTecnicoBase { }

    public class DiagnosticoTecnicoResponse : DiagnosticoTecnicoBase
    {
        public int Id { get; set; }
        public DateTime CreadoEn { get; set; }
        public OrdenServicioResponse? Orden { get; set; }
    }
}