using Abstracciones.Modelos;

namespace Abstracciones.Interfaces.DA
{
    public interface IProductoDA
    {
        Task<IEnumerable<ProductoResponse>> Obtener();
        Task<ProductoResponse> Obtener(int Id);
        Task<int> Agregar(ProductoRequest producto);
        Task<int> Editar(int Id, ProductoRequest producto);
        Task<int> Eliminar(int Id);
    }
}
