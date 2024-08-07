using Microsoft.EntityFrameworkCore;
using OrderManagement.Core.Entities;
using OrderManagement.Core.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Repository
{
    public static class SpecificationEvaluator<T> where T:BaseEntity
    {
        public static IQueryable<T> GetQuery (IQueryable<T> inputQuery, ISpecification<T> spec)
        {
            var query = inputQuery;
            if (spec.Critria is not null)
                query = query.Where(spec.Critria);
            query = spec.Includes.Aggregate(query, (currentQury, IncludeExpression) => currentQury.Include(IncludeExpression));
            return query;
        }
    }
}
