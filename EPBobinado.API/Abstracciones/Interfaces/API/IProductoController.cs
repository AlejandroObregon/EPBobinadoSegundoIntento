using Abstracciones.Modelos;
using Microsoft.AspNetCore.Mvc;

namespace Abstracciones.Interfaces.API
{
    public interface IProductoController
    {
        Task<IActionResult> Obtener();
        Task<IActionResult> Obtener(int Id);
        Task<IActionResult> Agregar(ProductoRequest producto);
        Task<IActionResult> Editar(int Id, ProductoRequest producto);
        Task<IActionResult> Eliminar(int Id);
    }
}
