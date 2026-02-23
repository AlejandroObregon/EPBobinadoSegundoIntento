using Abstracciones.Interfaces.DA;
using Abstracciones.Modelos;
using Dapper;
using Microsoft.Data.SqlClient;

namespace DA
{
    public class ClienteDA : IClienteDA
    {
        private IRepositorioDapper _repositorioDapper;
        private SqlConnection _sqlConnection;

        public ClienteDA(IRepositorioDapper repositorioDapper)
        {
            _repositorioDapper = repositorioDapper;
            _sqlConnection = _repositorioDapper.ObtenerRepositorio();
        }

        public async Task<int> Agregar(ClienteRequest request)
        {
            string query = @"AgregarCliente";
            var resultadoConsulta = await _sqlConnection.ExecuteScalarAsync<int>(query, new
            {
                Nombre = request.Nombre,
                Telefono = request.Telefono,
                Email = request.Email,
                Direccion = request.Direccion,
                Activo = request.Activo
            });

            return resultadoConsulta;
        }

        public async Task<int> Editar(int Id, ClienteRequest request)
        {
            await verificarClienteExiste(Id);
            string query = @"EditarCliente";
            var resultadoConsulta = await _sqlConnection.ExecuteScalarAsync<int>(query, new
            {
                Id = Id,
                Nombre = request.Nombre,
                Telefono = request.Telefono,
                Email = request.Email,
                Direccion = request.Direccion,
                Activo = request.Activo
            });
            return resultadoConsulta;
        }

        public async Task<int> Eliminar(int Id)
        {
            await verificarClienteExiste(Id);
            string query = @"EliminarCliente";
            var resultadoConsulta = await _sqlConnection.ExecuteScalarAsync<int>(query, new
            {
                Id = Id
            });
            return resultadoConsulta;
        }

        public async Task<IEnumerable<ClienteResponse>> Obtener()
        {
            string query = @"ObtenerClientes";
            var resultadoConsulta = await _sqlConnection.QueryAsync<ClienteResponse>(query);
            return resultadoConsulta;
        }

        public async Task<ClienteResponse> Obtener(int Id)
        {
            string query = @"ObtenerCliente";
            var resultadoConsulta = await _sqlConnection.QueryAsync<ClienteResponse>(query,
                new { Id = Id });
            return resultadoConsulta.FirstOrDefault();
        }

        private async Task verificarClienteExiste(int Id)
        {
            ClienteResponse? resultadoConsulta = await Obtener(Id);
            if (resultadoConsulta == null)
                throw new Exception("No se encontró el cliente");
        }
    }
}