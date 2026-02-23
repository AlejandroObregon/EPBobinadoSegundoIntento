using Abstracciones.Interfaces.API;
using Abstracciones.Interfaces.Flujo;
using Abstracciones.Modelos;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConfiguracionImpuestoController : ControllerBase, IConfiguracionImpuestoController
    {
        private IConfiguracionImpuestoFlujo _configuracionImpuestoFlujo;
        private ILogger<ConfiguracionImpuestoController> _logger;

        public ConfiguracionImpuestoController(IConfiguracionImpuestoFlujo configuracionImpuestoFlujo, ILogger<ConfiguracionImpuestoController> logger)
        {
            _configuracionImpuestoFlujo = configuracionImpuestoFlujo;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Agregar(ConfiguracionImpuestoRequest request)
        {
            var resultado = await _configuracionImpuestoFlujo.Agregar(request);
            return CreatedAtAction(nameof(Obtener), new { Id = resultado });
        }

        [HttpPut("{Id}")]
        public async Task<IActionResult> Editar(int Id, ConfiguracionImpuestoRequest request)
        {
            var resultado = await _configuracionImpuestoFlujo.Editar(Id, request);
            return Ok(resultado);
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> Eliminar(int Id)
        {
            var resultado = await _configuracionImpuestoFlujo.Eliminar(Id);
            return NoContent();
        }

        [HttpGet]
        public async Task<IActionResult> Obtener()
        {
            var resultado = await _configuracionImpuestoFlujo.Obtener();
            if (!resultado.Any())
                return NoContent();
            return Ok(resultado);
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> Obtener(int Id)
        {
            var resultado = await _configuracionImpuestoFlujo.Obtener(Id);
            return Ok(resultado);
        }
    }
}