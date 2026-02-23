using Abstracciones.Modelos;
using Microsoft.AspNetCore.Mvc;

namespace Abstracciones.Interfaces.API
{
    public interface IConfiguracionPrecioController
    {
        Task<IActionResult> Obtener();
        Task<IActionResult> Obtener(int Id);
        Task<IActionResult> Agregar(ConfiguracionPrecioRequest configuracionPrecio);
        Task<IActionResult> Editar(int Id, ConfiguracionPrecioRequest configuracionPrecio);
        Task<IActionResult> Eliminar(int Id);
    }
}
