using Microsoft.EntityFrameworkCore;
using MyWebApiProject.Data;
using MyWebApiProject.Models;
using MyWebApiProject.Repositories;
using System;
using Xunit;

namespace MyWebApiProject.Tests.Repositories
{
    public class EmployeeRepositoryTests
    {
        private EmployeeDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<EmployeeDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // unique DB for each test
                .Options;

            var context = new EmployeeDbContext(options);
            context.Database.EnsureCreated();
            return context;
        }

        [Fact]
        public void AddEmployee()
        {
            // Arrange 
            var context = GetDbContext(); 
            var repo = new EmployeeRepository(context);    // Create a Instance of Repositoey 

            var employee = new EmployeeModel
            {
                Guid = Guid.NewGuid(),
                Name = "John Doe",
                Email = "john@example.com",
                Address = "Test Address",
                Age = 30,
                Department = "IT",
                Salary = 10000,
                IsActive = true,
                CreatedBy = 1
            };

            // Act 
            var result = repo.Add(employee);

            // Assert
            Assert.Equal("John Doe", result.Name);
   
        }

        [Fact]
        public void GetByGuid_ExistingGuid_ReturnsEmployee()
        {
            // Arrange 
            var context = GetDbContext();
            var repo = new EmployeeRepository(context);
            var employee = new EmployeeModel
            {
                Guid = Guid.NewGuid(),
                Name = "Jane",
                Email = "jane@example.com",
                Address = "123 Street",
                Age = 28,
                Department = "HR",
                Salary = 5000,
                IsActive = true
            };

            context.Employees.Add(employee);
            context.SaveChanges();

            // Act 
            var result = repo.GetByGuid(employee.Guid);

            // Assert
            Assert.Equal("Jane", result.Name);
        }

        [Fact]
        public void Update_ShouldUpdateEmployee()
        {
            // Arrange 
            var context = GetDbContext();
            var repo = new EmployeeRepository(context);
            var employee = new EmployeeModel
            {
                Guid = Guid.NewGuid(),
                Name = "Old Name",
                Email = "old@example.com",
                Address = "Old Address",
                Age = 35,
                Department = "Finance",
                Salary = 7500,
                IsActive = true
            };

            context.Employees.Add(employee);
            context.SaveChanges();

            // Act 
            employee.Name = "Updated Name";
            var result = repo.Update(employee);

            // Assert
            Assert.Equal("Updated Name", result.Name);
        }

        [Fact]
        public void Delete_ExistingEmployee_ReturnsTrue()
        {
            // Arrange 
            var context = GetDbContext();
            var repo = new EmployeeRepository(context);
            var guid = Guid.NewGuid();
            var employee = new EmployeeModel
            {
                Guid = guid,
                Name = "ToDelete",
                Email = "del@example.com",
                Address = "Del Address",
                Age = 29,
                Department = "Support",
                Salary = 4000,
                IsActive = true
            };

            context.Employees.Add(employee);
            context.SaveChanges();

            // Act 
            var result = repo.Delete(guid);

            // Assert
            Assert.True(result);
            Assert.Null(context.Employees.Find(guid));
        }

        [Fact]
        public void Delete_NonExistingEmployee_ReturnsFalse()
        {
            // Arrange 
            var context = GetDbContext();
            var repo = new EmployeeRepository(context);

            // Act 
            var result = repo.Delete(Guid.NewGuid());

            // Assert
            Assert.False(result);
        }

        //[Fact]
        //public void IsEmailExists_WithSameEmailDifferentGuid_ReturnsTrue()
        //{
        //    // Arrange 
        //    var context = GetDbContext();
        //    var repo = new EmployeeRepository(context);
        //    var guid1 = Guid.NewGuid();
        //    var employee = new EmployeeModel
        //    {
        //        Guid = guid1,
        //        Name = "Test",
        //        Email = "same@example.com",
        //        Address = "Test Address",
        //        Age = 30,
        //        Department = "Testing",
        //        Salary = 6000,
        //        IsActive = true
        //    };

        //    context.Employees.Add(employee);
        //    context.SaveChanges();

        //    // Act 
        //    var result = repo.IsEmailExists("same@example.com", Guid.NewGuid());

        //    // Assert
        //    Assert.True(result);
        //}

        [Fact]
        public void IsEmailExists_WithSameGuid_ReturnsFalse()
        {
            // Arrange 
            var context = GetDbContext();
            var repo = new EmployeeRepository(context);
            var guid = Guid.NewGuid();
            var employee = new EmployeeModel
            {
                Guid = guid,
                Name = "Test",
                Email = "unique@example.com",
                Address = "Unique Address",
                Age = 32,
                Department = "Marketing",
                Salary = 8500,
                IsActive = true
            };

            context.Employees.Add(employee);
            context.SaveChanges();

            // Act 
            var result = repo.IsEmailExists("unique@example.com", guid);

            // Assert
            Assert.False(result);
        }
    }
}
