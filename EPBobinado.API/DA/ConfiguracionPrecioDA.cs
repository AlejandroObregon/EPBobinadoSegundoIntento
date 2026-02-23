using Abstracciones.Interfaces.DA;
using Abstracciones.Modelos;
using Dapper;
using Microsoft.Data.SqlClient;

namespace DA
{
    public class ConfiguracionPrecioDA : IConfiguracionPrecioDA
    {
        private IRepositorioDapper _repositorioDapper;
        private SqlConnection _sqlConnection;

        public ConfiguracionPrecioDA(IRepositorioDapper repositorioDapper)
        {
            _repositorioDapper = repositorioDapper;
            _sqlConnection = _repositorioDapper.ObtenerRepositorio();
        }

        public async Task<int> Agregar(ConfiguracionPrecioRequest request)
        {
            string query = @"AgregarConfiguracionPrecio";
            var resultadoConsulta = await _sqlConnection.ExecuteScalarAsync<int>(query, new
            {
                PrecioHora = request.PrecioHora,
                Margen = request.Margen,
                ConfiguradoPor = request.ConfiguradoPor
            });

            return resultadoConsulta;
        }

        public async Task<int> Editar(int Id, ConfiguracionPrecioRequest request)
        {
            await verificarConfiguracionPrecioExiste(Id);
            string query = @"EditarConfiguracionPrecio";
            var resultadoConsulta = await _sqlConnection.ExecuteScalarAsync<int>(query, new
            {
                Id = Id,
                PrecioHora = request.PrecioHora,
                Margen = request.Margen,
                ConfiguradoPor = request.ConfiguradoPor
            });
            return resultadoConsulta;
        }

        public async Task<int> Eliminar(int Id)
        {
            await verificarConfiguracionPrecioExiste(Id);
            string query = @"EliminarConfiguracionPrecio";
            var resultadoConsulta = await _sqlConnection.ExecuteScalarAsync<int>(query, new
            {
                Id = Id
            });
            return resultadoConsulta;
        }

        public async Task<IEnumerable<ConfiguracionPrecioResponse>> Obtener()
        {
            string query = @"ObtenerConfiguracionesPrecio";
            var resultadoConsulta = await _sqlConnection.QueryAsync<ConfiguracionPrecioResponse>(query);
            return resultadoConsulta;
        }

        public async Task<ConfiguracionPrecioResponse> Obtener(int Id)
        {
            string query = @"ObtenerConfiguracionPrecio";
            var resultadoConsulta = await _sqlConnection.QueryAsync<ConfiguracionPrecioResponse>(query,
                new { Id = Id });
            return resultadoConsulta.FirstOrDefault();
        }

        private async Task verificarConfiguracionPrecioExiste(int Id)
        {
            ConfiguracionPrecioResponse? resultadoConsulta = await Obtener(Id);
            if (resultadoConsulta == null)
                throw new Exception("No se encontró la configuración de precio");
        }
    }
}