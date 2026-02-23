using Abstracciones.Modelos;

namespace Abstracciones.Interfaces.DA
{
    public interface ISesionDA
    {
        Task<IEnumerable<SesionResponse>> Obtener();
        Task<SesionResponse> Obtener(int Id);
        Task<int> Agregar(SesionRequest sesion);
        Task<int> Editar(int Id, SesionRequest sesion);
        Task<int> Eliminar(int Id);
    }
}
