using Abstracciones.Modelos;

namespace Abstracciones.Interfaces.DA
{
    public interface IFacturaDA
    {
        Task<IEnumerable<FacturaResponse>> Obtener();
        Task<FacturaResponse> Obtener(int Id);
        Task<int> Agregar(FacturaRequest factura);
        Task<int> Editar(int Id, FacturaRequest factura);
        Task<int> Eliminar(int Id);
    }
}
