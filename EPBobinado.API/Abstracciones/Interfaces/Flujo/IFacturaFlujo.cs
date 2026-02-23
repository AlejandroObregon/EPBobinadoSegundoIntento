using Abstracciones.Modelos;

namespace Abstracciones.Interfaces.Flujo
{
    public interface IFacturaFlujo
    {
        Task<IEnumerable<FacturaResponse>> Obtener();
        Task<FacturaResponse> Obtener(int Id);
        Task<int> Agregar(FacturaRequest factura);
        Task<int> Editar(int Id, FacturaRequest factura);
        Task<int> Eliminar(int Id);
    }
}
