using Abstracciones.Modelos;
using Microsoft.AspNetCore.Mvc;

namespace Abstracciones.Interfaces.API
{
    public interface IModeloMotorController
    {
        Task<IActionResult> Obtener();
        Task<IActionResult> Obtener(int Id);
        Task<IActionResult> Agregar(ModeloMotorRequest modeloMotor);
        Task<IActionResult> Editar(int Id, ModeloMotorRequest modeloMotor);
        Task<IActionResult> Eliminar(int Id);
    }
}
