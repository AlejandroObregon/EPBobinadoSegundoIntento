using Abstracciones.Modelos;

namespace Abstracciones.Interfaces.Flujo
{
    public interface IUsuarioFlujo
    {
        Task<IEnumerable<UsuarioResponse>> Obtener();
        Task<UsuarioResponse> Obtener(int Id);
        Task<int> Agregar(UsuarioRequest usuario);
        Task<int> Editar(int Id, UsuarioRequest usuario);
        Task<int> Eliminar(int Id);
    }
}
