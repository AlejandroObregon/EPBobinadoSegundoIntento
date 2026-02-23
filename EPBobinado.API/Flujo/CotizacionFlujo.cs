using Abstracciones.Interfaces.DA;
using Abstracciones.Interfaces.Flujo;
using Abstracciones.Modelos;

namespace Flujo
{
    public class CotizacionFlujo : ICotizacionFlujo
    {
        private ICotizacionDA _cotizacionDA;

        public CotizacionFlujo(ICotizacionDA cotizacionDA)
        {
            _cotizacionDA = cotizacionDA;
        }

        public Task<int> Agregar(CotizacionRequest cotizacion)
        {
            return _cotizacionDA.Agregar(cotizacion);
        }

        public Task<int> Editar(int Id, CotizacionRequest cotizacion)
        {
            return _cotizacionDA.Editar(Id, cotizacion);
        }

        public Task<int> Eliminar(int Id)
        {
            return _cotizacionDA.Eliminar(Id);
        }

        public Task<IEnumerable<CotizacionResponse>> Obtener()
        {
            return _cotizacionDA.Obtener();
        }

        public Task<CotizacionResponse> Obtener(int Id)
        {
            return _cotizacionDA.Obtener(Id);
        }
    }
}
