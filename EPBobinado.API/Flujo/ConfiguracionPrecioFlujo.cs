using Abstracciones.Interfaces.DA;
using Abstracciones.Interfaces.Flujo;
using Abstracciones.Modelos;

namespace Flujo
{
    public class ConfiguracionPrecioFlujo : IConfiguracionPrecioFlujo
    {
        private IConfiguracionPrecioDA _configuracionPrecioDA;

        public ConfiguracionPrecioFlujo(IConfiguracionPrecioDA configuracionPrecioDA)
        {
            _configuracionPrecioDA = configuracionPrecioDA;
        }

        public Task<int> Agregar(ConfiguracionPrecioRequest configuracionPrecio)
        {
            return _configuracionPrecioDA.Agregar(configuracionPrecio);
        }

        public Task<int> Editar(int Id, ConfiguracionPrecioRequest configuracionPrecio)
        {
            return _configuracionPrecioDA.Editar(Id, configuracionPrecio);
        }

        public Task<int> Eliminar(int Id)
        {
            return _configuracionPrecioDA.Eliminar(Id);
        }

        public Task<IEnumerable<ConfiguracionPrecioResponse>> Obtener()
        {
            return _configuracionPrecioDA.Obtener();
        }

        public Task<ConfiguracionPrecioResponse> Obtener(int Id)
        {
            return _configuracionPrecioDA.Obtener(Id);
        }
    }
}
