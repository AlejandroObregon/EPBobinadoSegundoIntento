using Abstracciones.Interfaces.DA;
using Abstracciones.Modelos;
using Dapper;
using Microsoft.Data.SqlClient;

namespace DA
{
    public class RolDA : IRolDA
    {
        private IRepositorioDapper _repositorioDapper;
        private SqlConnection _sqlConnection;

        public RolDA(IRepositorioDapper repositorioDapper)
        {
            _repositorioDapper = repositorioDapper;
            _sqlConnection = _repositorioDapper.ObtenerRepositorio();
        }

        public async Task<int> Agregar(RolRequest request)
        {
            string query = @"AgregarRol";
            var resultadoConsulta = await _sqlConnection.ExecuteScalarAsync<int>(query, new
            {
                Nombre = request.Nombre,
                Descripcion = request.Descripcion
            });

            return resultadoConsulta;
        }

        public async Task<int> Editar(int Id, RolRequest request)
        {
            await verificarRolExiste(Id);
            string query = @"EditarRol";
            var resultadoConsulta = await _sqlConnection.ExecuteScalarAsync<int>(query, new
            {
                Id = Id,
                Nombre = request.Nombre,
                Descripcion = request.Descripcion
            });
            return resultadoConsulta;
        }

        public async Task<int> Eliminar(int Id)
        {
            await verificarRolExiste(Id);
            string query = @"EliminarRol";
            var resultadoConsulta = await _sqlConnection.ExecuteScalarAsync<int>(query, new
            {
                Id = Id
            });
            return resultadoConsulta;
        }

        public async Task<IEnumerable<RolResponse>> Obtener()
        {
            string query = @"ObtenerRoles";
            var resultadoConsulta = await _sqlConnection.QueryAsync<RolResponse>(query);
            return resultadoConsulta;
        }

        public async Task<RolResponse> Obtener(int Id)
        {
            string query = @"ObtenerRol";
            var resultadoConsulta = await _sqlConnection.QueryAsync<RolResponse>(query,
                new { Id = Id });
            return resultadoConsulta.FirstOrDefault();
        }

        private async Task verificarRolExiste(int Id)
        {
            RolResponse? resultadoConsulta = await Obtener(Id);
            if (resultadoConsulta == null)
                throw new Exception("No se encontró el rol");
        }
    }
}