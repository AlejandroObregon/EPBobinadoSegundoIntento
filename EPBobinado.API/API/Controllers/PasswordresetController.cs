using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace API.Controllers
{
    [Route("api/Usuario/TokenReset")]
    [ApiController]
    public class PasswordResetController : ControllerBase
    {
        private readonly IConfiguration _config;

        public PasswordResetController(IConfiguration config)
        {
            _config = config;
        }

        // POST api/Usuario/TokenReset → crear token
        [HttpPost]
        public async Task<IActionResult> CrearToken([FromBody] CrearTokenRequest req)
        {
            using var conn = new SqlConnection(_config.GetConnectionString("BD"));
            await conn.ExecuteAsync("CrearTokenReset", new
            {
                req.UsuarioId,
                req.Token,
                req.Expiracion
            }, commandType: System.Data.CommandType.StoredProcedure);

            return Ok();
        }

        // GET api/Usuario/TokenReset/{token} → validar token
        [HttpGet("{token}")]
        public async Task<IActionResult> ValidarToken(string token)
        {
            using var conn = new SqlConnection(_config.GetConnectionString("BD"));
            var resultado = await conn.QueryFirstOrDefaultAsync(
                "ValidarTokenReset",
                new { Token = token },
                commandType: System.Data.CommandType.StoredProcedure);

            if (resultado == null) return NotFound();
            return Ok(resultado);
        }

        // POST api/Usuario/TokenReset/Usar → marcar token como usado
        [HttpPost("Usar")]
        public async Task<IActionResult> UsarToken([FromBody] UsarTokenRequest req)
        {
            using var conn = new SqlConnection(_config.GetConnectionString("BD"));
            await conn.ExecuteAsync("UsarTokenReset",
                new { Token = req.Token },
                commandType: System.Data.CommandType.StoredProcedure);

            return Ok();
        }

        public class CrearTokenRequest
        {
            public int UsuarioId { get; set; }
            public string Token { get; set; } = "";
            public DateTime Expiracion { get; set; }
        }

        public class UsarTokenRequest
        {
            public string Token { get; set; } = "";
        }
    }
}