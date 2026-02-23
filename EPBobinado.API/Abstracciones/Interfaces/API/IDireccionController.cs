using Abstracciones.Modelos;
using Microsoft.AspNetCore.Mvc;

namespace Abstracciones.Interfaces.API
{
    public interface IDireccionController
    {
        Task<IActionResult> Obtener();
        Task<IActionResult> Obtener(int Id);
        Task<IActionResult> Agregar(DireccionRequest direccion);
        Task<IActionResult> Editar(int Id, DireccionRequest direccion);
        Task<IActionResult> Eliminar(int Id);
    }
}
