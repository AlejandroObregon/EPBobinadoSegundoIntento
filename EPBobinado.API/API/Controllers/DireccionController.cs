using Abstracciones.Interfaces.API;
using Abstracciones.Interfaces.Flujo;
using Abstracciones.Modelos;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DireccionController : ControllerBase, IDireccionController
    {
        private IDireccionFlujo _direccionFlujo;
        private ILogger<DireccionController> _logger;

        public DireccionController(IDireccionFlujo direccionFlujo, ILogger<DireccionController> logger)
        {
            _direccionFlujo = direccionFlujo;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Agregar(DireccionRequest request)
        {
            var resultado = await _direccionFlujo.Agregar(request);
            return CreatedAtAction(nameof(Obtener), new { Id = resultado });
        }

        [HttpPut("{Id}")]
        public async Task<IActionResult> Editar(int Id, DireccionRequest request)
        {
            var resultado = await _direccionFlujo.Editar(Id, request);
            return Ok(resultado);
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> Eliminar(int Id)
        {
            var resultado = await _direccionFlujo.Eliminar(Id);
            return NoContent();
        }

        [HttpGet]
        public async Task<IActionResult> Obtener()
        {
            var resultado = await _direccionFlujo.Obtener();
            if (!resultado.Any())
                return NoContent();
            return Ok(resultado);
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> Obtener(int Id)
        {
            var resultado = await _direccionFlujo.Obtener(Id);
            return Ok(resultado);
        }
    }
}