using Abstracciones.Modelos;
using Microsoft.AspNetCore.Mvc;

namespace Abstracciones.Interfaces.API
{
    public interface IPagoController
    {
        Task<IActionResult> Obtener();
        Task<IActionResult> Obtener(int Id);
        Task<IActionResult> Agregar(PagoRequest pago);
        Task<IActionResult> Editar(int Id, PagoRequest ago);
        Task<IActionResult> Eliminar(int Id);
    }
}
