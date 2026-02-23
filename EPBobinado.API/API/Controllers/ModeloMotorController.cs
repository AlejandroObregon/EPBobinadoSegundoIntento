using Abstracciones.Interfaces.API;
using Abstracciones.Interfaces.Flujo;
using Abstracciones.Modelos;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ModeloMotorController : ControllerBase, IModeloMotorController
    {
        private IModeloMotorFlujo _modeloMotorFlujo;
        private ILogger<ModeloMotorController> _logger;

        public ModeloMotorController(IModeloMotorFlujo modeloMotorFlujo, ILogger<ModeloMotorController> logger)
        {
            _modeloMotorFlujo = modeloMotorFlujo;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Agregar(ModeloMotorRequest request)
        {
            var resultado = await _modeloMotorFlujo.Agregar(request);
            return CreatedAtAction(nameof(Obtener), new { Id = resultado });
        }

        [HttpPut("{Id}")]
        public async Task<IActionResult> Editar(int Id, ModeloMotorRequest request)
        {
            var resultado = await _modeloMotorFlujo.Editar(Id, request);
            return Ok(resultado);
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> Eliminar(int Id)
        {
            var resultado = await _modeloMotorFlujo.Eliminar(Id);
            return NoContent();
        }

        [HttpGet]
        public async Task<IActionResult> Obtener()
        {
            var resultado = await _modeloMotorFlujo.Obtener();
            if (!resultado.Any())
                return NoContent();
            return Ok(resultado);
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> Obtener(int Id)
        {
            var resultado = await _modeloMotorFlujo.Obtener(Id);
            return Ok(resultado);
        }
    }
}