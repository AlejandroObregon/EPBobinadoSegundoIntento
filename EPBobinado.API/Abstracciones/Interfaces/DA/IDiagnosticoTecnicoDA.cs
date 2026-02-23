using Abstracciones.Modelos;

namespace Abstracciones.Interfaces.DA
{
    public interface IDiagnosticoTecnicoDA
    {
        Task<IEnumerable<DiagnosticoTecnicoResponse>> Obtener();
        Task<DiagnosticoTecnicoResponse> Obtener(int Id);
        Task<int> Agregar(DiagnosticoTecnicoRequest diagnosticoTecnico);
        Task<int> Editar(int Id, DiagnosticoTecnicoRequest diagnosticoTecnico);
        Task<int> Eliminar(int Id);
    }
}
