using Abstracciones.Interfaces.DA;
using Abstracciones.Modelos;
using Dapper;
using Microsoft.Data.SqlClient;

namespace DA
{
    // DTO plano — recibe todas las columnas del SP con aliases únicos
    internal class MotorFlat
    {
        // ── Motor ─────────────────────────────────
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public int ModeloId { get; set; }
        public string? NumeroSerie { get; set; }
        public DateTime CreadoEn { get; set; }
        // ── Usuario ───────────────────────────────
        public int? UsuId { get; set; }
        public string? UsuNombre { get; set; }
        public string? UsuEmail { get; set; }
        public string? UsuCedula { get; set; }
        public string? UsuTelefono { get; set; }
        public int? UsuRolId { get; set; }
        public bool? UsuActivo { get; set; }
        // ── ModeloMotor ───────────────────────────
        public int? ModId { get; set; }
        public string? ModNombre { get; set; }
        public string? ModEspecificaciones { get; set; }
    }

    public class MotorDA : IMotorDA
    {
        private readonly IRepositorioDapper _repositorioDapper;
        private readonly SqlConnection _sqlConnection;

        public MotorDA(IRepositorioDapper repositorioDapper)
        {
            _repositorioDapper = repositorioDapper;
            _sqlConnection = _repositorioDapper.ObtenerRepositorio();
        }

        public async Task<int> Agregar(MotorRequest request)
        {
            return await _sqlConnection.ExecuteScalarAsync<int>("AgregarMotor", new
            {
                request.UsuarioId,
                request.ModeloId,
                request.NumeroSerie
            }, commandType: System.Data.CommandType.StoredProcedure);
        }

        public async Task<int> Editar(int Id, MotorRequest request)
        {
            await verificarMotorExiste(Id);
            return await _sqlConnection.ExecuteScalarAsync<int>("EditarMotor", new
            {
                Id,
                request.UsuarioId,
                request.ModeloId,
                request.NumeroSerie
            }, commandType: System.Data.CommandType.StoredProcedure);
        }

        public async Task<int> Eliminar(int Id)
        {
            await verificarMotorExiste(Id);
            return await _sqlConnection.ExecuteScalarAsync<int>("EliminarMotor",
                new { Id },
                commandType: System.Data.CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<MotorResponse>> Obtener()
        {
            var filas = await _sqlConnection.QueryAsync<MotorFlat>(
                "ObtenerMotores",
                commandType: System.Data.CommandType.StoredProcedure);
            return filas.Select(MapearMotor);
        }

        public async Task<MotorResponse> Obtener(int Id)
        {
            var filas = await _sqlConnection.QueryAsync<MotorFlat>(
                "ObtenerMotor",
                new { Id },
                commandType: System.Data.CommandType.StoredProcedure);
            return filas.Select(MapearMotor).FirstOrDefault();
        }

        public async Task<IEnumerable<MotorResponse>> ObtenerPorUsuario(int usuarioId)
        {
            var filas = await _sqlConnection.QueryAsync<MotorFlat>(
                "ObtenerMotoresPorUsuario",
                new { UsuarioId = usuarioId },
                commandType: System.Data.CommandType.StoredProcedure);
            return filas.Select(MapearMotor);
        }

        // ── Mapeo plano → respuesta con objetos anidados ─────────────
        private static MotorResponse MapearMotor(MotorFlat f) => new()
        {
            Id = f.Id,
            UsuarioId = f.UsuarioId,
            ModeloId = f.ModeloId,
            NumeroSerie = f.NumeroSerie,
            CreadoEn = f.CreadoEn,
            Usuario = f.UsuId == null ? null : new UsuarioResponse
            {
                Id = f.UsuId.Value,
                Nombre = f.UsuNombre ?? "",
                Email = f.UsuEmail ?? "",
                Cedula = f.UsuCedula ?? "",
                Telefono = f.UsuTelefono,
                RolId = f.UsuRolId ?? 0,
                Activo = f.UsuActivo ?? false
            },
            Modelo = f.ModId == null ? null : new ModeloMotorResponse
            {
                Id = f.ModId.Value,
                Nombre = f.ModNombre ?? "",
                Especificaciones = f.ModEspecificaciones
            }
        };

        private async Task verificarMotorExiste(int Id)
        {
            var motor = await Obtener(Id);
            if (motor == null)
                throw new Exception("No se encontró el motor");
        }
    }
}