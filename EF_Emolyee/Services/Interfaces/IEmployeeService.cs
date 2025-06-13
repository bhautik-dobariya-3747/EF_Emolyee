using MyWebApiProject.Models;
using System;

namespace Employee.Service.Interface
{
    public interface IEmployeeService
    {
        EmployeeModel Create(EmployeeModel employee);
        EmployeeModel Update(EmployeeModel employee);
        bool Delete(Guid guid);
        EmployeeModel Get(Guid guid);
        bool IsEmailExists(string email, Guid? currentGuid = null);
    }
}