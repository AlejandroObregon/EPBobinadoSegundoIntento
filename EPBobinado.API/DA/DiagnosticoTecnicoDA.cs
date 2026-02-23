// DA/DiagnosticoTecnicoDA.cs
using Abstracciones.Interfaces.DA;
using Abstracciones.Modelos;
using Dapper;
using Microsoft.Data.SqlClient;

namespace DA
{
    public class DiagnosticoTecnicoDA : IDiagnosticoTecnicoDA
    {
        private IRepositorioDapper _repositorioDapper;
        private SqlConnection _sqlConnection;

        public DiagnosticoTecnicoDA(IRepositorioDapper repositorioDapper)
        {
            _repositorioDapper = repositorioDapper;
            _sqlConnection = _repositorioDapper.ObtenerRepositorio();
        }

        public async Task<int> Agregar(DiagnosticoTecnicoRequest request)
        {
            string query = @"AgregarDiagnosticoTecnico";
            var resultadoConsulta = await _sqlConnection.ExecuteScalarAsync<int>(query, new
            {
                OrdenId = request.OrdenId,
                Detalle = request.Detalle
            });

            return resultadoConsulta;
        }

        public async Task<int> Editar(int Id, DiagnosticoTecnicoRequest request)
        {
            await verificarDiagnosticoTecnicoExiste(Id);
            string query = @"EditarDiagnosticoTecnico";
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
            await verificarDiagnosticoTecnicoExiste(Id);
            string query = @"EliminarDiagnosticoTecnico";
            var resultadoConsulta = await _sqlConnection.ExecuteScalarAsync<int>(query, new
            {
                Id = Id
            });
            return resultadoConsulta;
        }

        public async Task<IEnumerable<DiagnosticoTecnicoResponse>> Obtener()
        {
            string query = @"ObtenerDiagnosticosTecnicos";
            var resultadoConsulta = await _sqlConnection.QueryAsync<DiagnosticoTecnicoResponse>(query);
            return resultadoConsulta;
        }

        public async Task<DiagnosticoTecnicoResponse> Obtener(int Id)
        {
            string query = @"ObtenerDiagnosticoTecnico";
            var resultadoConsulta = await _sqlConnection.QueryAsync<DiagnosticoTecnicoResponse>(query,
                new { Id = Id });
            return resultadoConsulta.FirstOrDefault();
        }

        private async Task verificarDiagnosticoTecnicoExiste(int Id)
        {
            DiagnosticoTecnicoResponse? resultadoConsulta = await Obtener(Id);
            if (resultadoConsulta == null)
                throw new Exception("No se encontró el diagnóstico técnico");
        }
    }
}