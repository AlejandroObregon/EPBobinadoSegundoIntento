using Abstracciones.Interfaces.DA;
using Abstracciones.Modelos;
using Dapper;
using Microsoft.Data.SqlClient;

namespace DA
{
    public class OrdenServicioDA : IOrdenServicioDA
    {
        private IRepositorioDapper _repositorioDapper;
        private SqlConnection _sqlConnection;

        public OrdenServicioDA(IRepositorioDapper repositorioDapper)
        {
            _repositorioDapper = repositorioDapper;
            _sqlConnection = _repositorioDapper.ObtenerRepositorio();
        }

        public async Task<int> Agregar(OrdenServicioRequest request)
        {
            string query = @"AgregarOrdenServicio";
            var resultadoConsulta = await _sqlConnection.ExecuteScalarAsync<int>(query, new
            {
                MotorId = request.MotorId,
                Estado = request.Estado,
                TecnicoId = request.TecnicoId
            });

            return resultadoConsulta;
        }

        public async Task<int> Editar(int Id, OrdenServicioRequest request)
        {
            await verificarOrdenServicioExiste(Id);
            string query = @"EditarOrdenServicio";
            var resultadoConsulta = await _sqlConnection.ExecuteScalarAsync<int>(query, new
            {
                Id = Id,
                MotorId = request.MotorId,
                Estado = request.Estado,
                TecnicoId = request.TecnicoId
            });
            return resultadoConsulta;
        }

        public async Task<int> Eliminar(int Id)
        {
            await verificarOrdenServicioExiste(Id);
            string query = @"EliminarOrdenServicio";
            var resultadoConsulta = await _sqlConnection.ExecuteScalarAsync<int>(query, new
            {
                Id = Id
            });
            return resultadoConsulta;
        }

        public async Task<IEnumerable<OrdenServicioResponse>> Obtener()
        {
            string query = @"ObtenerOrdenesServicio";
            var resultadoConsulta = await _sqlConnection.QueryAsync<OrdenServicioResponse>(query);
            return resultadoConsulta;
        }

        public async Task<OrdenServicioResponse> Obtener(int Id)
        {
            string query = @"ObtenerOrdenServicio";
            var resultadoConsulta = await _sqlConnection.QueryAsync<OrdenServicioResponse>(query,
                new { Id = Id });
            return resultadoConsulta.FirstOrDefault();
        }

        private async Task verificarOrdenServicioExiste(int Id)
        {
            OrdenServicioResponse? resultadoConsulta = await Obtener(Id);
            if (resultadoConsulta == null)
                throw new Exception("No se encontró la orden de servicio");
        }
    }
}