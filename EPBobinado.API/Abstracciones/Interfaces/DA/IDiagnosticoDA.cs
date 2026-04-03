using Abstracciones.Modelos;

namespace Abstracciones.Interfaces.DA
{
    public interface IDiagnosticoDA
    {
        Task<IEnumerable<DiagnosticoResponse>> Obtener();
        Task<DiagnosticoResponse> Obtener(int Id);
        Task<int> Agregar(DiagnosticoRequest diagnostico);
        Task<int> Editar(int Id, DiagnosticoRequest diagnostico);
        Task<int> Eliminar(int Id);
    }
}
