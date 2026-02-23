using Abstracciones.Modelos;
using Microsoft.AspNetCore.Mvc;

namespace Abstracciones.Interfaces.API
{
    public interface IBitacoraController
    {
        Task<IActionResult> Obtener();
        Task<IActionResult> Obtener(int Id);
        Task<IActionResult> Agregar(BitacoraRequest bitacora);
        Task<IActionResult> Editar(int Id, BitacoraRequest bitacora);
        Task<IActionResult> Eliminar(int Id);
    }
}
