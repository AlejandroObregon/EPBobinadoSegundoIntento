using Abstracciones.Interfaces.DA;
using Abstracciones.Interfaces.Flujo;
using Abstracciones.Modelos;

namespace Flujo
{
    public class ProductoFlujo : IProductoFlujo
    {
        private IProductoDA _productoDA;

        public ProductoFlujo(IProductoDA productoDA)
        {
            _productoDA = productoDA;
        }

        public Task<int> Agregar(ProductoRequest producto)
        {
            return _productoDA.Agregar(producto);
        }

        public Task<int> Editar(int Id, ProductoRequest producto)
        {
            return _productoDA.Editar(Id, producto);
        }

        public Task<int> Eliminar(int Id)
        {
            return _productoDA.Eliminar(Id);
        }

        public Task<IEnumerable<ProductoResponse>> Obtener()
        {
            return _productoDA.Obtener();
        }

        public Task<ProductoResponse> Obtener(int Id)
        {
            return _productoDA.Obtener(Id);
        }
    }
}
