using Abstracciones.Modelos;

namespace Abstracciones.Interfaces.DA
{
    public interface IMotorDA
    {
        Task<IEnumerable<MotorResponse>> Obtener();
        Task<MotorResponse> Obtener(int Id);
        Task<int> Agregar(MotorRequest motor);
        Task<int> Editar(int Id, MotorRequest motor);
        Task<int> Eliminar(int Id);
    }
}
