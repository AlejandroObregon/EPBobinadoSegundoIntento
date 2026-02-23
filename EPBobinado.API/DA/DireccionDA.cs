using Abstracciones.Interfaces.DA;
using Abstracciones.Modelos;
using Dapper;
using Microsoft.Data.SqlClient;

namespace DA
{
    public class DireccionDA : IDireccionDA
    {
        private IRepositorioDapper _repositorioDapper;
        private SqlConnection _sqlConnection;

        public DireccionDA(IRepositorioDapper repositorioDapper)
        {
            _repositorioDapper = repositorioDapper;
            _sqlConnection = _repositorioDapper.ObtenerRepositorio();
        }

        public async Task<int> Agregar(DireccionRequest request)
        {
            string query = @"AgregarDireccion";
            var resultadoConsulta = await _sqlConnection.ExecuteScalarAsync<int>(query, new
            {
                Provincia = request.Provincia,
                Canton = request.Canton,
                Distrito = request.Distrito,
                CodigoPostal = request.CodigoPostal,
                Descripcion = request.Descripcion
            });

            return resultadoConsulta;
        }

        public async Task<int> Editar(int Id, DireccionRequest request)
        {
            await verificarDireccionExiste(Id);
            string query = @"EditarDireccion";
            var resultadoConsulta = await _sqlConnection.ExecuteScalarAsync<int>(query, new
            {
                Id = Id,
                Provincia = request.Provincia,
                Canton = request.Canton,
                Distrito = request.Distrito,
                CodigoPostal = request.CodigoPostal,
                Descripcion = request.Descripcion
            });
            return resultadoConsulta;
        }

        public async Task<int> Eliminar(int Id)
        {
            await verificarDireccionExiste(Id);
            string query = @"EliminarDireccion";
            var resultadoConsulta = await _sqlConnection.ExecuteScalarAsync<int>(query, new
            {
                Id = Id
            });
            return resultadoConsulta;
        }

        public async Task<IEnumerable<DireccionResponse>> Obtener()
        {
            string query = @"ObtenerDirecciones";
            var resultadoConsulta = await _sqlConnection.QueryAsync<DireccionResponse>(query);
            return resultadoConsulta;
        }

        public async Task<DireccionResponse> Obtener(int Id)
        {
            string query = @"ObtenerDireccion";
            var resultadoConsulta = await _sqlConnection.QueryAsync<DireccionResponse>(query,
                new { Id = Id });
            return resultadoConsulta.FirstOrDefault();
        }

        private async Task verificarDireccionExiste(int Id)
        {
            DireccionResponse? resultadoConsulta = await Obtener(Id);
            if (resultadoConsulta == null)
                throw new Exception("No se encontró la dirección");
        }
    }
}