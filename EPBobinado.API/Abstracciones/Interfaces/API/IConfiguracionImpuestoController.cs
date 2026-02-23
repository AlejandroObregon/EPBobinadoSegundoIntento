using Abstracciones.Modelos;
using Microsoft.AspNetCore.Mvc;

namespace Abstracciones.Interfaces.API
{
    public interface IConfiguracionImpuestoController
    {
        Task<IActionResult> Obtener();
        Task<IActionResult> Obtener(int Id);
        Task<IActionResult> Agregar(ConfiguracionImpuestoRequest configuracionImpuesto);
        Task<IActionResult> Editar(int Id, ConfiguracionImpuestoRequest configuracionImpuesto);
        Task<IActionResult> Eliminar(int Id);
    }
}
