using Abstracciones.Interfaces.API;
using Abstracciones.Interfaces.Flujo;
using Abstracciones.Modelos;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdenServicioController : ControllerBase, IOrdenServicioController
    {
        private IOrdenServicioFlujo _ordenServicioFlujo;
        private ILogger<OrdenServicioController> _logger;

        public OrdenServicioController(IOrdenServicioFlujo ordenServicioFlujo, ILogger<OrdenServicioController> logger)
        {
            _ordenServicioFlujo = ordenServicioFlujo;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Agregar(OrdenServicioRequest request)
        {
            var resultado = await _ordenServicioFlujo.Agregar(request);
            return CreatedAtAction(nameof(Obtener), new { Id = resultado });
        }

        [HttpPut("{Id}")]
        public async Task<IActionResult> Editar(int Id, OrdenServicioRequest request)
        {
            var resultado = await _ordenServicioFlujo.Editar(Id, request);
            return Ok(resultado);
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> Eliminar(int Id)
        {
            var resultado = await _ordenServicioFlujo.Eliminar(Id);
            return NoContent();
        }

        [HttpGet]
        public async Task<IActionResult> Obtener()
        {
            var resultado = await _ordenServicioFlujo.Obtener();
            if (!resultado.Any())
                return NoContent();
            return Ok(resultado);
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> Obtener(int Id)
        {
            var resultado = await _ordenServicioFlujo.Obtener(Id);
            return Ok(resultado);
        }
    }
}