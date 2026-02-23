using Abstracciones.Modelos;
using Microsoft.AspNetCore.Mvc;

namespace Abstracciones.Interfaces.API
{
    public interface IDiagnosticoTecnicoController
    {
        Task<IActionResult> Obtener();
        Task<IActionResult> Obtener(int Id);
        Task<IActionResult> Agregar(DiagnosticoTecnicoRequest diagnosticoTecnico);
        Task<IActionResult> Editar(int Id, DiagnosticoTecnicoRequest diagnosticoTecnico);
        Task<IActionResult> Eliminar(int Id);
    }
}
