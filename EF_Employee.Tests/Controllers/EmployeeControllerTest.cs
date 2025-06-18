using Employee.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using MyWebApiProject.Controllers;
using MyWebApiProject.Models;
using System;
using System.Collections.Generic;
using Xunit;

namespace MyWebApiProject.Tests
{
    public class EmployeeControllerTests
    {
        private readonly Mock<IEmployeeService> _mockService;
        private readonly EmployeeController _controller;

        public EmployeeControllerTests()
        {
            _mockService = new Mock<IEmployeeService>();
            _controller = new EmployeeController(_mockService.Object);
        }

        [Fact]
        public void Create_ValidEmployee_ReturnsOk()
        {
            // Arrange
            var employee = new EmployeeModel
            {
                Guid = Guid.NewGuid(),
                Name = "John",
                Email = "john@example.com",
                Address = "123 Street",
                Age = 30,
                Department = "IT",
                Salary = 50000,
                IsActive = true
            };

            _mockService.Setup(s => s.IsEmailExists(employee.Email, It.IsAny<Guid>())).Returns(false);
            _mockService.Setup(s => s.Create(employee)).Returns(employee);

            // Act
            var result = _controller.Create(employee);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = okResult.Value as IDictionary<string, object>;
            Assert.Equal("Employee created successfully.", response["message"]);
        }

        [Fact]
        public void Create_InvalidModel_ReturnsBadRequest()
        {
            // Arrange
            _controller.ModelState.AddModelError("Name", "Required");

            var invalidEmployee = new EmployeeModel
            {
                Guid = Guid.NewGuid(),
                Name = "KANJI",
                Address = "GOKUL",
                Age = 25,
                Department = "OPEARTOR",
                Salary = 40000000000,
                IsActive = true,
                Email = "kanji@gmail.com"
            };

            // Act
            var result = _controller.Create(invalidEmployee);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.IsType<SerializableError>(badRequestResult.Value);
        }

        [Fact]
        public void Create_DuplicateEmail_ReturnsBadRequest()
        {
            // Arrange
            var employee = new EmployeeModel
            {
                Guid = Guid.NewGuid(),
                Name = "Mary",
                Email = "duplicate@example.com",
                Address = "456 Avenue",
                Age = 28,
                Department = "HR",
                Salary = 40000m,
                IsActive = true
            };

            _mockService.Setup(s => s.IsEmailExists(employee.Email, null)).Returns(true);

            // Act
            var result = _controller.Create(employee);

            // Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Email address already exists.", badRequest.Value);
        }

        [Fact]
        public void Update_ValidEmployee_ReturnsOk()
        {
            // Arrange
            var employee = new EmployeeModel    
            {
                Email = "shah@example.com",
                Guid = Guid.NewGuid(),
                Name = "jay",
                Address = "pok",
                Age = 30
            };

            _mockService.Setup(s => s.IsEmailExists(employee.Email, null)).Returns(false);
            _mockService.Setup(s => s.Update(employee)).Returns(employee);


            // Act
            var result = _controller.Update(employee);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
        }

        [Fact]
        public void Update_EmailExists_ReturnsBadRequest()
        {
            // Arrange
            var employee = new EmployeeModel { Email = "duplicate@example.com", Guid = Guid.NewGuid() };
            _mockService.Setup(s => s.IsEmailExists(employee.Email, employee.Guid)).Returns(true);

            // Act
            var result = _controller.Update(employee);

            // Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Email already exists for another employee.", badRequest.Value);
        }

        [Fact]
        public void Update_InvalidModelState_ReturnsBadRequest()
        {
            // Arrange
            _controller.ModelState.AddModelError("Name", "Name is required.");

            var employee = new EmployeeModel(); // Missing fields

            // Act
            var result = _controller.Update(employee);

            // Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequest.StatusCode);
        }

        [Fact]
        public void Update_EmployeeNotFound_ReturnsNotFound()
        {
            // Arrange
            var employee = new EmployeeModel
            {
                Guid = Guid.NewGuid(),
                Name = "Ghost",
                Address = "Nowhere",
                Age = 99,
                Department = "Unknown",
                Salary = 0,
                IsActive = false,
                Email = "ghost@example.com"
            };

            _mockService.Setup(s => s.IsEmailExists(employee.Email, employee.Guid)).Returns(false);
            _mockService.Setup(s => s.Update(employee)).Returns((EmployeeModel)null); // not found

            // Act
            var result = _controller.Update(employee);

            // Assert
            var notFound = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Employee not found.", notFound.Value);
        }

        [Fact]
        public void Delete_ExistingGuid_ReturnsOk()
        {
            // Arrange
            var guid = Guid.NewGuid();
            _mockService.Setup(s => s.Delete(guid)).Returns(true);

            // Act
            var result = _controller.Delete(guid);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
        }

        [Fact]
        public void Delete_NonExistingGuid_ReturnsNotFound()
        {
            // Arrange
            var guid = Guid.NewGuid();
            _mockService.Setup(s => s.Delete(guid)).Returns(false);

            // Act
            var result = _controller.Delete(guid);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Employee not found.", notFoundResult.Value);
        }

        [Fact]
        public void Get_ExistingEmployee_ReturnsOk()
        {
            // Arrange
            var guid = Guid.NewGuid();
            var employee = new EmployeeModel { Guid = guid, Name = "Test" };
            _mockService.Setup(s => s.Get(guid)).Returns(employee);

            // Act
            var result = _controller.Get(guid);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(employee, okResult.Value);
        }

        [Fact]
        public void Get_NonExistingEmployee_ReturnsNotFound()
        {
            // Arrange
            var guid = Guid.NewGuid();
            _mockService.Setup(s => s.Get(guid)).Returns((EmployeeModel?)null);

            // Act
            var result = _controller.Get(guid);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Employee not found.", notFoundResult.Value);
        }
    }
}
