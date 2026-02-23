using Abstracciones.Modelos;

namespace Abstracciones.Interfaces.DA
{
    public interface IMovimientoInventarioDA
    {
        Task<IEnumerable<MovimientoInventarioResponse>> Obtener();
        Task<MovimientoInventarioResponse> Obtener(int Id);
        Task<int> Agregar(MovimientoInventarioRequest movimientoInventario);
        Task<int> Editar(int Id, MovimientoInventarioRequest movimientoInventario);
        Task<int> Eliminar(int Id);
    }
}
