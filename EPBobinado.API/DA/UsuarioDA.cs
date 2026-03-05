using Abstracciones.Interfaces.DA;
using Abstracciones.Modelos;
using Dapper;
using Microsoft.Data.SqlClient;

namespace DA
{
    public class UsuarioDA : IUsuarioDA
    {
        private IRepositorioDapper _repositorioDapper;
        private SqlConnection _sqlConnection;

        public UsuarioDA(IRepositorioDapper repositorioDapper)
        {
            _repositorioDapper = repositorioDapper;
            _sqlConnection = _repositorioDapper.ObtenerRepositorio();
        }

        public async Task<int> Agregar(UsuarioRequest request)
        {
            string query = @"AgregarUsuario";
            var resultadoConsulta = await _sqlConnection.ExecuteScalarAsync<int>(query, new
            {
                Cedula = request.Cedula,
                Nombre = request.Nombre,
                Email = request.Email,
                PasswordHash = request.PasswordHash,
                RolId = request.RolId,
                DireccionId = request.DireccionId,
                Telefono = request.Telefono,
                Activo = request.Activo
            });
            return resultadoConsulta;
        }

        public async Task<int> Editar(int Id, UsuarioRequest request)
        {
            await verificarUsuarioExiste(Id);
            string query = @"EditarUsuario";
            var resultadoConsulta = await _sqlConnection.ExecuteScalarAsync<int>(query, new
            {
                Id = Id,
                Cedula = request.Cedula,
                Nombre = request.Nombre,
                Email = request.Email,
                PasswordHash = request.PasswordHash,
                RolId = request.RolId,
                DireccionId = request.DireccionId,
                Telefono = request.Telefono,
                Activo = request.Activo
            });
            return resultadoConsulta;
        }

        public async Task<int> Eliminar(int Id)
        {
            await verificarUsuarioExiste(Id);
            string query = @"EliminarUsuario";
            var resultadoConsulta = await _sqlConnection.ExecuteScalarAsync<int>(query, new
            {
                Id = Id
            });
            return resultadoConsulta;
        }

        public async Task<IEnumerable<UsuarioResponse>> Obtener()
        {
            string query = @"ObtenerUsuarios";
            var resultadoConsulta = await _sqlConnection.QueryAsync<UsuarioResponse>(query);
            return resultadoConsulta;
        }

        public async Task<UsuarioResponse> Obtener(int Id)
        {
            string query = @"ObtenerUsuario";
            var resultadoConsulta = await _sqlConnection.QueryAsync<UsuarioResponse>(query,
                new { Id = Id });
            return resultadoConsulta.FirstOrDefault();
        }

        public async Task<UsuarioResponse?> Login(string email, string passwordHash)
        {
            string query = @"LoginUsuario";
            var resultadoConsulta = await _sqlConnection.QueryAsync<UsuarioResponse>(query, new
            {
                Email = email,
                PasswordHash = passwordHash
            });
            return resultadoConsulta.FirstOrDefault();
        }

        private async Task verificarUsuarioExiste(int Id)
        {
            UsuarioResponse? resultadoConsulta = await Obtener(Id);
            if (resultadoConsulta == null)
                throw new Exception("No se encontró el usuario");
        }
    }
}