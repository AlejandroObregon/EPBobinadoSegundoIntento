using Abstracciones.Interfaces.DA;
using Abstracciones.Interfaces.Flujo;
using Abstracciones.Modelos;

namespace Flujo
{
    public class ConfiguracionImpuestoFlujo : IConfiguracionImpuestoFlujo
    {
        private IConfiguracionImpuestoDA _configuracionImpuestoDA;

        public ConfiguracionImpuestoFlujo(IConfiguracionImpuestoDA configuracionImpuestoDA)
        {
            _configuracionImpuestoDA = configuracionImpuestoDA;
        }

        public Task<int> Agregar(ConfiguracionImpuestoRequest configuracionImpuesto)
        {
            return _configuracionImpuestoDA.Agregar(configuracionImpuesto);
        }

        public Task<int> Editar(int Id, ConfiguracionImpuestoRequest configuracionImpuesto)
        {
            return _configuracionImpuestoDA.Editar(Id, configuracionImpuesto);
        }

        public Task<int> Eliminar(int Id)
        {
            return _configuracionImpuestoDA.Eliminar(Id);
        }

        public Task<IEnumerable<ConfiguracionImpuestoResponse>> Obtener()
        {
            return _configuracionImpuestoDA.Obtener();
        }

        public Task<ConfiguracionImpuestoResponse> Obtener(int Id)
        {
            return _configuracionImpuestoDA.Obtener(Id);
        }
    }
}
