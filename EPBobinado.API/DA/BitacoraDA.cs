using Abstracciones.Interfaces.DA;
using Abstracciones.Modelos;
using Dapper;
using Microsoft.Data.SqlClient;

namespace DA
{
    public class BitacoraDA : IBitacoraDA
    {
        private IRepositorioDapper _repositorioDapper;
        private SqlConnection _sqlConnection;

        public BitacoraDA(IRepositorioDapper repositorioDapper)
        {
            _repositorioDapper = repositorioDapper;
            _sqlConnection = _repositorioDapper.ObtenerRepositorio();
        }

        public async Task<int> Agregar(BitacoraRequest request)
        {
            string query = @"AgregarBitacora";
            var resultadoConsulta = await _sqlConnection.ExecuteScalarAsync<int>(query, new
            {
                UsuarioId = request.UsuarioId,
                Accion = request.Accion,
                TablaAfectada = request.TablaAfectada,
                RegistroId = request.RegistroId
            });

            return resultadoConsulta;
        }

        public async Task<int> Editar(int Id, BitacoraRequest request)
        {
            await verificarBitacoraExiste(Id);
            string query = @"EditarBitacora";
            var resultadoConsulta = await _sqlConnection.ExecuteScalarAsync<int>(query, new
            {
                Id = Id,
                UsuarioId = request.UsuarioId,
                Accion = request.Accion,
                TablaAfectada = request.TablaAfectada,
                RegistroId = request.RegistroId
            });
            return resultadoConsulta;
        }

        public async Task<int> Eliminar(int Id)
        {
            await verificarBitacoraExiste(Id);
            string query = @"EliminarBitacora";
            var resultadoConsulta = await _sqlConnection.ExecuteScalarAsync<int>(query, new
            {
                Id = Id
            });
            return resultadoConsulta;
        }

        public async Task<IEnumerable<BitacoraResponse>> Obtener()
        {
            string query = @"ObtenerBitacora";
            var resultadoConsulta = await _sqlConnection.QueryAsync<BitacoraResponse>(query);
            return resultadoConsulta;
        }

        public async Task<BitacoraResponse> Obtener(int Id)
        {
            string query = @"ObtenerRegistroBitacora";
            var resultadoConsulta = await _sqlConnection.QueryAsync<BitacoraResponse>(query,
                new { Id = Id });
            return resultadoConsulta.FirstOrDefault();
        }

        private async Task verificarBitacoraExiste(int Id)
        {
            BitacoraResponse? resultadoConsulta = await Obtener(Id);
            if (resultadoConsulta == null)
                throw new Exception("No se encontró el registro en bitácora");
        }
    }
}