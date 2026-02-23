using Abstracciones.Interfaces.API;
using Abstracciones.Interfaces.Flujo;
using Abstracciones.Modelos;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiagnosticoInicialController : ControllerBase, IDiagnosticoInicialController
    {
        private IDiagnosticoInicialFlujo _diagnosticoInicialFlujo;
        private ILogger<DiagnosticoInicialController> _logger;

        public DiagnosticoInicialController(IDiagnosticoInicialFlujo diagnosticoInicialFlujo, ILogger<DiagnosticoInicialController> logger)
        {
            _diagnosticoInicialFlujo = diagnosticoInicialFlujo;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Agregar(DiagnosticoInicialRequest request)
        {
            var resultado = await _diagnosticoInicialFlujo.Agregar(request);
            return CreatedAtAction(nameof(Obtener), new { Id = resultado });
        }

        [HttpPut("{Id}")]
        public async Task<IActionResult> Editar(int Id, DiagnosticoInicialRequest request)
        {
            var resultado = await _diagnosticoInicialFlujo.Editar(Id, request);
            return Ok(resultado);
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> Eliminar(int Id)
        {
            var resultado = await _diagnosticoInicialFlujo.Eliminar(Id);
            return NoContent();
        }

        [HttpGet]
        public async Task<IActionResult> Obtener()
        {
            var resultado = await _diagnosticoInicialFlujo.Obtener();
            if (!resultado.Any())
                return NoContent();
            return Ok(resultado);
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> Obtener(int Id)
        {
            var resultado = await _diagnosticoInicialFlujo.Obtener(Id);
            return Ok(resultado);
        }
    }
}