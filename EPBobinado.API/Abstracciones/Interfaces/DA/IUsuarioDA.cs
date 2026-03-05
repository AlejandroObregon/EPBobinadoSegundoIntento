using Abstracciones.Modelos;

namespace Abstracciones.Interfaces.DA
{
    public interface IUsuarioDA
    {
        Task<int> Agregar(UsuarioRequest request);
        Task<int> Editar(int Id, UsuarioRequest request);
        Task<int> Eliminar(int Id);
        Task<IEnumerable<UsuarioResponse>> Obtener();
        Task<UsuarioResponse> Obtener(int Id);
        Task<UsuarioResponse?> Login(string email, string passwordHash);
    }
}