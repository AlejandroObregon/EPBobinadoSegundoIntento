using Abstracciones.Interfaces.API;
using Abstracciones.Interfaces.Flujo;
using Abstracciones.Modelos;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovimientoInventarioController : ControllerBase, IMovimientoInventarioController
    {
        private IMovimientoInventarioFlujo _movimientoInventarioFlujo;
        private ILogger<MovimientoInventarioController> _logger;

        public MovimientoInventarioController(IMovimientoInventarioFlujo movimientoInventarioFlujo, ILogger<MovimientoInventarioController> logger)
        {
            _movimientoInventarioFlujo = movimientoInventarioFlujo;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Agregar(MovimientoInventarioRequest request)
        {
            var resultado = await _movimientoInventarioFlujo.Agregar(request);
            return CreatedAtAction(nameof(Obtener), new { Id = resultado });
        }

        [HttpPut("{Id}")]
        public async Task<IActionResult> Editar(int Id, MovimientoInventarioRequest request)
        {
            var resultado = await _movimientoInventarioFlujo.Editar(Id, request);
            return Ok(resultado);
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> Eliminar(int Id)
        {
            var resultado = await _movimientoInventarioFlujo.Eliminar(Id);
            return NoContent();
        }

        [HttpGet]
        public async Task<IActionResult> Obtener()
        {
            var resultado = await _movimientoInventarioFlujo.Obtener();
            if (!resultado.Any())
                return NoContent();
            return Ok(resultado);
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> Obtener(int Id)
        {
            var resultado = await _movimientoInventarioFlujo.Obtener(Id);
            return Ok(resultado);
        }
    }
}