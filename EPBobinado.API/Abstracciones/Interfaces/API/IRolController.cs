using Abstracciones.Modelos;
using Microsoft.AspNetCore.Mvc;

namespace Abstracciones.Interfaces.API
{
    public interface IRolController
    {
        Task<IActionResult> Obtener();
        Task<IActionResult> Obtener(int Id);
        Task<IActionResult> Agregar(RolRequest rol);
        Task<IActionResult> Editar(int Id, RolRequest rol);
        Task<IActionResult> Eliminar(int Id);
    }
}
