using OrderManagement.Core.Entities;
using OrderManagement.Core.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Core.Repositories.Contracts
{
    public  interface IGenericRepository<T> where T :BaseEntity
    {
        Task<T?> GetAsync(int id);
        Task<IReadOnlyList<T>> GetAllAsync();

        Task<T?> GetEntityWithSpecAsync(ISpecification<T> spec);
        Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecification<T> spec);

        //Task<int> GetCountAsync(ISpecification<T> spec);

        Task AddAsync(T item);
        void Delete(T item);
        void Update(T item);
    }
}
