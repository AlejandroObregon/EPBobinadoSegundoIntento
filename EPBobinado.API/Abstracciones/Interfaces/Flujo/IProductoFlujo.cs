using Abstracciones.Modelos;

namespace Abstracciones.Interfaces.Flujo
{
    public interface IProductoFlujo
    {
        Task<IEnumerable<ProductoResponse>> Obtener();
        Task<ProductoResponse> Obtener(int Id);
        Task<int> Agregar(ProductoRequest producto);
        Task<int> Editar(int Id, ProductoRequest producto);
        Task<int> Eliminar(int Id);
    }
}
