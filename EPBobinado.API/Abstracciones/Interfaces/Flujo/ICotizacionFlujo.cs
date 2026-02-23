using Abstracciones.Modelos;

namespace Abstracciones.Interfaces.Flujo
{
    public interface ICotizacionFlujo
    {
        Task<IEnumerable<CotizacionResponse>> Obtener();
        Task<CotizacionResponse> Obtener(int Id);
        Task<int> Agregar(CotizacionRequest cotizacion);
        Task<int> Editar(int Id, CotizacionRequest cotizacion);
        Task<int> Eliminar(int Id);
    }
}
