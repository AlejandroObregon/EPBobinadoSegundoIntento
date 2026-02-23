using Abstracciones.Modelos;

namespace Abstracciones.Interfaces.DA
{
    public interface IRolDA
    {
        Task<IEnumerable<RolResponse>> Obtener();
        Task<RolResponse> Obtener(int Id);
        Task<int> Agregar(RolRequest rol);
        Task<int> Editar(int Id, RolRequest rol);
        Task<int> Eliminar(int Id);
    }
}
