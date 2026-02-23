using Abstracciones.Interfaces.DA;
using Abstracciones.Interfaces.Flujo;
using Abstracciones.Modelos;

namespace Flujo
{
    public class DiagnosticoTecnicoFlujo : IDiagnosticoTecnicoFlujo
    {
        private IDiagnosticoTecnicoDA _diagnosticoTecnicoDA;

        public DiagnosticoTecnicoFlujo(IDiagnosticoTecnicoDA diagnosticoTecnicoDA)
        {
            _diagnosticoTecnicoDA = diagnosticoTecnicoDA;
        }

        public Task<int> Agregar(DiagnosticoTecnicoRequest diagnosticoTecnico)
        {
            return _diagnosticoTecnicoDA.Agregar(diagnosticoTecnico);
        }

        public Task<int> Editar(int Id, DiagnosticoTecnicoRequest diagnosticoTecnico)
        {
            return _diagnosticoTecnicoDA.Editar(Id, diagnosticoTecnico);
        }

        public Task<int> Eliminar(int Id)
        {
            return _diagnosticoTecnicoDA.Eliminar(Id);
        }

        public Task<IEnumerable<DiagnosticoTecnicoResponse>> Obtener()
        {
            return _diagnosticoTecnicoDA.Obtener();
        }

        public Task<DiagnosticoTecnicoResponse> Obtener(int Id)
        {
            return _diagnosticoTecnicoDA.Obtener(Id);
        }
    }
}
