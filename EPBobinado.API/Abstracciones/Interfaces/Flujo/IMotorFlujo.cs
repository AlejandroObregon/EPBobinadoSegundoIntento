using Abstracciones.Modelos;

namespace Abstracciones.Interfaces.Flujo
{
    public interface IMotorFlujo
    {
        Task<IEnumerable<MotorResponse>> Obtener();
        Task<MotorResponse> Obtener(int Id);
        Task<int> Agregar(MotorRequest motor);
        Task<int> Editar(int Id, MotorRequest motor);
        Task<int> Eliminar(int Id);
    }
}
