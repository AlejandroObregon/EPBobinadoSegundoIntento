using Abstracciones.Interfaces.API;
using Abstracciones.Interfaces.Flujo;
using Abstracciones.Modelos;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConfiguracionPrecioController : ControllerBase, IConfiguracionPrecioController
    {
        private IConfiguracionPrecioFlujo _configuracionPrecioFlujo;
        private ILogger<ConfiguracionPrecioController> _logger;

        public ConfiguracionPrecioController(IConfiguracionPrecioFlujo configuracionPrecioFlujo, ILogger<ConfiguracionPrecioController> logger)
        {
            _configuracionPrecioFlujo = configuracionPrecioFlujo;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Agregar(ConfiguracionPrecioRequest request)
        {
            var resultado = await _configuracionPrecioFlujo.Agregar(request);
            return CreatedAtAction(nameof(Obtener), new { Id = resultado });
        }

        [HttpPut("{Id}")]
        public async Task<IActionResult> Editar(int Id, ConfiguracionPrecioRequest request)
        {
            var resultado = await _configuracionPrecioFlujo.Editar(Id, request);
            return Ok(resultado);
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> Eliminar(int Id)
        {
            var resultado = await _configuracionPrecioFlujo.Eliminar(Id);
            return NoContent();
        }

        [HttpGet]
        public async Task<IActionResult> Obtener()
        {
            var resultado = await _configuracionPrecioFlujo.Obtener();
            if (!resultado.Any())
                return NoContent();
            return Ok(resultado);
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> Obtener(int Id)
        {
            var resultado = await _configuracionPrecioFlujo.Obtener(Id);
            return Ok(resultado);
        }
    }
}