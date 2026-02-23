using Abstracciones.Interfaces.DA;
using Abstracciones.Modelos;
using Dapper;
using Microsoft.Data.SqlClient;

namespace DA
{
    public class CotizacionDA : ICotizacionDA
    {
        private IRepositorioDapper _repositorioDapper;
        private SqlConnection _sqlConnection;

        public CotizacionDA(IRepositorioDapper repositorioDapper)
        {
            _repositorioDapper = repositorioDapper;
            _sqlConnection = _repositorioDapper.ObtenerRepositorio();
        }

        public async Task<int> Agregar(CotizacionRequest request)
        {
            string query = @"AgregarCotizacion";
            var resultadoConsulta = await _sqlConnection.ExecuteScalarAsync<int>(query, new
            {
                OrdenId = request.OrdenId,
                Total = request.Total,
                Aprobada = request.Aprobada
            });

            return resultadoConsulta;
        }

        public async Task<int> Editar(int Id, CotizacionRequest request)
        {
            await verificarCotizacionExiste(Id);
            string query = @"EditarCotizacion";
            var resultadoConsulta = await _sqlConnection.ExecuteScalarAsync<int>(query, new
            {
                Id = Id,
                OrdenId = request.OrdenId,
                Total = request.Total,
                Aprobada = request.Aprobada
            });
            return resultadoConsulta;
        }

        public async Task<int> Eliminar(int Id)
        {
            await verificarCotizacionExiste(Id);
            string query = @"EliminarCotizacion";
            var resultadoConsulta = await _sqlConnection.ExecuteScalarAsync<int>(query, new
            {
                Id = Id
            });
            return resultadoConsulta;
        }

        public async Task<IEnumerable<CotizacionResponse>> Obtener()
        {
            string query = @"ObtenerCotizaciones";
            var resultadoConsulta = await _sqlConnection.QueryAsync<CotizacionResponse>(query);
            return resultadoConsulta;
        }

        public async Task<CotizacionResponse> Obtener(int Id)
        {
            string query = @"ObtenerCotizacion";
            var resultadoConsulta = await _sqlConnection.QueryAsync<CotizacionResponse>(query,
                new { Id = Id });
            return resultadoConsulta.FirstOrDefault();
        }

        private async Task verificarCotizacionExiste(int Id)
        {
            CotizacionResponse? resultadoConsulta = await Obtener(Id);
            if (resultadoConsulta == null)
                throw new Exception("No se encontró la cotización");
        }
    }
}