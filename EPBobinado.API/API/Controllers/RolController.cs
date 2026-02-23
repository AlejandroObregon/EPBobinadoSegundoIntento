using Abstracciones.Interfaces.API;
using Abstracciones.Interfaces.Flujo;
using Abstracciones.Modelos;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolController : ControllerBase, IRolController
    {
        private IRolFlujo _rolFlujo;
        private ILogger<RolController> _logger;

        public RolController(IRolFlujo rolFlujo, ILogger<RolController> logger)
        {
            _rolFlujo = rolFlujo;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Agregar(RolRequest request)
        {
            var resultado = await _rolFlujo.Agregar(request);
            return CreatedAtAction(nameof(Obtener), new { Id = resultado });
        }

        [HttpPut("{Id}")]
        public async Task<IActionResult> Editar(int Id, RolRequest request)
        {
            var resultado = await _rolFlujo.Editar(Id, request);
            return Ok(resultado);
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> Eliminar(int Id)
        {
            var resultado = await _rolFlujo.Eliminar(Id);
            return NoContent();
        }

        [HttpGet]
        public async Task<IActionResult> Obtener()
        {
            var resultado = await _rolFlujo.Obtener();
            if (!resultado.Any())
                return NoContent();
            return Ok(resultado);
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> Obtener(int Id)
        {
            var resultado = await _rolFlujo.Obtener(Id);
            return Ok(resultado);
        }
    }
}