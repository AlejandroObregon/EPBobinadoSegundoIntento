using Abstracciones.Modelos;

namespace Abstracciones.Interfaces.Flujo
{
    public interface IDiagnosticoInicialFlujo
    {
        Task<IEnumerable<DiagnosticoInicialResponse>> Obtener();
        Task<DiagnosticoInicialResponse> Obtener(int Id);
        Task<int> Agregar(DiagnosticoInicialRequest diagnosticoInicial);
        Task<int> Editar(int Id, DiagnosticoInicialRequest diagnosticoInicial);
        Task<int> Eliminar(int Id);
    }
}
