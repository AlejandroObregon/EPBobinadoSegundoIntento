using Abstracciones.Modelos;
using Microsoft.AspNetCore.Mvc;

namespace Abstracciones.Interfaces.API
{
    public interface IProveedorController
    {
        Task<IActionResult> Obtener();
        Task<IActionResult> Obtener(int Id);
        Task<IActionResult> Agregar(ProveedorRequest proveedor);
        Task<IActionResult> Editar(int Id, ProveedorRequest proveedor);
        Task<IActionResult> Eliminar(int Id);
    }
}
