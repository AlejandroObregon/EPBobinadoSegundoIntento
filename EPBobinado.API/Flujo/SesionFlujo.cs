using Abstracciones.Interfaces.DA;
using Abstracciones.Interfaces.Flujo;
using Abstracciones.Modelos;

namespace Flujo
{
    public class SesionFlujo : ISesionFlujo
    {
        private ISesionDA _sesionDA;

        public SesionFlujo(ISesionDA sesionDA)
        {
            _sesionDA = sesionDA;
        }

        public Task<int> Agregar(SesionRequest sesion)
        {
            return _sesionDA.Agregar(sesion);
        }

        public Task<int> Editar(int Id, SesionRequest sesion)
        {
            return _sesionDA.Editar(Id, sesion);
        }

        public Task<int> Eliminar(int Id)
        {
            return _sesionDA.Eliminar(Id);
        }

        public Task<IEnumerable<SesionResponse>> Obtener()
        {
            return _sesionDA.Obtener();
        }

        public Task<SesionResponse> Obtener(int Id)
        {
            return _sesionDA.Obtener(Id);
        }
    }
}
