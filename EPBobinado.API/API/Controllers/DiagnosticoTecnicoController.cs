// API/Controllers/DiagnosticoTecnicoController.cs
using Abstracciones.Interfaces.API;
using Abstracciones.Interfaces.Flujo;
using Abstracciones.Modelos;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiagnosticoTecnicoController : ControllerBase, IDiagnosticoTecnicoController
    {
        private IDiagnosticoTecnicoFlujo _diagnosticoTecnicoFlujo;
        private ILogger<DiagnosticoTecnicoController> _logger;

        public DiagnosticoTecnicoController(IDiagnosticoTecnicoFlujo diagnosticoTecnicoFlujo, ILogger<DiagnosticoTecnicoController> logger)
        {
            _diagnosticoTecnicoFlujo = diagnosticoTecnicoFlujo;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Agregar(DiagnosticoTecnicoRequest request)
        {
            var resultado = await _diagnosticoTecnicoFlujo.Agregar(request);
            return CreatedAtAction(nameof(Obtener), new { Id = resultado });
        }

        [HttpPut("{Id}")]
        public async Task<IActionResult> Editar(int Id, DiagnosticoTecnicoRequest request)
        {
            var resultado = await _diagnosticoTecnicoFlujo.Editar(Id, request);
            return Ok(resultado);
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> Eliminar(int Id)
        {
            var resultado = await _diagnosticoTecnicoFlujo.Eliminar(Id);
            return NoContent();
        }

        [HttpGet]
        public async Task<IActionResult> Obtener()
        {
            var resultado = await _diagnosticoTecnicoFlujo.Obtener();
            if (!resultado.Any())
                return NoContent();
            return Ok(resultado);
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> Obtener(int Id)
        {
            var resultado = await _diagnosticoTecnicoFlujo.Obtener(Id);
            return Ok(resultado);
        }
    }
}