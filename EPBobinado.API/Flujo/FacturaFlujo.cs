using Abstracciones.Interfaces.DA;
using Abstracciones.Interfaces.Flujo;
using Abstracciones.Modelos;

namespace Flujo
{
    public class FacturaFlujo : IFacturaFlujo
    {
        private IFacturaDA _facturaDA;

        public FacturaFlujo(IFacturaDA facturaDA)
        {
            _facturaDA = facturaDA;
        }

        public Task<int> Agregar(FacturaRequest factura)
        {
            return _facturaDA.Agregar(factura);
        }

        public Task<int> Editar(int Id, FacturaRequest factura)
        {
            return _facturaDA.Editar(Id, factura);
        }

        public Task<int> Eliminar(int Id)
        {
            return _facturaDA.Eliminar(Id);
        }

        public Task<IEnumerable<FacturaResponse>> Obtener()
        {
            return _facturaDA.Obtener();
        }

        public Task<FacturaResponse> Obtener(int Id)
        {
            return _facturaDA.Obtener(Id);
        }
    }
}
