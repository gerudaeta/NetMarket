using System.Linq;
using Core.Entities;
using Core.Specifications;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogic.Data
{
    public class SpecificationEvaluator<T> where T : ClaseBase
    {
        public static IQueryable<T> GetQuery(IQueryable<T> inputQuery, ISpecification<T> spec)
        {
            if (spec.Criteria is not null)
            {
                inputQuery = inputQuery.Where(spec.Criteria);
            }

            inputQuery = spec.Includes.Aggregate(inputQuery, (current, include) => current.Include(include));

            return inputQuery;
        }
    }
}