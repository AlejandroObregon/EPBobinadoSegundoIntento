// DA/DiagnosticoDA.cs
using Abstracciones.Interfaces.DA;
using Abstracciones.Modelos;
using Dapper;
using Microsoft.Data.SqlClient;

namespace DA
{
    public class DiagnosticoDA : IDiagnosticoDA
    {
        private IRepositorioDapper _repositorioDapper;
        private SqlConnection _sqlConnection;

        public DiagnosticoDA(IRepositorioDapper repositorioDapper)
        {
            _repositorioDapper = repositorioDapper;
            _sqlConnection = _repositorioDapper.ObtenerRepositorio();
        }

        public async Task<int> Agregar(DiagnosticoRequest request)
        {
            string query = @"AgregarDiagnostico";
            var resultadoConsulta = await _sqlConnection.ExecuteScalarAsync<int>(query, new
            {
                OrdenId = request.OrdenId,
                Detalle = request.Detalle
            });

            return resultadoConsulta;
        }

        public async Task<int> Editar(int Id, DiagnosticoRequest request)
        {
            await verificarDiagnosticoExiste(Id);
            string query = @"EditarDiagnostico";
            var resultadoConsulta = await _sqlConnection.ExecuteScalarAsync<int>(query, new
            {
                Id = Id,
                OrdenId = request.OrdenId,
                Detalle = request.Detalle
            });
            return resultadoConsulta;
        }

        public async Task<int> Eliminar(int Id)
        {
            await verificarDiagnosticoExiste(Id);
            string query = @"EliminarDiagnostico";
            var resultadoConsulta = await _sqlConnection.ExecuteScalarAsync<int>(query, new
            {
                Id = Id
            });
            return resultadoConsulta;
        }

        public async Task<IEnumerable<DiagnosticoResponse>> Obtener()
        {
            string query = @"ObtenerDiagnosticos";
            var resultadoConsulta = await _sqlConnection.QueryAsync<DiagnosticoResponse>(query);
            return resultadoConsulta;
        }

        public async Task<DiagnosticoResponse> Obtener(int Id)
        {
            string query = @"ObtenerDiagnostico";
            var resultadoConsulta = await _sqlConnection.QueryAsync<DiagnosticoResponse>(query,
                new { Id = Id });
            return resultadoConsulta.FirstOrDefault();
        }

        private async Task verificarDiagnosticoExiste(int Id)
        {
            DiagnosticoResponse? resultadoConsulta = await Obtener(Id);
            if (resultadoConsulta == null)
                throw new Exception("No se encontró el diagnóstico técnico");
        }
    }
}