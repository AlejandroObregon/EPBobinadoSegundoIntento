using Abstracciones.Modelos;
using Microsoft.AspNetCore.Mvc;

namespace Abstracciones.Interfaces.API
{
    public interface IDiagnosticoController
    {
        Task<IActionResult> Obtener();
        Task<IActionResult> Obtener(int Id);
        Task<IActionResult> Agregar(DiagnosticoRequest diagnostico);
        Task<IActionResult> Editar(int Id, DiagnosticoRequest diagnostico);
        Task<IActionResult> Eliminar(int Id);
    }
}
