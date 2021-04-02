using BusinessLogic.Data;
using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Logic
{
    public class ProductoRepository : IProductoRepository
    {
        private readonly MarketDbContext _marketDbContext;

        public ProductoRepository(MarketDbContext marketDbContext)
        {
            _marketDbContext = marketDbContext;
        }

        public async Task<Producto> GetProductoById(int id)
        {
            return await _marketDbContext.Productos
                .Include(x => x.Marca)
                .Include(x => x.Categoria)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IReadOnlyList<Producto>> GetProductos()
        {
            return await _marketDbContext.Productos
                .Include(x => x.Marca)
                .Include(x => x.Categoria)
                .ToListAsync();
        }
    }
}
