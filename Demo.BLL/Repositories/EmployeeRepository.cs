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
    public class EmployeeRepository : GenericRepository<Employee>, IEmployeeRepository
    {

        public EmployeeRepository(ApplicationDbContext context) : base(context) // Ask CLR for creating Object From DbContext
        {
        }

        public IEnumerable<Employee> SearchByName(string name)
        {
            return _context.Employees.Where(E => E.Name.ToLower().Contains(name)).Include(E => E.Department);
        }
        //public int Add(Employee entity)
        //{
        //    _context.Employees.Add(entity);
        //    return _context.SaveChanges();
        //}

        //public int Delete(Employee entity)
        //{
        //    _context.Employees.Remove(entity);
        //    return _context.SaveChanges();
        //}

        //public Employee Get(int id)
        //{
        //    return _context.Employees.Find(id);
        //}

        //public IEnumerable<Employee> GetAll()
        //{
        //    return _context.Employees.ToList();
        //}

        //public int Update(Employee entity)
        //{
        //    _context.Employees.Update(entity);
        //    return _context.SaveChanges();
        //}
    }
}
