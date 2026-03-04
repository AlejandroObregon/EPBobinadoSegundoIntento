using System.ComponentModel.DataAnnotations;

namespace Abstracciones.Modelos
{
    public class MotorBase
    {
        [Required(ErrorMessage = "El usuario es requerido")]
        [Range(1, int.MaxValue, ErrorMessage = "Usuario inválido")]
        public int UsuarioId { get; set; }

        [Required(ErrorMessage = "El modelo es requerido")]
        [Range(1, int.MaxValue, ErrorMessage = "Modelo inválido")]
        public int ModeloId { get; set; }

        [StringLength(100, ErrorMessage = "El número de serie debe ser menor a 100 caracteres")]
        public string? NumeroSerie { get; set; }
    }

    public class MotorRequest : MotorBase
    {
        [Required(ErrorMessage = "El usuario es requerido")]
        public int UsuarioId { get; set; }

        [Required(ErrorMessage = "El modelo es requerido")]
        public int ModeloId { get; set; }
    }

    public class MotorResponse : MotorBase
    {
        public int Id { get; set; }
        public DateTime CreadoEn { get; set; }
        public UsuarioResponse? Usuario { get; set; }
        public ModeloMotorResponse? Modelo { get; set; }
    }
}