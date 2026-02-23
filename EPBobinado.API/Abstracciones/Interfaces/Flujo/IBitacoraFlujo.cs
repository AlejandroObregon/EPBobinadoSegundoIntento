using Abstracciones.Modelos;

namespace Abstracciones.Interfaces.Flujo
{
    public interface IBitacoraFlujo
    {
        Task<IEnumerable<BitacoraResponse>> Obtener();
        Task<BitacoraResponse> Obtener(int Id);
        Task<int> Agregar(BitacoraRequest bitacora);
        Task<int> Editar(int Id, BitacoraRequest bitacora);
        Task<int> Eliminar(int Id);
    }
}
