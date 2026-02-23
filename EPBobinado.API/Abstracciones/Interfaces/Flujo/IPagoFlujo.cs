using Abstracciones.Modelos;

namespace Abstracciones.Interfaces.Flujo
{
    public interface IPagoFlujo
    {
        Task<IEnumerable<PagoResponse>> Obtener();
        Task<PagoResponse> Obtener(int Id);
        Task<int> Agregar(PagoRequest pago);
        Task<int> Editar(int Id, PagoRequest pago);
        Task<int> Eliminar(int Id);
    }
}
