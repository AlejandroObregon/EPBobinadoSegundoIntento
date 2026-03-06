using Abstracciones.Interfaces.DA;
using Abstracciones.Modelos;
using Dapper;
using Microsoft.Data.SqlClient;

namespace DA
{
    // ── DTO plano — mapea todas las columnas del SP con aliases únicos ──
    internal class OrdenServicioFlat
    {
        // ── OrdenServicio ─────────────────────────────────────────────
        public int Id { get; set; }
        public int MotorId { get; set; }
        public string Estado { get; set; } = "";
        public int? TecnicoId { get; set; }
        public DateTime CreadoEn { get; set; }

        // ── Motor ─────────────────────────────────────────────────────
        public int? MotId { get; set; }
        public string? MotNumeroSerie { get; set; }
        public int? MotUsuarioId { get; set; }
        public int? MotModeloId { get; set; }
        public DateTime? MotCreadoEn { get; set; }

        // ── Usuario dueño del motor ───────────────────────────────────
        public int? DueId { get; set; }
        public string? DueNombre { get; set; }
        public string? DueEmail { get; set; }
        public string? DueCedula { get; set; }
        public string? DueTelefono { get; set; }
        public int? DueRolId { get; set; }
        public bool? DueActivo { get; set; }

        // ── Modelo del motor ──────────────────────────────────────────
        public int? ModId { get; set; }
        public string? ModNombre { get; set; }
        public string? ModEspecificaciones { get; set; }

        // ── Técnico asignado ──────────────────────────────────────────
        public int? TecId { get; set; }
        public string? TecNombre { get; set; }
        public string? TecEmail { get; set; }
        public string? TecCedula { get; set; }
        public string? TecTelefono { get; set; }
        public int? TecRolId { get; set; }
        public bool? TecActivo { get; set; }
    }

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
                request.TecnicoId
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
                request.TecnicoId
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
            var filas = await _sqlConnection.QueryAsync<OrdenServicioFlat>(
                "ObtenerOrdenesServicio",
                commandType: System.Data.CommandType.StoredProcedure);
            return filas.Select(Mapear);
        }

        public async Task<OrdenServicioResponse?> Obtener(int Id)
        {
            var filas = await _sqlConnection.QueryAsync<OrdenServicioFlat>(
                "ObtenerOrdenServicio",
                new { Id },
                commandType: System.Data.CommandType.StoredProcedure);
            return filas.Select(Mapear).FirstOrDefault();
        }

        // ── Mapeo plano → respuesta con objetos anidados ─────────────

        private static OrdenServicioResponse Mapear(OrdenServicioFlat f) => new()
        {
            Id = f.Id,
            MotorId = f.MotorId,
            Estado = f.Estado,
            TecnicoId = f.TecnicoId,
            CreadoEn = f.CreadoEn,

            Motor = f.MotId == null ? null : new MotorResponse
            {
                Id = f.MotId.Value,
                NumeroSerie = f.MotNumeroSerie,
                UsuarioId = f.MotUsuarioId ?? 0,
                ModeloId = f.MotModeloId ?? 0,
                CreadoEn = f.MotCreadoEn ?? default,

                Usuario = f.DueId == null ? null : new UsuarioResponse
                {
                    Id = f.DueId.Value,
                    Nombre = f.DueNombre ?? "",
                    Email = f.DueEmail ?? "",
                    Cedula = f.DueCedula ?? "",
                    Telefono = f.DueTelefono,
                    RolId = f.DueRolId ?? 0,
                    Activo = f.DueActivo ?? false
                },

                Modelo = f.ModId == null ? null : new ModeloMotorResponse
                {
                    Id = f.ModId.Value,
                    Nombre = f.ModNombre ?? "",
                    Especificaciones = f.ModEspecificaciones
                }
            },

            Tecnico = f.TecId == null ? null : new UsuarioResponse
            {
                Id = f.TecId.Value,
                Nombre = f.TecNombre ?? "",
                Email = f.TecEmail ?? "",
                Cedula = f.TecCedula ?? "",
                Telefono = f.TecTelefono,
                RolId = f.TecRolId ?? 0,
                Activo = f.TecActivo ?? false
            }
        };

        private async Task VerificarExiste(int Id)
        {
            var orden = await Obtener(Id);
            if (orden == null)
                throw new Exception($"No se encontró la orden de servicio con Id {Id}");
        }
    }
}