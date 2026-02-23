using Abstracciones.Modelos;

namespace Abstracciones.Interfaces.Flujo
{
    public interface IConfiguracionImpuestoFlujo
    {
        Task<IEnumerable<ConfiguracionImpuestoResponse>> Obtener();
        Task<ConfiguracionImpuestoResponse> Obtener(int Id);
        Task<int> Agregar(ConfiguracionImpuestoRequest configuracionImpuesto);
        Task<int> Editar(int Id, ConfiguracionImpuestoRequest configuracionImpuesto);
        Task<int> Eliminar(int Id);
    }
}
