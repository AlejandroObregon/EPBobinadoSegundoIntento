using Abstracciones.Interfaces.API;
using Abstracciones.Interfaces.Flujo;
using Abstracciones.Modelos;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SesionController : ControllerBase, ISesionController
    {
        private ISesionFlujo _sesionFlujo;
        private ILogger<SesionController> _logger;

        public SesionController(ISesionFlujo sesionFlujo, ILogger<SesionController> logger)
        {
            _sesionFlujo = sesionFlujo;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Agregar(SesionRequest request)
        {
            var resultado = await _sesionFlujo.Agregar(request);
            return CreatedAtAction(nameof(Obtener), new { Id = resultado });
        }

        [HttpPut("{Id}")]
        public async Task<IActionResult> Editar(int Id, SesionRequest request)
        {
            var resultado = await _sesionFlujo.Editar(Id, request);
            return Ok(resultado);
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> Eliminar(int Id)
        {
            var resultado = await _sesionFlujo.Eliminar(Id);
            return NoContent();
        }

        [HttpGet]
        public async Task<IActionResult> Obtener()
        {
            var resultado = await _sesionFlujo.Obtener();
            if (!resultado.Any())
                return NoContent();
            return Ok(resultado);
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> Obtener(int Id)
        {
            var resultado = await _sesionFlujo.Obtener(Id);
            return Ok(resultado);
        }
    }
}