using Abstracciones.Modelos;

namespace Abstracciones.Interfaces.Flujo
{
    public interface IClienteFlujo
    {
        Task<IEnumerable<ClienteResponse>> Obtener();
        Task<ClienteResponse> Obtener(int Id);
        Task<int> Agregar(ClienteRequest cliente);
        Task<int> Editar(int Id, ClienteRequest cliente);
        Task<int> Eliminar(int Id);
    }
}
