using Abstracciones.Interfaces.DA;
using Abstracciones.Modelos;
using Dapper;
using Microsoft.Data.SqlClient;

namespace DA
{

    public class OrdenServicioDA : IOrdenServicioDA
    {
        private readonly IRepositorioDapper _repositorioDapper;
        private readonly SqlConnection _sqlConnection;

        public OrdenServicioDA(IRepositorioDapper repositorioDapper)
        {
            _repositorioDapper = repositorioDapper;
            _sqlConnection = _repositorioDapper.ObtenerRepositorio();
        }

        // ── CUD ──────────────────────────────────────────────────────

        public async Task<int> Agregar(OrdenServicioRequest request)
        {
            return await _sqlConnection.ExecuteScalarAsync<int>("AgregarOrdenServicio", new
            {
                request.MotorId,
                request.Estado,
                request.TecnicoId,
                request.UsuarioId,
                request.Descripcion,
                request.Costo,
                request.FechaCita
            }, commandType: System.Data.CommandType.StoredProcedure);
        }


        public async Task<int> Editar(int Id, OrdenServicioRequest request)
        {
            await VerificarExiste(Id);
            return await _sqlConnection.ExecuteScalarAsync<int>("EditarOrdenServicio", new
            {
                Id,
                request.MotorId,
                request.Estado,
                request.TecnicoId,
                request.UsuarioId,
                request.Descripcion,
                request.Costo,
                request.FechaCita
            }, commandType: System.Data.CommandType.StoredProcedure);
        }

        public async Task<int> Eliminar(int Id)
        {
            await VerificarExiste(Id);
            return await _sqlConnection.ExecuteScalarAsync<int>("EliminarOrdenServicio",
                new { Id },
                commandType: System.Data.CommandType.StoredProcedure);
        }

        // ── Queries ───────────────────────────────────────────────────

        public async Task<IEnumerable<OrdenServicioResponse>> Obtener()
        {
            string query = @"ObtenerOrdenesServicio";
            var resultadoConsulta = await _sqlConnection.QueryAsync<OrdenServicioResponse>(query);
            return resultadoConsulta;
        }

        public async Task<OrdenServicioResponse?> Obtener(int Id)
        {
            string query = @"ObtenerOrdenServicio";
            var resultadoConsulta = await _sqlConnection.QueryAsync<OrdenServicioResponse>(query,
                new { Id = Id });
            return resultadoConsulta.FirstOrDefault(); ;
        }
       


        private async Task VerificarExiste(int Id)
        {
            var orden = await Obtener(Id);
            if (orden == null)
                throw new Exception($"No se encontró la orden de servicio con Id {Id}");
        }
    }
}