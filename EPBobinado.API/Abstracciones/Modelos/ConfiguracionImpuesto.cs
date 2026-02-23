using System.ComponentModel.DataAnnotations;

namespace Abstracciones.Modelos
{
    public class ConfiguracionImpuestoBase
    {
        [Required(ErrorMessage = "El porcentaje de impuesto es requerido")]
        [Range(0, 100, ErrorMessage = "El porcentaje debe estar entre 0 y 100")]
        public decimal Porcentaje { get; set; }

        [Required(ErrorMessage = "El usuario configurador es requerido")]
        [Range(1, int.MaxValue, ErrorMessage = "Usuario inválido")]
        public int ConfiguradoPor { get; set; }
    }

    public class ConfiguracionImpuestoRequest : ConfiguracionImpuestoBase { }

    public class ConfiguracionImpuestoResponse : ConfiguracionImpuestoBase
    {
        public int Id { get; set; }
        public DateTime FechaConfiguracion { get; set; }
        public UsuarioResponse? UsuarioConfigurador { get; set; }
    }
}