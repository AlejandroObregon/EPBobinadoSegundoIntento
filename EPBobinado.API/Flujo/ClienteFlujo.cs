using Abstracciones.Interfaces.DA;
using Abstracciones.Interfaces.Flujo;
using Abstracciones.Modelos;

namespace Flujo
{
    public class ClienteFlujo : IClienteFlujo
    {
        private IClienteDA _clienteDA;

        public ClienteFlujo(IClienteDA clienteDA)
        {
            _clienteDA = clienteDA;
        }

        public Task<int> Agregar(ClienteRequest cliente)
        {
            return _clienteDA.Agregar(cliente);
        }

        public Task<int> Editar(int Id, ClienteRequest cliente)
        {
            return _clienteDA.Editar(Id, cliente);
        }

        public Task<int> Eliminar(int Id)
        {
            return _clienteDA.Eliminar(Id);
        }

        public Task<IEnumerable<ClienteResponse>> Obtener()
        {
            return _clienteDA.Obtener();
        }

        public Task<ClienteResponse> Obtener(int Id)
        {
            return _clienteDA.Obtener(Id);
        }
    }
}
