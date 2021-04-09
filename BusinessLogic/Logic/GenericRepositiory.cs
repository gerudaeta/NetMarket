using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessLogic.Data;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogic.Logic
{
    public class GenericRepositiory<T> : IGenericRepositiory<T> where T : ClaseBase
    {
        private readonly MarketDbContext _marketDbContext;

        public GenericRepositiory(MarketDbContext marketDbContext)
        {
            _marketDbContext = marketDbContext;
        }
        
        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            return await _marketDbContext.Set<T>().ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _marketDbContext.Set<T>().FindAsync(id);
        }
        
        public async Task<T> GetByIdWithSpec(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).FirstOrDefaultAsync();
        }

        public async Task<IReadOnlyList<T>> GetAllWithSpec(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).ToListAsync();
        }
        
        private IQueryable<T> ApplySpecification(ISpecification<T> spec)
        {
            return SpecificationEvaluator<T>.GetQuery(_marketDbContext.Set<T>().AsQueryable(), spec);
        }
        
        public async Task<int> CountAsync(ISpecification<T> specification)
        {
            return await ApplySpecification(specification).CountAsync();
        }

        public async Task<int> Add(T entity)
        {
            _marketDbContext.Set<T>().Add(entity);
            return await _marketDbContext.SaveChangesAsync();
        }

        public async Task<int> Update(T entity)
        {
            _marketDbContext.Set<T>().Attach(entity);
            _marketDbContext.Entry(entity).State = EntityState.Modified;
            return await _marketDbContext.SaveChangesAsync();
        }
    }
}