using Abstracciones.Interfaces.DA;
using Abstracciones.Modelos;
using Dapper;
using Microsoft.Data.SqlClient;

namespace DA
{
    public class ProveedorDA : IProveedorDA
    {
        private IRepositorioDapper _repositorioDapper;
        private SqlConnection _sqlConnection;

        public ProveedorDA(IRepositorioDapper repositorioDapper)
        {
            _repositorioDapper = repositorioDapper;
            _sqlConnection = _repositorioDapper.ObtenerRepositorio();
        }

        public async Task<int> Agregar(ProveedorRequest request)
        {
            string query = @"AgregarProveedor";
            var resultadoConsulta = await _sqlConnection.ExecuteScalarAsync<int>(query, new
            {
                Nombre = request.Nombre,
                Contacto = request.Contacto,
                CreadoPor = request.CreadoPor
            });

            return resultadoConsulta;
        }

        public async Task<int> Editar(int Id, ProveedorRequest request)
        {
            await verificarProveedorExiste(Id);
            string query = @"EditarProveedor";
            var resultadoConsulta = await _sqlConnection.ExecuteScalarAsync<int>(query, new
            {
                Id = Id,
                Nombre = request.Nombre,
                Contacto = request.Contacto,
                CreadoPor = request.CreadoPor
            });
            return resultadoConsulta;
        }

        public async Task<int> Eliminar(int Id)
        {
            await verificarProveedorExiste(Id);
            string query = @"EliminarProveedor";
            var resultadoConsulta = await _sqlConnection.ExecuteScalarAsync<int>(query, new
            {
                Id = Id
            });
            return resultadoConsulta;
        }

        public async Task<IEnumerable<ProveedorResponse>> Obtener()
        {
            string query = @"ObtenerProveedores";
            var resultadoConsulta = await _sqlConnection.QueryAsync<ProveedorResponse>(query);
            return resultadoConsulta;
        }

        public async Task<ProveedorResponse> Obtener(int Id)
        {
            string query = @"ObtenerProveedor";
            var resultadoConsulta = await _sqlConnection.QueryAsync<ProveedorResponse>(query,
                new { Id = Id });
            return resultadoConsulta.FirstOrDefault();
        }

        private async Task verificarProveedorExiste(int Id)
        {
            ProveedorResponse? resultadoConsulta = await Obtener(Id);
            if (resultadoConsulta == null)
                throw new Exception("No se encontró el proveedor");
        }
    }
}