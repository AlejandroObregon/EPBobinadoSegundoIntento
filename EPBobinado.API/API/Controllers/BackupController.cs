using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BackupController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly ILogger<BackupController> _logger;

        public BackupController(IConfiguration config, ILogger<BackupController> logger)
        {
            _config = config;
            _logger = logger;
        }

        // ── GET api/Backup/Bak ───────────────────────────────────────
        // Ejecuta BACKUP DATABASE y retorna el archivo .bak para descargar
        [HttpGet("Bak")]
        public async Task<IActionResult> GenerarBak()
        {
            try
            {
                var connStr = _config.GetConnectionString("BD")
                               ?? throw new InvalidOperationException("No se encontró la cadena de conexión.");

                var nombre = $"EPBobinadoDB_{DateTime.Now:yyyyMMdd_HHmmss}.bak";

                // Carpeta configurable en appsettings.json → "BackupPath"
                // Debe tener permisos para la cuenta de SQL Server Y para el proceso de la API.
                // Por defecto: C:\EPBobinadoBackups\
                var carpeta = (_config.GetValue<string>("BackupPath") ?? @"C:\EPBobinadoBackups\").TrimEnd('\\');
                Directory.CreateDirectory(carpeta);   // crea la carpeta si no existe
                var rutaBak = Path.Combine(carpeta, nombre);

                // Obtener el nombre real de la base de datos desde la conexión
                string dbName;
                using (var conn = new SqlConnection(connStr))
                {
                    await conn.OpenAsync();
                    dbName = conn.Database;

                    var sql = $"BACKUP DATABASE [{dbName}] TO DISK = @ruta WITH FORMAT, INIT, STATS = 10";
                    using var cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@ruta", rutaBak);
                    cmd.CommandTimeout = 300; // 5 minutos máximo
                    await cmd.ExecuteNonQueryAsync();
                }

                // Leer el archivo y retornarlo como descarga
                var bytes = await System.IO.File.ReadAllBytesAsync(rutaBak);

                // Limpiar el archivo temporal
                System.IO.File.Delete(rutaBak);

                return File(bytes, "application/octet-stream", nombre);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al generar backup .bak");
                return StatusCode(500, new { error = ex.Message });
            }
        }

        // ── GET api/Backup/Sql ───────────────────────────────────────
        // Genera un script .sql con INSERTs de todas las tablas
        [HttpGet("Sql")]
        public async Task<IActionResult> GenerarSql()
        {
            try
            {
                var connStr = _config.GetConnectionString("BD")
                              ?? throw new InvalidOperationException("No se encontró la cadena de conexión.");

                var script = await GenerarScriptSqlAsync(connStr);
                var nombre = $"EPBobinadoDB_{DateTime.Now:yyyyMMdd_HHmmss}.sql";
                var bytes = System.Text.Encoding.UTF8.GetBytes(script);

                return File(bytes, "application/sql", nombre);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al generar backup .sql");
                return StatusCode(500, new { error = ex.Message });
            }
        }

        // ── Helpers ───────────────────────────────────────────────────
        private static async Task<string> GenerarScriptSqlAsync(string connStr)
        {
            var sb = new System.Text.StringBuilder();
            sb.AppendLine("-- ================================================");
            sb.AppendLine($"-- Backup EPBobinadoDB — {DateTime.Now:dd/MM/yyyy HH:mm:ss}");
            sb.AppendLine("-- ================================================");
            sb.AppendLine("SET NOCOUNT ON;");
            sb.AppendLine("SET IDENTITY_INSERT_SAFE OFF;");
            sb.AppendLine();

            // Tablas en orden de dependencia para respetar FK
            var tablas = new[]
            {
                "Roles", "Direcciones", "Usuarios",
                "ModeloMotores", "Motores",
                "OrdenesServicio",
                "DiagnosticosIniciales", "DiagnosticosTecnicos",
                "Productos", "DetallesOrden",
                "Cotizaciones"
            };

            using var conn = new SqlConnection(connStr);
            await conn.OpenAsync();

            foreach (var tabla in tablas)
            {
                // Verificar que la tabla exista
                var existsCmd = new SqlCommand(
                    "SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = @t", conn);
                existsCmd.Parameters.AddWithValue("@t", tabla);
                var existe = (int)(await existsCmd.ExecuteScalarAsync() ?? 0);
                if (existe == 0) continue;

                sb.AppendLine($"-- ── {tabla} ──────────────────────────────────────");
                sb.AppendLine($"SET IDENTITY_INSERT {tabla} ON;");

                using var cmd = new SqlCommand($"SELECT * FROM {tabla}", conn);
                using var reader = await cmd.ExecuteReaderAsync();

                bool hayFilas = false;
                var cols = Enumerable.Range(0, reader.FieldCount)
                                     .Select(i => reader.GetName(i))
                                     .ToList();

                while (await reader.ReadAsync())
                {
                    hayFilas = true;
                    var valores = cols.Select(c =>
                    {
                        var val = reader[c];
                        if (val == DBNull.Value) return "NULL";
                        return val switch
                        {
                            bool b => b ? "1" : "0",
                            int i => i.ToString(),
                            long l => l.ToString(),
                            decimal d => d.ToString(System.Globalization.CultureInfo.InvariantCulture),
                            double d => d.ToString(System.Globalization.CultureInfo.InvariantCulture),
                            DateTime dt => $"'{dt:yyyy-MM-dd HH:mm:ss}'",
                            _ => $"'{val.ToString()!.Replace("'", "''")}'"
                        };
                    });

                    sb.AppendLine(
                        $"INSERT INTO {tabla} ({string.Join(", ", cols)}) " +
                        $"VALUES ({string.Join(", ", valores)});"
                    );
                }

                if (!hayFilas)
                    sb.AppendLine($"-- (sin datos)");

                sb.AppendLine($"SET IDENTITY_INSERT {tabla} OFF;");
                sb.AppendLine();
            }

            sb.AppendLine("-- ── Fin del backup ────────────────────────────────");
            return sb.ToString();
        }
    }
}