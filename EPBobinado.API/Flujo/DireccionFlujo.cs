using Abstracciones.Interfaces.DA;
using Abstracciones.Interfaces.Flujo;
using Abstracciones.Modelos;

namespace Flujo
{
    public class DireccionFlujo : IDireccionFlujo
    {
        private IDireccionDA _direccionDA;

        public DireccionFlujo(IDireccionDA direccionDA)
        {
            _direccionDA = direccionDA;
        }

        public Task<int> Agregar(DireccionRequest direccion)
        {
            return _direccionDA.Agregar(direccion);
        }

        public Task<int> Editar(int Id, DireccionRequest direccion)
        {
            return _direccionDA.Editar(Id, direccion);
        }

        public Task<int> Eliminar(int Id)
        {
            return _direccionDA.Eliminar(Id);
        }

        public Task<IEnumerable<DireccionResponse>> Obtener()
        {
            return _direccionDA.Obtener();
        }

        public Task<DireccionResponse> Obtener(int Id)
        {
            return _direccionDA.Obtener(Id);
        }
    }
}
