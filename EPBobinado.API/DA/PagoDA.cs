// DA/PagoDA.cs
using Abstracciones.Interfaces.DA;
using Abstracciones.Modelos;
using Dapper;
using Microsoft.Data.SqlClient;

namespace DA
{
    public class PagoDA : IPagoDA
    {
        private IRepositorioDapper _repositorioDapper;
        private SqlConnection _sqlConnection;

        public PagoDA(IRepositorioDapper repositorioDapper)
        {
            _repositorioDapper = repositorioDapper;
            _sqlConnection = _repositorioDapper.ObtenerRepositorio();
        }

        public async Task<int> Agregar(PagoRequest request)
        {
            string query = @"AgregarPago";
            var resultadoConsulta = await _sqlConnection.ExecuteScalarAsync<int>(query, new
            {
                FacturaId = request.FacturaId,
                Monto = request.Monto,
                MetodoPago = request.MetodoPago
            });

            return resultadoConsulta;
        }

        public async Task<int> Editar(int Id, PagoRequest request)
        {
            await verificarPagoExiste(Id);
            string query = @"EditarPago";
            var resultadoConsulta = await _sqlConnection.ExecuteScalarAsync<int>(query, new
            {
                Id = Id,
                FacturaId = request.FacturaId,
                Monto = request.Monto,
                MetodoPago = request.MetodoPago
            });
            return resultadoConsulta;
        }

        public async Task<int> Eliminar(int Id)
        {
            await verificarPagoExiste(Id);
            string query = @"EliminarPago";
            var resultadoConsulta = await _sqlConnection.ExecuteScalarAsync<int>(query, new
            {
                Id = Id
            });
            return resultadoConsulta;
        }

        public async Task<IEnumerable<PagoResponse>> Obtener()
        {
            string query = @"ObtenerPagos";
            var resultadoConsulta = await _sqlConnection.QueryAsync<PagoResponse>(query);
            return resultadoConsulta;
        }

        public async Task<PagoResponse> Obtener(int Id)
        {
            string query = @"ObtenerPago";
            var resultadoConsulta = await _sqlConnection.QueryAsync<PagoResponse>(query,
                new { Id = Id });
            return resultadoConsulta.FirstOrDefault();
        }

        private async Task verificarPagoExiste(int Id)
        {
            PagoResponse? resultadoConsulta = await Obtener(Id);
            if (resultadoConsulta == null)
                throw new Exception("No se encontró el pago");
        }
    }
}