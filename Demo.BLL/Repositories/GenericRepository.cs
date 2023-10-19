using Demo.BLL.Interfaces;
using Demo.DAL.Data.Contexts;
using Demo.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T: BaseEntity
    {
        private protected readonly ApplicationDbContext _context;

        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public void Add(T entity)
        {
            //_context.Set<T>().Add(entity);
            _context.Add(entity); // EF Core 3.1 Feature
        }

        public void Delete(T entity)
        {
            //_context.Set<T>().Remove(entity);
            _context.Remove(entity); // EF Core 3.1 Feature
        }

        public async Task<T> Get(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            if(typeof(T) == typeof(Employee))
            {
                return (IEnumerable<T>) await _context.Employees.Include(E => E.Department).AsNoTracking().ToListAsync();
            }
            else
            {
                return await _context.Set<T>().AsNoTracking().ToListAsync();
            }
        }

        public void Update(T entity)
        {
            //_context.Set<T>().Update(entity);
            _context.Update(entity); // EF Core 3.1 Feature
        }
    }
}
