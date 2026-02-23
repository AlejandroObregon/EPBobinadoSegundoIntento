using Abstracciones.Interfaces.API;
using Abstracciones.Interfaces.Flujo;
using Abstracciones.Modelos;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClienteController : ControllerBase, IClienteController
    {
        private IClienteFlujo _clienteFlujo;
        private ILogger<ClienteController> _logger;

        public ClienteController(IClienteFlujo clienteFlujo, ILogger<ClienteController> logger)
        {
            _clienteFlujo = clienteFlujo;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Agregar(ClienteRequest request)
        {
            var resultado = await _clienteFlujo.Agregar(request);
            return CreatedAtAction(nameof(Obtener), new { Id = resultado });
        }

        [HttpPut("{Id}")]
        public async Task<IActionResult> Editar(int Id, ClienteRequest request)
        {
            var resultado = await _clienteFlujo.Editar(Id, request);
            return Ok(resultado);
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> Eliminar(int Id)
        {
            var resultado = await _clienteFlujo.Eliminar(Id);
            return NoContent();
        }

        [HttpGet]
        public async Task<IActionResult> Obtener()
        {
            var resultado = await _clienteFlujo.Obtener();
            if (!resultado.Any())
                return NoContent();
            return Ok(resultado);
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> Obtener(int Id)
        {
            var resultado = await _clienteFlujo.Obtener(Id);
            return Ok(resultado);
        }
    }
}