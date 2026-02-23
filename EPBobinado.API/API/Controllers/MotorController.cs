using Abstracciones.Interfaces.API;
using Abstracciones.Interfaces.Flujo;
using Abstracciones.Modelos;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MotorController : ControllerBase, IMotorController
    {
        private IMotorFlujo _motorFlujo;
        private ILogger<MotorController> _logger;

        public MotorController(IMotorFlujo motorFlujo, ILogger<MotorController> logger)
        {
            _motorFlujo = motorFlujo;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Agregar(MotorRequest request)
        {
            var resultado = await _motorFlujo.Agregar(request);
            return CreatedAtAction(nameof(Obtener), new { Id = resultado });
        }

        [HttpPut("{Id}")]
        public async Task<IActionResult> Editar(int Id, MotorRequest request)
        {
            var resultado = await _motorFlujo.Editar(Id, request);
            return Ok(resultado);
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> Eliminar(int Id)
        {
            var resultado = await _motorFlujo.Eliminar(Id);
            return NoContent();
        }

        [HttpGet]
        public async Task<IActionResult> Obtener()
        {
            var resultado = await _motorFlujo.Obtener();
            if (!resultado.Any())
                return NoContent();
            return Ok(resultado);
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> Obtener(int Id)
        {
            var resultado = await _motorFlujo.Obtener(Id);
            return Ok(resultado);
        }
    }
}