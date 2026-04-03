using Abstracciones.Interfaces.DA;
using Abstracciones.Interfaces.Flujo;
using Abstracciones.Modelos;

namespace Flujo
{
    public class DiagnosticoFlujo : IDiagnosticoFlujo
    {
        private IDiagnosticoDA _diagnosticoDA;

        public DiagnosticoFlujo(IDiagnosticoDA diagnosticoDA)
        {
            _diagnosticoDA = diagnosticoDA;
        }

        public Task<int> Agregar(DiagnosticoRequest diagnostico)
        {
            return _diagnosticoDA.Agregar(diagnostico);
        }

        public Task<int> Editar(int Id, DiagnosticoRequest diagnostico)
        {
            return _diagnosticoDA.Editar(Id, diagnostico);
        }

        public Task<int> Eliminar(int Id)
        {
            return _diagnosticoDA.Eliminar(Id);
        }

        public Task<IEnumerable<DiagnosticoResponse>> Obtener()
        {
            return _diagnosticoDA.Obtener();
        }

        public Task<DiagnosticoResponse> Obtener(int Id)
        {
            return _diagnosticoDA.Obtener(Id);
        }
    }
}
