using Abstracciones.Interfaces.API;
using Abstracciones.Interfaces.Flujo;
using Abstracciones.Modelos;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase, IUsuarioController
    {
        private IUsuarioFlujo _usuarioFlujo;
        private ILogger<UsuarioController> _logger;

        public UsuarioController(IUsuarioFlujo usuarioFlujo, ILogger<UsuarioController> logger)
        {
            _usuarioFlujo = usuarioFlujo;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Agregar(UsuarioRequest request)
        {
            var resultado = await _usuarioFlujo.Agregar(request);
            return CreatedAtAction(nameof(Obtener), new { Id = resultado });
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(Abstracciones.Modelos.LoginRequest request)
        {
            var resultado = await _usuarioFlujo.Login(request.Email, request.Password);
            if (resultado == null)
                return Unauthorized("Correo o contraseña incorrectos.");
            return Ok(resultado);
        }

        [HttpPut("{Id}")]
        public async Task<IActionResult> Editar(int Id, UsuarioRequest request)
        {
            var resultado = await _usuarioFlujo.Editar(Id, request);
            return Ok(resultado);
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> Eliminar(int Id)
        {
            var resultado = await _usuarioFlujo.Eliminar(Id);
            return NoContent();
        }

        [HttpGet]
        public async Task<IActionResult> Obtener()
        {
            var resultado = await _usuarioFlujo.Obtener();
            if (!resultado.Any())
                return NoContent();
            return Ok(resultado);
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> Obtener(int Id)
        {
            var resultado = await _usuarioFlujo.Obtener(Id);
            return Ok(resultado);
        }
    }
}