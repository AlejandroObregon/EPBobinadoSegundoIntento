using Abstracciones.Interfaces.DA;
using Abstracciones.Modelos;
using Dapper;
using Microsoft.Data.SqlClient;

namespace DA
{
    public class ModeloMotorDA : IModeloMotorDA
    {
        private IRepositorioDapper _repositorioDapper;
        private SqlConnection _sqlConnection;

        public ModeloMotorDA(IRepositorioDapper repositorioDapper)
        {
            _repositorioDapper = repositorioDapper;
            _sqlConnection = _repositorioDapper.ObtenerRepositorio();
        }

        public async Task<int> Agregar(ModeloMotorRequest request)
        {
            string query = @"AgregarModeloMotor";
            var resultadoConsulta = await _sqlConnection.ExecuteScalarAsync<int>(query, new
            {
                Nombre = request.Nombre,
                Especificaciones = request.Especificaciones
            });

            return resultadoConsulta;
        }

        public async Task<int> Editar(int Id, ModeloMotorRequest request)
        {
            await verificarModeloMotorExiste(Id);
            string query = @"EditarModeloMotor";
            var resultadoConsulta = await _sqlConnection.ExecuteScalarAsync<int>(query, new
            {
                Id = Id,
                Nombre = request.Nombre,
                Especificaciones = request.Especificaciones
            });
            return resultadoConsulta;
        }

        public async Task<int> Eliminar(int Id)
        {
            await verificarModeloMotorExiste(Id);
            string query = @"EliminarModeloMotor";
            var resultadoConsulta = await _sqlConnection.ExecuteScalarAsync<int>(query, new
            {
                Id = Id
            });
            return resultadoConsulta;
        }

        public async Task<IEnumerable<ModeloMotorResponse>> Obtener()
        {
            string query = @"ObtenerModelosMotor";
            var resultadoConsulta = await _sqlConnection.QueryAsync<ModeloMotorResponse>(query);
            return resultadoConsulta;
        }

        public async Task<ModeloMotorResponse> Obtener(int Id)
        {
            string query = @"ObtenerModeloMotor";
            var resultadoConsulta = await _sqlConnection.QueryAsync<ModeloMotorResponse>(query,
                new { Id = Id });
            return resultadoConsulta.FirstOrDefault();
        }

        private async Task verificarModeloMotorExiste(int Id)
        {
            ModeloMotorResponse? resultadoConsulta = await Obtener(Id);
            if (resultadoConsulta == null)
                throw new Exception("No se encontró el modelo de motor");
        }
    }
}