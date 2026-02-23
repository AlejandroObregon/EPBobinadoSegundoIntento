using Abstracciones.Interfaces.DA;
using Abstracciones.Interfaces.Flujo;
using Abstracciones.Modelos;

namespace Flujo
{
    public class ProveedorFlujo : IProveedorFlujo
    {
        private IProveedorDA _proveedorDA;

        public ProveedorFlujo(IProveedorDA proveedorDA)
        {
            _proveedorDA = proveedorDA;
        }

        public Task<int> Agregar(ProveedorRequest proveedor)
        {
            return _proveedorDA.Agregar(proveedor);
        }

        public Task<int> Editar(int Id, ProveedorRequest proveedor)
        {
            return _proveedorDA.Editar(Id, proveedor);
        }

        public Task<int> Eliminar(int Id)
        {
            return _proveedorDA.Eliminar(Id);
        }

        public Task<IEnumerable<ProveedorResponse>> Obtener()
        {
            return _proveedorDA.Obtener();
        }

        public Task<ProveedorResponse> Obtener(int Id)
        {
            return _proveedorDA.Obtener(Id);
        }
    }
}
