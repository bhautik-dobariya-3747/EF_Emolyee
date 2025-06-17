using Employee.Repository.Interface;
using Employee.Service.Interface;
using MyWebApiProject.Models;
using System;
    
namespace Employee.Service
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _repository;

        public EmployeeService(IEmployeeRepository repository)
        {
            _repository = repository;
        }

        public EmployeeModel Create(EmployeeModel employee)
        {       
            if (employee.Guid == Guid.Empty)
                employee.Guid = Guid.NewGuid();

            if (employee.CreatedBy == 0)
                employee.CreatedBy = 1;

            var createdEmployee = _repository.Add(employee);
            return createdEmployee;
        }   

        public EmployeeModel Update(EmployeeModel employee)
        {
            var existing = _repository.GetByGuid(employee.Guid);
            if (existing == null)
            return null;

            existing.Name = employee.Name;
            existing.Address = employee.Address;
            existing.Age = employee.Age;
            existing.Department = employee.Department;
            existing.Salary = employee.Salary;
            existing.Email = employee.Email;
            existing.IsActive = employee.IsActive;
            existing.ModifiedBy = employee.ModifiedBy == null || employee.ModifiedBy == 0 ? 2 : employee.ModifiedBy;
            existing.ModifiedDate = DateTime.Now;

            return _repository.Update(existing);
        }

        public bool Delete(Guid guid)
        {
            return _repository.Delete(guid);
        }

        public EmployeeModel Get(Guid guid)
        {
            return _repository.GetByGuid(guid);
        }

        public bool IsEmailExists(string email, Guid? currentGuid = null)
        {
            return _repository.IsEmailExists(email, currentGuid ?? Guid.Empty);
        }

    }
}
