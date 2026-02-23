// API/Controllers/BitacoraController.cs
using Abstracciones.Interfaces.API;
using Abstracciones.Interfaces.Flujo;
using Abstracciones.Modelos;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BitacoraController : ControllerBase, IBitacoraController
    {
        private IBitacoraFlujo _bitacoraFlujo;
        private ILogger<BitacoraController> _logger;

        public BitacoraController(IBitacoraFlujo bitacoraFlujo, ILogger<BitacoraController> logger)
        {
            _bitacoraFlujo = bitacoraFlujo;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Agregar(BitacoraRequest request)
        {
            var resultado = await _bitacoraFlujo.Agregar(request);
            return CreatedAtAction(nameof(Obtener), new { Id = resultado });
        }

        [HttpPut("{Id}")]
        public async Task<IActionResult> Editar(int Id, BitacoraRequest request)
        {
            var resultado = await _bitacoraFlujo.Editar(Id, request);
            return Ok(resultado);
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> Eliminar(int Id)
        {
            var resultado = await _bitacoraFlujo.Eliminar(Id);
            return NoContent();
        }

        [HttpGet]
        public async Task<IActionResult> Obtener()
        {
            var resultado = await _bitacoraFlujo.Obtener();
            if (!resultado.Any())
                return NoContent();
            return Ok(resultado);
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> Obtener(int Id)
        {
            var resultado = await _bitacoraFlujo.Obtener(Id);
            return Ok(resultado);
        }
    }
}