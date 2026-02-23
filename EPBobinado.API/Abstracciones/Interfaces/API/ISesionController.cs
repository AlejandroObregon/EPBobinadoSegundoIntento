using Abstracciones.Modelos;
using Microsoft.AspNetCore.Mvc;

namespace Abstracciones.Interfaces.API
{
    public interface ISesionController
    {
        Task<IActionResult> Obtener();
        Task<IActionResult> Obtener(int Id);
        Task<IActionResult> Agregar(SesionRequest sesion);
        Task<IActionResult> Editar(int Id, SesionRequest sesion);
        Task<IActionResult> Eliminar(int Id);
    }
}
