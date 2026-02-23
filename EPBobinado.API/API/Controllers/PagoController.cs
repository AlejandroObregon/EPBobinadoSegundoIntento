using Abstracciones.Interfaces.API;
using Abstracciones.Interfaces.Flujo;
using Abstracciones.Modelos;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PagoController : ControllerBase, IPagoController
    {
        private IPagoFlujo _pagoFlujo;
        private ILogger<PagoController> _logger;

        public PagoController(IPagoFlujo pagoFlujo, ILogger<PagoController> logger)
        {
            _pagoFlujo = pagoFlujo;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Agregar(PagoRequest request)
        {
            var resultado = await _pagoFlujo.Agregar(request);
            return CreatedAtAction(nameof(Obtener), new { Id = resultado });
        }

        [HttpPut("{Id}")]
        public async Task<IActionResult> Editar(int Id, PagoRequest request)
        {
            var resultado = await _pagoFlujo.Editar(Id, request);
            return Ok(resultado);
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> Eliminar(int Id)
        {
            var resultado = await _pagoFlujo.Eliminar(Id);
            return NoContent();
        }

        [HttpGet]
        public async Task<IActionResult> Obtener()
        {
            var resultado = await _pagoFlujo.Obtener();
            if (!resultado.Any())
                return NoContent();
            return Ok(resultado);
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> Obtener(int Id)
        {
            var resultado = await _pagoFlujo.Obtener(Id);
            return Ok(resultado);
        }
    }
}