using Abstracciones.Interfaces.DA;
using Abstracciones.Modelos;
using Dapper;
using Microsoft.Data.SqlClient;

namespace DA
{
    public class FacturaDA : IFacturaDA
    {
        private IRepositorioDapper _repositorioDapper;
        private SqlConnection _sqlConnection;

        public FacturaDA(IRepositorioDapper repositorioDapper)
        {
            _repositorioDapper = repositorioDapper;
            _sqlConnection = _repositorioDapper.ObtenerRepositorio();
        }

        public async Task<int> Agregar(FacturaRequest request)
        {
            string query = @"AgregarFactura";
            var resultadoConsulta = await _sqlConnection.ExecuteScalarAsync<int>(query, new
            {
                OrdenId = request.OrdenId,
                Total = request.Total,
                Impuesto = request.Impuesto
            });

            return resultadoConsulta;
        }

        public async Task<int> Editar(int Id, FacturaRequest request)
        {
            await verificarFacturaExiste(Id);
            string query = @"EditarFactura";
            var resultadoConsulta = await _sqlConnection.ExecuteScalarAsync<int>(query, new
            {
                Id = Id,
                OrdenId = request.OrdenId,
                Total = request.Total,
                Impuesto = request.Impuesto
            });
            return resultadoConsulta;
        }

        public async Task<int> Eliminar(int Id)
        {
            await verificarFacturaExiste(Id);
            string query = @"EliminarFactura";
            var resultadoConsulta = await _sqlConnection.ExecuteScalarAsync<int>(query, new
            {
                Id = Id
            });
            return resultadoConsulta;
        }

        public async Task<IEnumerable<FacturaResponse>> Obtener()
        {
            string query = @"ObtenerFacturas";
            var resultadoConsulta = await _sqlConnection.QueryAsync<FacturaResponse>(query);
            return resultadoConsulta;
        }

        public async Task<FacturaResponse> Obtener(int Id)
        {
            string query = @"ObtenerFactura";
            var resultadoConsulta = await _sqlConnection.QueryAsync<FacturaResponse>(query,
                new { Id = Id });
            return resultadoConsulta.FirstOrDefault();
        }

        private async Task verificarFacturaExiste(int Id)
        {
            FacturaResponse? resultadoConsulta = await Obtener(Id);
            if (resultadoConsulta == null)
                throw new Exception("No se encontró la factura");
        }
    }
}