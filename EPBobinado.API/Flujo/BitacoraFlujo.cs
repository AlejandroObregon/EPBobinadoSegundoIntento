using Abstracciones.Interfaces.DA;
using Abstracciones.Interfaces.Flujo;
using Abstracciones.Modelos;

namespace Flujo
{
    public class BitacoraFlujo : IBitacoraFlujo
    {
        private IBitacoraDA _bitacoraDA;

        public BitacoraFlujo(IBitacoraDA bitacoraDA)
        {
            _bitacoraDA = bitacoraDA;
        }

        public Task<int> Agregar(BitacoraRequest bitacora)
        {
            return _bitacoraDA.Agregar(bitacora);
        }

        public Task<int> Editar(int Id, BitacoraRequest bitacora)
        {
            return _bitacoraDA.Editar(Id, bitacora);
        }

        public Task<int> Eliminar(int Id)
        {
            return _bitacoraDA.Eliminar(Id);
        }

        public Task<IEnumerable<BitacoraResponse>> Obtener()
        {
            return _bitacoraDA.Obtener();
        }

        public Task<BitacoraResponse> Obtener(int Id)
        {
            return _bitacoraDA.Obtener(Id);
        }
    }
}
