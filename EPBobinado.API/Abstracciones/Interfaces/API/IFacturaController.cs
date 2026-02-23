using Abstracciones.Modelos;
using Microsoft.AspNetCore.Mvc;

namespace Abstracciones.Interfaces.API
{
    public interface IFacturaController
    {
        Task<IActionResult> Obtener();
        Task<IActionResult> Obtener(int Id);
        Task<IActionResult> Agregar(FacturaRequest factura);
        Task<IActionResult> Editar(int Id, FacturaRequest factura);
        Task<IActionResult> Eliminar(int Id);
    }
}
