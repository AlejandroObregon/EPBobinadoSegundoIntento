using Abstracciones.Modelos;

namespace Abstracciones.Interfaces.DA
{
    public interface IConfiguracionImpuestoDA
    {
        Task<IEnumerable<ConfiguracionImpuestoResponse>> Obtener();
        Task<ConfiguracionImpuestoResponse> Obtener(int Id);
        Task<int> Agregar(ConfiguracionImpuestoRequest configuracionImpuesto);
        Task<int> Editar(int Id, ConfiguracionImpuestoRequest configuracionImpuesto);
        Task<int> Eliminar(int Id);
    }
}
