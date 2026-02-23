using Abstracciones.Interfaces.DA;
using Abstracciones.Interfaces.Flujo;
using Abstracciones.Modelos;

namespace Flujo
{
    public class MotorFlujo : IMotorFlujo
    {
        private IMotorDA _motorDA;

        public MotorFlujo(IMotorDA motorDA)
        {
            _motorDA = motorDA;
        }

        public Task<int> Agregar(MotorRequest motor)
        {
            return _motorDA.Agregar(motor);
        }

        public Task<int> Editar(int Id, MotorRequest motor)
        {
            return _motorDA.Editar(Id, motor);
        }

        public Task<int> Eliminar(int Id)
        {
            return _motorDA.Eliminar(Id);
        }

        public Task<IEnumerable<MotorResponse>> Obtener()
        {
            return _motorDA.Obtener();
        }

        public Task<MotorResponse> Obtener(int Id)
        {
            return _motorDA.Obtener(Id);
        }
    }
}
