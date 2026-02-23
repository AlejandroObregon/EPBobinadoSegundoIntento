using Abstracciones.Modelos;

namespace Abstracciones.Interfaces.DA
{
    public interface IOrdenServicioDA
    {
        Task<IEnumerable<OrdenServicioResponse>> Obtener();
        Task<OrdenServicioResponse> Obtener(int Id);
        Task<int> Agregar(OrdenServicioRequest ordenServicio);
        Task<int> Editar(int Id, OrdenServicioRequest ordenServicio);
        Task<int> Eliminar(int Id);
    }
}
