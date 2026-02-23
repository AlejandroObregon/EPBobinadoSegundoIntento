using Abstracciones.Interfaces.DA;
using Abstracciones.Interfaces.Flujo;
using Abstracciones.Modelos;

namespace Flujo
{
    public class UsuarioFlujo : IUsuarioFlujo
    {
        private IUsuarioDA _usuarioDA;

        public UsuarioFlujo(IUsuarioDA usuarioDA)
        {
            _usuarioDA = usuarioDA;
        }

        public Task<int> Agregar(UsuarioRequest usuario)
        {
            return _usuarioDA.Agregar(usuario);
        }

        public Task<int> Editar(int Id, UsuarioRequest usuario)
        {
            return _usuarioDA.Editar(Id, usuario);
        }

        public Task<int> Eliminar(int Id)
        {
            return _usuarioDA.Eliminar(Id);
        }

        public Task<IEnumerable<UsuarioResponse>> Obtener()
        {
            return _usuarioDA.Obtener();
        }

        public Task<UsuarioResponse> Obtener(int Id)
        {
            return _usuarioDA.Obtener(Id);
        }
    }
}
