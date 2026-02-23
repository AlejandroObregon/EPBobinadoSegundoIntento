using Abstracciones.Interfaces.DA;
using Abstracciones.Interfaces.Flujo;
using Abstracciones.Modelos;

namespace Flujo
{
    public class ModeloMotorFlujo : IModeloMotorFlujo
    {
        private IModeloMotorDA _modeloMotorDA;

        public ModeloMotorFlujo(IModeloMotorDA modeloMotorDA)
        {
            _modeloMotorDA = modeloMotorDA;
        }

        public Task<int> Agregar(ModeloMotorRequest modeloMotor)
        {
            return _modeloMotorDA.Agregar(modeloMotor);
        }

        public Task<int> Editar(int Id, ModeloMotorRequest modeloMotor)
        {
            return _modeloMotorDA.Editar(Id, modeloMotor);
        }

        public Task<int> Eliminar(int Id)
        {
            return _modeloMotorDA.Eliminar(Id);
        }

        public Task<IEnumerable<ModeloMotorResponse>> Obtener()
        {
            return _modeloMotorDA.Obtener();
        }

        public Task<ModeloMotorResponse> Obtener(int Id)
        {
            return _modeloMotorDA.Obtener(Id);
        }
    }
}
