using Abstracciones.Modelos;
using Microsoft.AspNetCore.Mvc;

namespace Abstracciones.Interfaces.API
{
    public interface ICotizacionController
    {
        Task<IActionResult> Obtener();
        Task<IActionResult> Obtener(int Id);
        Task<IActionResult> Agregar(CotizacionRequest cotizacion);
        Task<IActionResult> Editar(int Id, CotizacionRequest cotizacion);
        Task<IActionResult> Eliminar(int Id);
    }
}
