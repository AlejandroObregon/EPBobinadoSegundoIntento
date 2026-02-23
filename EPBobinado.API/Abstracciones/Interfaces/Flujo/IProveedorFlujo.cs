using Abstracciones.Modelos;

namespace Abstracciones.Interfaces.Flujo
{
    public interface IProveedorFlujo
    {
        Task<IEnumerable<ProveedorResponse>> Obtener();
        Task<ProveedorResponse> Obtener(int Id);
        Task<int> Agregar(ProveedorRequest proveedor);
        Task<int> Editar(int Id, ProveedorRequest proveedor);
        Task<int> Eliminar(int Id);
    }
}
