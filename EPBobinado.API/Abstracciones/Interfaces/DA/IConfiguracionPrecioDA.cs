using Abstracciones.Modelos;

namespace Abstracciones.Interfaces.DA
{
    public interface IConfiguracionPrecioDA
    {
        Task<IEnumerable<ConfiguracionPrecioResponse>> Obtener();
        Task<ConfiguracionPrecioResponse> Obtener(int Id);
        Task<int> Agregar(ConfiguracionPrecioRequest configuracionPrecio);
        Task<int> Editar(int Id, ConfiguracionPrecioRequest configuracionPrecio);
        Task<int> Eliminar(int Id);
    }
}
