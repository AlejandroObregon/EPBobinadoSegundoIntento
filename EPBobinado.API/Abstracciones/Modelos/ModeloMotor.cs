using System.ComponentModel.DataAnnotations;

namespace Abstracciones.Modelos
{
    public class ModeloMotorBase
    {
        [Required(ErrorMessage = "El nombre del modelo es requerido")]
        [StringLength(100, ErrorMessage = "El nombre debe ser menor a 100 caracteres")]
        public string Nombre { get; set; }

        public string? Especificaciones { get; set; }
    }

    public class ModeloMotorRequest : ModeloMotorBase { }

    public class ModeloMotorResponse : ModeloMotorBase
    {
        public int Id { get; set; }
    }
}