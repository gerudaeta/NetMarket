using System;
using System.Text.Json;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;
using StackExchange.Redis;

namespace BusinessLogic.Data
{
    public class CarritoCompraRepository : ICarritoCompraRepository
    {
        private readonly IDatabase _database;

        public CarritoCompraRepository(IConnectionMultiplexer connectionMultiplexer)
        {
            _database = connectionMultiplexer.GetDatabase();
        }

        public async Task<CarritoCompra> ObtenerCarritoCompraAsync(string id)
        {
            var data = await _database.StringGetAsync(id);

            return data.IsNullOrEmpty ? null : JsonSerializer.Deserialize<CarritoCompra>(data);
        }

        public async Task<CarritoCompra> ActualizarCarritoCompraAsync(CarritoCompra carritoCompra)
        {
            var status = await _database.StringSetAsync(carritoCompra.Id, JsonSerializer.Serialize(carritoCompra), TimeSpan.FromDays(30));

            if (!status) return null;

            return await ObtenerCarritoCompraAsync(carritoCompra.Id);
        }

        public async Task<bool> BorrarCarritoCompraAsycn(string id)
        {
            return await _database.KeyDeleteAsync(id);
        }
    }
}