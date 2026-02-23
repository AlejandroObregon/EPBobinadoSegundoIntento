using Abstracciones.Interfaces.DA;
using Abstracciones.Interfaces.Flujo;
using Abstracciones.Modelos;

namespace Flujo
{
    public class MovimientoInventarioFlujo : IMovimientoInventarioFlujo
    {
        private IMovimientoInventarioDA _movimientoInventarioDA;

        public MovimientoInventarioFlujo(IMovimientoInventarioDA movimientoInventarioDA)
        {
            _movimientoInventarioDA = movimientoInventarioDA;
        }

        public Task<int> Agregar(MovimientoInventarioRequest movimientoInventario)
        {
            return _movimientoInventarioDA.Agregar(movimientoInventario);
        }

        public Task<int> Editar(int Id, MovimientoInventarioRequest movimientoInventario)
        {
            return _movimientoInventarioDA.Editar(Id, movimientoInventario);
        }

        public Task<int> Eliminar(int Id)
        {
            return _movimientoInventarioDA.Eliminar(Id);
        }

        public Task<IEnumerable<MovimientoInventarioResponse>> Obtener()
        {
            return _movimientoInventarioDA.Obtener();
        }

        public Task<MovimientoInventarioResponse> Obtener(int Id)
        {
            return _movimientoInventarioDA.Obtener(Id);
        }
    }
}
