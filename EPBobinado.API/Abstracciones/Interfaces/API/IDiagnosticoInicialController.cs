using Abstracciones.Modelos;
using Microsoft.AspNetCore.Mvc;

namespace Abstracciones.Interfaces.API
{
    public interface IDiagnosticoInicialController
    {
        Task<IActionResult> Obtener();
        Task<IActionResult> Obtener(int Id);
        Task<IActionResult> Agregar(DiagnosticoInicialRequest diagnosticoInicial);
        Task<IActionResult> Editar(int Id, DiagnosticoInicialRequest diagnosticoInicial);
        Task<IActionResult> Eliminar(int Id);
    }
}
