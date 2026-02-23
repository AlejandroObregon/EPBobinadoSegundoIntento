using Abstracciones.Modelos;

namespace Abstracciones.Interfaces.Flujo
{
    public interface ISesionFlujo
    {
        Task<IEnumerable<SesionResponse>> Obtener();
        Task<SesionResponse> Obtener(int Id);
        Task<int> Agregar(SesionRequest sesion);
        Task<int> Editar(int Id, SesionRequest sesion);
        Task<int> Eliminar(int Id);
    }
}
