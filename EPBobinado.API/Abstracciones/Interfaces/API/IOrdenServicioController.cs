using Abstracciones.Modelos;
using Microsoft.AspNetCore.Mvc;

namespace Abstracciones.Interfaces.API
{
    public interface IOrdenServicioController
    {
        Task<IActionResult> Obtener();
        Task<IActionResult> Obtener(int Id);
        Task<IActionResult> Agregar(OrdenServicioRequest ordenServicio);
        Task<IActionResult> Editar(int Id, OrdenServicioRequest ordenServicio);
        Task<IActionResult> Eliminar(int Id);
    }
}
