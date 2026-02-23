using Abstracciones.Interfaces.DA;
using Abstracciones.Modelos;
using Dapper;
using Microsoft.Data.SqlClient;

namespace DA
{
    public class ConfiguracionImpuestoDA : IConfiguracionImpuestoDA
    {
        private IRepositorioDapper _repositorioDapper;
        private SqlConnection _sqlConnection;

        public ConfiguracionImpuestoDA(IRepositorioDapper repositorioDapper)
        {
            _repositorioDapper = repositorioDapper;
            _sqlConnection = _repositorioDapper.ObtenerRepositorio();
        }

        public async Task<int> Agregar(ConfiguracionImpuestoRequest request)
        {
            string query = @"AgregarConfiguracionImpuesto";
            var resultadoConsulta = await _sqlConnection.ExecuteScalarAsync<int>(query, new
            {
                Porcentaje = request.Porcentaje,
                ConfiguradoPor = request.ConfiguradoPor
            });

            return resultadoConsulta;
        }

        public async Task<int> Editar(int Id, ConfiguracionImpuestoRequest request)
        {
            await verificarConfiguracionImpuestoExiste(Id);
            string query = @"EditarConfiguracionImpuesto";
            var resultadoConsulta = await _sqlConnection.ExecuteScalarAsync<int>(query, new
            {
                Id = Id,
                Porcentaje = request.Porcentaje,
                ConfiguradoPor = request.ConfiguradoPor
            });
            return resultadoConsulta;
        }

        public async Task<int> Eliminar(int Id)
        {
            await verificarConfiguracionImpuestoExiste(Id);
            string query = @"EliminarConfiguracionImpuesto";
            var resultadoConsulta = await _sqlConnection.ExecuteScalarAsync<int>(query, new
            {
                Id = Id
            });
            return resultadoConsulta;
        }

        public async Task<IEnumerable<ConfiguracionImpuestoResponse>> Obtener()
        {
            string query = @"ObtenerConfiguracionesImpuesto";
            var resultadoConsulta = await _sqlConnection.QueryAsync<ConfiguracionImpuestoResponse>(query);
            return resultadoConsulta;
        }

        public async Task<ConfiguracionImpuestoResponse> Obtener(int Id)
        {
            string query = @"ObtenerConfiguracionImpuesto";
            var resultadoConsulta = await _sqlConnection.QueryAsync<ConfiguracionImpuestoResponse>(query,
                new { Id = Id });
            return resultadoConsulta.FirstOrDefault();
        }

        private async Task verificarConfiguracionImpuestoExiste(int Id)
        {
            ConfiguracionImpuestoResponse? resultadoConsulta = await Obtener(Id);
            if (resultadoConsulta == null)
                throw new Exception("No se encontró la configuración de impuesto");
        }
    }
}