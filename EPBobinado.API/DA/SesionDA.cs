using Abstracciones.Interfaces.DA;
using Abstracciones.Modelos;
using Dapper;
using Microsoft.Data.SqlClient;

namespace DA
{
    public class SesionDA : ISesionDA
    {
        private IRepositorioDapper _repositorioDapper;
        private SqlConnection _sqlConnection;

        public SesionDA(IRepositorioDapper repositorioDapper)
        {
            _repositorioDapper = repositorioDapper;
            _sqlConnection = _repositorioDapper.ObtenerRepositorio();
        }

        public async Task<int> Agregar(SesionRequest request)
        {
            string query = @"AgregarSesion";
            var resultadoConsulta = await _sqlConnection.ExecuteScalarAsync<int>(query, new
            {
                UsuarioId = request.UsuarioId,
                Token = request.Token,
                UltimaActividad = request.UltimaActividad,
                Activa = request.Activa
            });

            return resultadoConsulta;
        }

        public async Task<int> Editar(int Id, SesionRequest request)
        {
            await verificarSesionExiste(Id);
            string query = @"EditarSesion";
            var resultadoConsulta = await _sqlConnection.ExecuteScalarAsync<int>(query, new
            {
                Id = Id,
                UsuarioId = request.UsuarioId,
                Token = request.Token,
                UltimaActividad = request.UltimaActividad,
                Activa = request.Activa
            });
            return resultadoConsulta;
        }

        public async Task<int> Eliminar(int Id)
        {
            await verificarSesionExiste(Id);
            string query = @"EliminarSesion";
            var resultadoConsulta = await _sqlConnection.ExecuteScalarAsync<int>(query, new
            {
                Id = Id
            });
            return resultadoConsulta;
        }

        public async Task<IEnumerable<SesionResponse>> Obtener()
        {
            string query = @"ObtenerSesiones";
            var resultadoConsulta = await _sqlConnection.QueryAsync<SesionResponse>(query);
            return resultadoConsulta;
        }

        public async Task<SesionResponse> Obtener(int Id)
        {
            string query = @"ObtenerSesion";
            var resultadoConsulta = await _sqlConnection.QueryAsync<SesionResponse>(query,
                new { Id = Id });
            return resultadoConsulta.FirstOrDefault();
        }

        private async Task verificarSesionExiste(int Id)
        {
            SesionResponse? resultadoConsulta = await Obtener(Id);
            if (resultadoConsulta == null)
                throw new Exception("No se encontró la sesión");
        }
    }
}