using Abstracciones.Modelos;
using Microsoft.AspNetCore.Mvc;

namespace Abstracciones.Interfaces.API
{
    public interface IMotorController
    {
        Task<IActionResult> Obtener();
        Task<IActionResult> Obtener(int Id);
        Task<IActionResult> Agregar(MotorRequest motor);
        Task<IActionResult> Editar(int Id, MotorRequest motor);
        Task<IActionResult> Eliminar(int Id);
    }
}
