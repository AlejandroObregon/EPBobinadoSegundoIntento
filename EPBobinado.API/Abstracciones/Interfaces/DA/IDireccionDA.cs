using Abstracciones.Modelos;

namespace Abstracciones.Interfaces.DA
{
    public interface IDireccionDA
    {
        Task<IEnumerable<DireccionResponse>> Obtener();
        Task<DireccionResponse> Obtener(int Id);
        Task<int> Agregar(DireccionRequest direccion);
        Task<int> Editar(int Id, DireccionRequest direccion);
        Task<int> Eliminar(int Id);
    }
}
