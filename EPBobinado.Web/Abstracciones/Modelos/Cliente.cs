using System.ComponentModel.DataAnnotations;

namespace Abstracciones.Modelos
{
    public class ClienteBase
    {
        [Required(ErrorMessage = "El nombre del cliente es requerido")]
        [StringLength(100, ErrorMessage = "El nombre debe ser menor a 100 caracteres")]
        public string Nombre { get; set; }

        [StringLength(20, ErrorMessage = "El teléfono debe ser menor a 20 caracteres")]
        [Phone(ErrorMessage = "Formato de teléfono inválido")]
        public string? Telefono { get; set; }

        [EmailAddress(ErrorMessage = "Formato de email inválido")]
        [StringLength(100, ErrorMessage = "El email debe ser menor a 100 caracteres")]
        public string? Email { get; set; }

        [StringLength(255, ErrorMessage = "La dirección debe ser menor a 255 caracteres")]
        public string? Direccion { get; set; }

        [Required(ErrorMessage = "El estado activo es requerido")]
        public bool Activo { get; set; }
    }

    public class ClienteRequest : ClienteBase { }

    public class ClienteResponse : ClienteBase
    {
        public int Id { get; set; }
        public DateTime CreadoEn { get; set; }
    }
}