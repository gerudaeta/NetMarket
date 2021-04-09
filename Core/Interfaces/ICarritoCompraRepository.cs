using System.Threading.Tasks;
using Core.Entities;

namespace Core.Interfaces
{
    public interface ICarritoCompraRepository
    {
        Task<CarritoCompra> ObtenerCarritoCompraAsync(string id);
        Task<CarritoCompra> ActualizarCarritoCompraAsync(CarritoCompra carritoCompra);
        Task<bool> BorrarCarritoCompraAsycn(string id);
    }
}