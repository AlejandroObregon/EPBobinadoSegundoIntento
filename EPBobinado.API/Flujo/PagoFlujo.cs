using Abstracciones.Interfaces.DA;
using Abstracciones.Interfaces.Flujo;
using Abstracciones.Modelos;

namespace Flujo
{
    public class PagoFlujo : IPagoFlujo
    {
        private IPagoDA _pagoDA;

        public PagoFlujo(IPagoDA pagoDA)
        {
            _pagoDA = pagoDA;
        }

        public Task<int> Agregar(PagoRequest pago)
        {
            return _pagoDA.Agregar(pago);
        }

        public Task<int> Editar(int Id, PagoRequest pago)
        {
            return _pagoDA.Editar(Id, pago);
        }

        public Task<int> Eliminar(int Id)
        {
            return _pagoDA.Eliminar(Id);
        }

        public Task<IEnumerable<PagoResponse>> Obtener()
        {
            return _pagoDA.Obtener();
        }

        public Task<PagoResponse> Obtener(int Id)
        {
            return _pagoDA.Obtener(Id);
        }
    }
}
