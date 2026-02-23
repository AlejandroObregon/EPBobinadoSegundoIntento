using Abstracciones.Interfaces.DA;
using Abstracciones.Modelos;
using Dapper;
using Microsoft.Data.SqlClient;

namespace DA
{
    public class MotorDA : IMotorDA
    {
        private IRepositorioDapper _repositorioDapper;
        private SqlConnection _sqlConnection;

        public MotorDA(IRepositorioDapper repositorioDapper)
        {
            _repositorioDapper = repositorioDapper;
            _sqlConnection = _repositorioDapper.ObtenerRepositorio();
        }

        public async Task<int> Agregar(MotorRequest request)
        {
            string query = @"AgregarMotor";
            var resultadoConsulta = await _sqlConnection.ExecuteScalarAsync<int>(query, new
            {
                ClienteId = request.ClienteId,
                ModeloId = request.ModeloId,
                NumeroSerie = request.NumeroSerie
            });

            return resultadoConsulta;
        }

        public async Task<int> Editar(int Id, MotorRequest request)
        {
            await verificarMotorExiste(Id);
            string query = @"EditarMotor";
            var resultadoConsulta = await _sqlConnection.ExecuteScalarAsync<int>(query, new
            {
                Id = Id,
                ClienteId = request.ClienteId,
                ModeloId = request.ModeloId,
                NumeroSerie = request.NumeroSerie
            });
            return resultadoConsulta;
        }

        public async Task<int> Eliminar(int Id)
        {
            await verificarMotorExiste(Id);
            string query = @"EliminarMotor";
            var resultadoConsulta = await _sqlConnection.ExecuteScalarAsync<int>(query, new
            {
                Id = Id
            });
            return resultadoConsulta;
        }

        public async Task<IEnumerable<MotorResponse>> Obtener()
        {
            string query = @"ObtenerMotores";
            var resultadoConsulta = await _sqlConnection.QueryAsync<MotorResponse>(query);
            return resultadoConsulta;
        }

        public async Task<MotorResponse> Obtener(int Id)
        {
            string query = @"ObtenerMotor";
            var resultadoConsulta = await _sqlConnection.QueryAsync<MotorResponse>(query,
                new { Id = Id });
            return resultadoConsulta.FirstOrDefault();
        }

        private async Task verificarMotorExiste(int Id)
        {
            MotorResponse? resultadoConsulta = await Obtener(Id);
            if (resultadoConsulta == null)
                throw new Exception("No se encontró el motor");
        }
    }
}