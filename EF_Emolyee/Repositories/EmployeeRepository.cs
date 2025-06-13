using Employee.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using MyWebApiProject.Data;
using MyWebApiProject.Models;
using System;
using System.Linq;

namespace MyWebApiProject.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly EmployeeDbContext _context;

        public EmployeeRepository(EmployeeDbContext context)
        {
            _context = context;
        }

        public bool IsEmailExists(string email, Guid currentGuid)
        {
            return _context.Employees
                .Any(e => e.Email == email && e.Guid != currentGuid);
        }
                
        public EmployeeModel Add(EmployeeModel employee)
        {
            try
            {
                // Access Employee table on database
                _context.Employees.Add(employee);
                _context.SaveChanges();
                return employee;
            }
            catch (DbUpdateException ex)
            {
                throw new Exception("Error saving employee: " + ex.InnerException?.Message, ex);
            }
        }

        public EmployeeModel Update(EmployeeModel employee)
        {
            _context.Employees.Update(employee);
            _context.SaveChanges();
            return employee;
        }   


        public bool Delete(Guid guid)
        {
            var employee = _context.Employees.Find(guid);
            if (employee == null) return false;

            _context.Employees.Remove(employee);
            return _context.SaveChanges() > 0;
        }

        public EmployeeModel GetByGuid(Guid guid)
        {
            return _context.Employees.Find(guid);
        }

        public void Delete(EmployeeModel employee)
        {
            throw new NotImplementedException();
        }
    }
}
