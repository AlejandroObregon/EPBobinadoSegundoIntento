using Abstracciones.Interfaces.DA;
using Abstracciones.Interfaces.Flujo;
using Abstracciones.Modelos;

namespace Flujo
{
    public class OrdenServicioFlujo : IOrdenServicioFlujo
    {
        private IOrdenServicioDA _ordenServicioDA;

        public OrdenServicioFlujo(IOrdenServicioDA ordenServicioDA)
        {
            _ordenServicioDA = ordenServicioDA;
        }

        public Task<int> Agregar(OrdenServicioRequest ordenServicio)
        {
            return _ordenServicioDA.Agregar(ordenServicio);
        }

        public Task<int> Editar(int Id, OrdenServicioRequest ordenServicio)
        {
            return _ordenServicioDA.Editar(Id, ordenServicio);
        }

        public Task<int> Eliminar(int Id)
        {
            return _ordenServicioDA.Eliminar(Id);
        }

        public Task<IEnumerable<OrdenServicioResponse>> Obtener()
        {
            return _ordenServicioDA.Obtener();
        }

        public Task<OrdenServicioResponse> Obtener(int Id)
        {
            return _ordenServicioDA.Obtener(Id);
        }
    }
}
