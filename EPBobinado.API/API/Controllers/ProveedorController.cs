using Abstracciones.Interfaces.API;
using Abstracciones.Interfaces.Flujo;
using Abstracciones.Modelos;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProveedorController : ControllerBase, IProveedorController
    {
        private IProveedorFlujo _proveedorFlujo;
        private ILogger<ProveedorController> _logger;

        public ProveedorController(IProveedorFlujo proveedorFlujo, ILogger<ProveedorController> logger)
        {
            _proveedorFlujo = proveedorFlujo;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Agregar(ProveedorRequest request)
        {
            var resultado = await _proveedorFlujo.Agregar(request);
            return CreatedAtAction(nameof(Obtener), new { Id = resultado });
        }

        [HttpPut("{Id}")]
        public async Task<IActionResult> Editar(int Id, ProveedorRequest request)
        {
            var resultado = await _proveedorFlujo.Editar(Id, request);
            return Ok(resultado);
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> Eliminar(int Id)
        {
            var resultado = await _proveedorFlujo.Eliminar(Id);
            return NoContent();
        }

        [HttpGet]
        public async Task<IActionResult> Obtener()
        {
            var resultado = await _proveedorFlujo.Obtener();
            if (!resultado.Any())
                return NoContent();
            return Ok(resultado);
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> Obtener(int Id)
        {
            var resultado = await _proveedorFlujo.Obtener(Id);
            return Ok(resultado);
        }
    }
}