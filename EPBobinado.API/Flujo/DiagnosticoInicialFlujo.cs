using Abstracciones.Interfaces.DA;
using Abstracciones.Interfaces.Flujo;
using Abstracciones.Modelos;

namespace Flujo
{
    public class DiagnosticoInicialFlujo : IDiagnosticoInicialFlujo
    {
        private IDiagnosticoInicialDA _diagnosticoInicialDA;

        public DiagnosticoInicialFlujo(IDiagnosticoInicialDA diagnosticoInicialDA)
        {
            _diagnosticoInicialDA = diagnosticoInicialDA;
        }

        public Task<int> Agregar(DiagnosticoInicialRequest diagnosticoInicial)
        {
            return _diagnosticoInicialDA.Agregar(diagnosticoInicial);
        }

        public Task<int> Editar(int Id, DiagnosticoInicialRequest diagnosticoInicial)
        {
            return _diagnosticoInicialDA.Editar(Id, diagnosticoInicial);
        }

        public Task<int> Eliminar(int Id)
        {
            return _diagnosticoInicialDA.Eliminar(Id);
        }

        public Task<IEnumerable<DiagnosticoInicialResponse>> Obtener()
        {
            return _diagnosticoInicialDA.Obtener();
        }

        public Task<DiagnosticoInicialResponse> Obtener(int Id)
        {
            return _diagnosticoInicialDA.Obtener(Id);
        }
    }
}
