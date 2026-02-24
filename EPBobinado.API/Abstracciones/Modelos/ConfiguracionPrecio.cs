using System.ComponentModel.DataAnnotations;

namespace Abstracciones.Modelos
{
    public class ConfiguracionPrecioBase
    {
        [Required(ErrorMessage = "El precio por hora es requerido")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio por hora debe ser mayor a 0")]
        public decimal PrecioHora { get; set; }

        [Required(ErrorMessage = "El margen es requerido")]
        [Range(0, 100, ErrorMessage = "El margen debe estar entre 0 y 100")]
        public decimal Margen { get; set; }

        [Required(ErrorMessage = "El usuario configurador es requerido")]
        [Range(1, int.MaxValue, ErrorMessage = "Usuario inválido")]
        public int ConfiguradoPor { get; set; }
    }

    public class ConfiguracionPrecioRequest : ConfiguracionPrecioBase { }

    public class ConfiguracionPrecioResponse : ConfiguracionPrecioBase
    {
        public int Id { get; set; }
        public DateTime FechaConfiguracion { get; set; }
        public string? UsuarioConfigurador { get; set; }
    }
}