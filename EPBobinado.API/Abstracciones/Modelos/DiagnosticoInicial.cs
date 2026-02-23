using System.ComponentModel.DataAnnotations;

namespace Abstracciones.Modelos
{
    public class DiagnosticoInicialBase
    {
        [Required(ErrorMessage = "La orden es requerida")]
        [Range(1, int.MaxValue, ErrorMessage = "Orden inválida")]
        public int OrdenId { get; set; }

        [Required(ErrorMessage = "La descripción es requerida")]
        public string Descripcion { get; set; }
    }

    public class DiagnosticoInicialRequest : DiagnosticoInicialBase { }

    public class DiagnosticoInicialResponse : DiagnosticoInicialBase
    {
        public int Id { get; set; }
        public DateTime CreadoEn { get; set; }
        public OrdenServicioResponse? Orden { get; set; }
    }
}