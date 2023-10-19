using Demo.BLL.Interfaces;
using Demo.DAL.Data.Contexts;
using Demo.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Repositories
{
    public class DepartmentRepository : GenericRepository<Department>, IDepartmentRepository
    {

        public DepartmentRepository(ApplicationDbContext context) : base(context) //Ask CLR for Creating object of DbContext 
        {
           
        }


        //public int Add(Department entity)
        //{
        //    _context.Departments.Add(entity);
        //    return _context.SaveChanges();
        //}

        //public int Delete(Department entity)
        //{
        //    _context.Departments.Remove(entity);
        //    return _context.SaveChanges();
        //}

        //public int Update(Department entity)
        //{
        //    _context.Departments.Update(entity);
        //    return _context.SaveChanges();
        //}
        //public Department Get(int id)
        //{
        //    return _context.Departments.Find(id);
        //}

        //public IEnumerable<Department> GetAll()
        //{
        //    return _context.Departments.ToList();
        //}

    }
}
