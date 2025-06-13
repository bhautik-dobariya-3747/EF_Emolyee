using MyWebApiProject.Models;
using System;

namespace Employee.Repository.Interface
{
    public interface IEmployeeRepository
    {
        bool IsEmailExists(string email, Guid currentGuid);
        EmployeeModel Add(EmployeeModel employee);
        EmployeeModel Update(EmployeeModel employee);
        bool Delete(Guid guid);
        EmployeeModel GetByGuid(Guid guid);
        void Delete(EmployeeModel employee);
    }
}