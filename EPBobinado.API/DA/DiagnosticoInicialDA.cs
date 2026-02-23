// DA/DiagnosticoInicialDA.cs
using Abstracciones.Interfaces.DA;
using Abstracciones.Modelos;
using Dapper;
using Microsoft.Data.SqlClient;

namespace DA
{
    public class DiagnosticoInicialDA : IDiagnosticoInicialDA
    {
        private IRepositorioDapper _repositorioDapper;
        private SqlConnection _sqlConnection;

        public DiagnosticoInicialDA(IRepositorioDapper repositorioDapper)
        {
            _repositorioDapper = repositorioDapper;
            _sqlConnection = _repositorioDapper.ObtenerRepositorio();
        }

        public async Task<int> Agregar(DiagnosticoInicialRequest request)
        {
            string query = @"AgregarDiagnosticoInicial";
            var resultadoConsulta = await _sqlConnection.ExecuteScalarAsync<int>(query, new
            {
                OrdenId = request.OrdenId,
                Descripcion = request.Descripcion
            });

            return resultadoConsulta;
        }

        public async Task<int> Editar(int Id, DiagnosticoInicialRequest request)
        {
            await verificarDiagnosticoInicialExiste(Id);
            string query = @"EditarDiagnosticoInicial";
            var resultadoConsulta = await _sqlConnection.ExecuteScalarAsync<int>(query, new
            {
                Id = Id,
                OrdenId = request.OrdenId,
                Descripcion = request.Descripcion
            });
            return resultadoConsulta;
        }

        public async Task<int> Eliminar(int Id)
        {
            await verificarDiagnosticoInicialExiste(Id);
            string query = @"EliminarDiagnosticoInicial";
            var resultadoConsulta = await _sqlConnection.ExecuteScalarAsync<int>(query, new
            {
                Id = Id
            });
            return resultadoConsulta;
        }

        public async Task<IEnumerable<DiagnosticoInicialResponse>> Obtener()
        {
            string query = @"ObtenerDiagnosticosIniciales";
            var resultadoConsulta = await _sqlConnection.QueryAsync<DiagnosticoInicialResponse>(query);
            return resultadoConsulta;
        }

        public async Task<DiagnosticoInicialResponse> Obtener(int Id)
        {
            string query = @"ObtenerDiagnosticoInicial";
            var resultadoConsulta = await _sqlConnection.QueryAsync<DiagnosticoInicialResponse>(query,
                new { Id = Id });
            return resultadoConsulta.FirstOrDefault();
        }

        private async Task verificarDiagnosticoInicialExiste(int Id)
        {
            DiagnosticoInicialResponse? resultadoConsulta = await Obtener(Id);
            if (resultadoConsulta == null)
                throw new Exception("No se encontró el diagnóstico inicial");
        }
    }
}