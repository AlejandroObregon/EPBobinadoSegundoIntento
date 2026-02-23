using Abstracciones.Interfaces.DA;
using Abstracciones.Interfaces.Flujo;
using Abstracciones.Modelos;

namespace Flujo
{
    public class RolFlujo : IRolFlujo
    {
        private IRolDA _rolDA;

        public RolFlujo(IRolDA rolDA)
        {
            _rolDA = rolDA;
        }

        public Task<int> Agregar(RolRequest rol)
        {
            return _rolDA.Agregar(rol);
        }

        public Task<int> Editar(int Id, RolRequest rol)
        {
            return _rolDA.Editar(Id, rol);
        }

        public Task<int> Eliminar(int Id)
        {
            return _rolDA.Eliminar(Id);
        }

        public Task<IEnumerable<RolResponse>> Obtener()
        {
            return _rolDA.Obtener();
        }

        public Task<RolResponse> Obtener(int Id)
        {
            return _rolDA.Obtener(Id);
        }
    }
}
