using System.ComponentModel.DataAnnotations;

namespace Abstracciones.Modelos
{
    public class BitacoraBase
    {
        public int? UsuarioId { get; set; }

        [Required(ErrorMessage = "La acción es requerida")]
        [StringLength(100, ErrorMessage = "La acción debe ser menor a 100 caracteres")]
        public string Accion { get; set; }

        [StringLength(100, ErrorMessage = "La tabla afectada debe ser menor a 100 caracteres")]
        public string? TablaAfectada { get; set; }

        public int? RegistroId { get; set; }
    }

    public class BitacoraRequest : BitacoraBase { }

    public class BitacoraResponse : BitacoraBase
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public UsuarioResponse? Usuario { get; set; }
    }
}