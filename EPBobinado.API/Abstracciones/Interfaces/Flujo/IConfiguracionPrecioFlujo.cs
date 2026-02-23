using Abstracciones.Modelos;

namespace Abstracciones.Interfaces.Flujo
{
    public interface IConfiguracionPrecioFlujo
    {
        Task<IEnumerable<ConfiguracionPrecioResponse>> Obtener();
        Task<ConfiguracionPrecioResponse> Obtener(int Id);
        Task<int> Agregar(ConfiguracionPrecioRequest configuracionPrecio);
        Task<int> Editar(int Id, ConfiguracionPrecioRequest configuracionPrecio);
        Task<int> Eliminar(int Id);
    }
}
