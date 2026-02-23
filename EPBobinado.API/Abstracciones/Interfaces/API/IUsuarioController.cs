using Abstracciones.Modelos;
using Microsoft.AspNetCore.Mvc;

namespace Abstracciones.Interfaces.API
{
    public interface IUsuarioController
    {
        Task<IActionResult> Obtener();
        Task<IActionResult> Obtener(int Id);
        Task<IActionResult> Agregar(UsuarioRequest usuario);
        Task<IActionResult> Editar(int Id, UsuarioRequest usuario);
        Task<IActionResult> Eliminar(int Id);
    }
}
