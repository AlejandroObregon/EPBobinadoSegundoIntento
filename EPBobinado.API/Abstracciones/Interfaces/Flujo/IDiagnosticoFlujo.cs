using Abstracciones.Modelos;

namespace Abstracciones.Interfaces.Flujo
{
    public interface IDiagnosticoFlujo
    {
        Task<IEnumerable<DiagnosticoResponse>> Obtener();
        Task<DiagnosticoResponse> Obtener(int Id);
        Task<int> Agregar(DiagnosticoRequest diagnostico);
        Task<int> Editar(int Id, DiagnosticoRequest diagnostico);
        Task<int> Eliminar(int Id);
    }
}
