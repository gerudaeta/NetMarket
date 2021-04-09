using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    public class CarritoComprasController : BaseApiController
    {
        private readonly ICarritoCompraRepository _carritoCompraRepository;

        public CarritoComprasController(ICarritoCompraRepository carritoCompraRepository)
        {
            _carritoCompraRepository = carritoCompraRepository;
        }

        [HttpGet]
        public async Task<ActionResult<CarritoCompra>> ObtenerCarritoCompraPorId(string id)
        {
            var carritoCompra = await _carritoCompraRepository.ObtenerCarritoCompraAsync(id);

            return Ok(carritoCompra ?? new CarritoCompra(id));
        }
        
        [HttpPost]
        public async Task<ActionResult<CarritoCompra>> ActualizarCarritoCompra(CarritoCompra carritoCompra)
        {
            var carritoActualizado = await _carritoCompraRepository.ActualizarCarritoCompraAsync(carritoCompra);
            return Ok(carritoActualizado);
        }
        
        [HttpDelete]
        public async Task BorrarCarritoCompraPorId(string id)
        {
            await _carritoCompraRepository.BorrarCarritoCompraAsycn(id);
        }
    }
}