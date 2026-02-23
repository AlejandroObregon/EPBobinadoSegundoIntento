using Abstracciones.Interfaces.API;
using Abstracciones.Interfaces.Flujo;
using Abstracciones.Modelos;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CotizacionController : ControllerBase, ICotizacionController
    {
        private ICotizacionFlujo _cotizacionFlujo;
        private ILogger<CotizacionController> _logger;

        public CotizacionController(ICotizacionFlujo cotizacionFlujo, ILogger<CotizacionController> logger)
        {
            _cotizacionFlujo = cotizacionFlujo;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Agregar(CotizacionRequest request)
        {
            var resultado = await _cotizacionFlujo.Agregar(request);
            return CreatedAtAction(nameof(Obtener), new { Id = resultado });
        }

        [HttpPut("{Id}")]
        public async Task<IActionResult> Editar(int Id, CotizacionRequest request)
        {
            var resultado = await _cotizacionFlujo.Editar(Id, request);
            return Ok(resultado);
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> Eliminar(int Id)
        {
            var resultado = await _cotizacionFlujo.Eliminar(Id);
            return NoContent();
        }

        [HttpGet]
        public async Task<IActionResult> Obtener()
        {
            var resultado = await _cotizacionFlujo.Obtener();
            if (!resultado.Any())
                return NoContent();
            return Ok(resultado);
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> Obtener(int Id)
        {
            var resultado = await _cotizacionFlujo.Obtener(Id);
            return Ok(resultado);
        }
    }
}