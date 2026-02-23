using Abstracciones.Modelos;
using Microsoft.AspNetCore.Mvc;

namespace Abstracciones.Interfaces.API
{
    public interface IClienteController
    {
        Task<IActionResult> Obtener();
        Task<IActionResult> Obtener(int Id);
        Task<IActionResult> Agregar(ClienteRequest cliente);
        Task<IActionResult> Editar(int Id, ClienteRequest cliente);
        Task<IActionResult> Eliminar(int Id);
    }
}
