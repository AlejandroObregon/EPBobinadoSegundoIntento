// API/Controllers/DiagnosticoController.cs
using Abstracciones.Interfaces.API;
using Abstracciones.Interfaces.Flujo;
using Abstracciones.Modelos;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiagnosticoController : ControllerBase, IDiagnosticoController
    {
        private IDiagnosticoFlujo _diagnosticoFlujo;
        private ILogger<DiagnosticoController> _logger;

        public DiagnosticoController(IDiagnosticoFlujo diagnosticoFlujo, ILogger<DiagnosticoController> logger)
        {
            _diagnosticoFlujo = diagnosticoFlujo;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Agregar(DiagnosticoRequest request)
        {
            var resultado = await _diagnosticoFlujo.Agregar(request);
            return CreatedAtAction(nameof(Obtener), new { Id = resultado });
        }

        [HttpPut("{Id}")]
        public async Task<IActionResult> Editar(int Id, DiagnosticoRequest request)
        {
            var resultado = await _diagnosticoFlujo.Editar(Id, request);
            return Ok(resultado);
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> Eliminar(int Id)
        {
            var resultado = await _diagnosticoFlujo.Eliminar(Id);
            return NoContent();
        }

        [HttpGet]
        public async Task<IActionResult> Obtener()
        {
            var resultado = await _diagnosticoFlujo.Obtener();
            if (!resultado.Any())
                return NoContent();
            return Ok(resultado);
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> Obtener(int Id)
        {
            var resultado = await _diagnosticoFlujo.Obtener(Id);
            return Ok(resultado);
        }
    }
}