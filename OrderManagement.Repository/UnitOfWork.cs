using Microsoft.EntityFrameworkCore;
using OrderManagement.Core;
using OrderManagement.Core.Entities;
using OrderManagement.Core.Repositories.Contracts;
using OrderManagement.Repository.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private  Hashtable _Repositories;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            _Repositories = new Hashtable();
        }
        public  async Task<int> CompleteAsync()
        {
           return await _context.SaveChangesAsync();
        }

        public async ValueTask DisposeAsync()
        {
             await _context.DisposeAsync();
        }

        public IGenericRepository<T> Repository<T>() where T : BaseEntity
        {
           var type =typeof(T).Name;
            if (!_Repositories.ContainsKey(type))
            {
                var repository = new GenericRepository<T>(_context);
                _Repositories.Add(type, repository);
            }
            return(IGenericRepository < T >)_Repositories[type];
        }
    }
}
