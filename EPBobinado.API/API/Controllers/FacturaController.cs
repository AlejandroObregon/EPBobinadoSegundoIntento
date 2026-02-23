using Abstracciones.Interfaces.API;
using Abstracciones.Interfaces.Flujo;
using Abstracciones.Modelos;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FacturaController : ControllerBase, IFacturaController
    {
        private IFacturaFlujo _facturaFlujo;
        private ILogger<FacturaController> _logger;

        public FacturaController(IFacturaFlujo facturaFlujo, ILogger<FacturaController> logger)
        {
            _facturaFlujo = facturaFlujo;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Agregar(FacturaRequest request)
        {
            var resultado = await _facturaFlujo.Agregar(request);
            return CreatedAtAction(nameof(Obtener), new { Id = resultado });
        }

        [HttpPut("{Id}")]
        public async Task<IActionResult> Editar(int Id, FacturaRequest request)
        {
            var resultado = await _facturaFlujo.Editar(Id, request);
            return Ok(resultado);
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> Eliminar(int Id)
        {
            var resultado = await _facturaFlujo.Eliminar(Id);
            return NoContent();
        }

        [HttpGet]
        public async Task<IActionResult> Obtener()
        {
            var resultado = await _facturaFlujo.Obtener();
            if (!resultado.Any())
                return NoContent();
            return Ok(resultado);
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> Obtener(int Id)
        {
            var resultado = await _facturaFlujo.Obtener(Id);
            return Ok(resultado);
        }
    }
}