using Abstracciones.Modelos;
using Microsoft.AspNetCore.Mvc;

namespace Abstracciones.Interfaces.API
{
    public interface IMovimientoInventarioController
    {
        Task<IActionResult> Obtener();
        Task<IActionResult> Obtener(int Id);
        Task<IActionResult> Agregar(MovimientoInventarioRequest movimientoInventario);
        Task<IActionResult> Editar(int Id, MovimientoInventarioRequest movimientoInventario);
        Task<IActionResult> Eliminar(int Id);
    }
}
