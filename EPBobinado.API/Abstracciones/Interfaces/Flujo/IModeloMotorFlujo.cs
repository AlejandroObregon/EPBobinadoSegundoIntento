using Abstracciones.Modelos;

namespace Abstracciones.Interfaces.Flujo
{
    public interface IModeloMotorFlujo
    {
        Task<IEnumerable<ModeloMotorResponse>> Obtener();
        Task<ModeloMotorResponse> Obtener(int Id);
        Task<int> Agregar(ModeloMotorRequest modeloMotor);
        Task<int> Editar(int Id, ModeloMotorRequest modeloMotor);
        Task<int> Eliminar(int Id);
    }
}
