using Abstracciones.Interfaces.DA;
using Abstracciones.Modelos;
using Dapper;
using Microsoft.Data.SqlClient;

namespace DA
{
    public class MovimientoInventarioDA : IMovimientoInventarioDA
    {
        private IRepositorioDapper _repositorioDapper;
        private SqlConnection _sqlConnection;

        public MovimientoInventarioDA(IRepositorioDapper repositorioDapper)
        {
            _repositorioDapper = repositorioDapper;
            _sqlConnection = _repositorioDapper.ObtenerRepositorio();
        }

        public async Task<int> Agregar(MovimientoInventarioRequest request)
        {
            string query = @"AgregarMovimientoInventario";
            var resultadoConsulta = await _sqlConnection.ExecuteScalarAsync<int>(query, new
            {
                ProductoId = request.ProductoId,
                OrdenId = request.OrdenId,
                Tipo = request.Tipo,
                Cantidad = request.Cantidad
            });

            return resultadoConsulta;
        }

        public async Task<int> Editar(int Id, MovimientoInventarioRequest request)
        {
            await verificarMovimientoInventarioExiste(Id);
            string query = @"EditarMovimientoInventario";
            var resultadoConsulta = await _sqlConnection.ExecuteScalarAsync<int>(query, new
            {
                Id = Id,
                ProductoId = request.ProductoId,
                OrdenId = request.OrdenId,
                Tipo = request.Tipo,
                Cantidad = request.Cantidad
            });
            return resultadoConsulta;
        }

        public async Task<int> Eliminar(int Id)
        {
            await verificarMovimientoInventarioExiste(Id);
            string query = @"EliminarMovimientoInventario";
            var resultadoConsulta = await _sqlConnection.ExecuteScalarAsync<int>(query, new
            {
                Id = Id
            });
            return resultadoConsulta;
        }

        public async Task<IEnumerable<MovimientoInventarioResponse>> Obtener()
        {
            string query = @"ObtenerMovimientosInventario";
            var resultadoConsulta = await _sqlConnection.QueryAsync<MovimientoInventarioResponse>(query);
            return resultadoConsulta;
        }

        public async Task<MovimientoInventarioResponse> Obtener(int Id)
        {
            string query = @"ObtenerMovimientoInventario";
            var resultadoConsulta = await _sqlConnection.QueryAsync<MovimientoInventarioResponse>(query,
                new { Id = Id });
            return resultadoConsulta.FirstOrDefault();
        }

        private async Task verificarMovimientoInventarioExiste(int Id)
        {
            MovimientoInventarioResponse? resultadoConsulta = await Obtener(Id);
            if (resultadoConsulta == null)
                throw new Exception("No se encontró el movimiento de inventario");
        }
    }
}