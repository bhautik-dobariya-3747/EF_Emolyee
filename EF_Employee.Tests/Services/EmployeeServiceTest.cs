using Employee.Repository.Interface;
using Employee.Service;
using MyWebApiProject.Models;
using Moq;
using System;
using Xunit;

public class EmployeeServiceTests
{
    private readonly Mock<IEmployeeRepository> _mockRepo;
    private readonly EmployeeService _service;

    public EmployeeServiceTests()
    {
        _mockRepo = new Mock<IEmployeeRepository>();
        _service = new EmployeeService(_mockRepo.Object);
    }

    [Fact]
    public void Create_ReturnsEmployee()
    {
        // Arrange 
        var employee = new EmployeeModel();
        _mockRepo.Setup(r => r.Add(It.IsAny<EmployeeModel>())).Returns(employee);  // When the Add() method is called with any EmployeeModel object, return someEmployee.

        // Act
        var result = _service.Create(employee);

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public void Create_AssignsGuidAndCreatedBy_WhenMissing()
    {
        // Arrange
        var employee = new EmployeeModel
        {
            Guid = Guid.Empty,         // Should be auto-assigned
            CreatedBy = 0,             // Should default to 1
            Name = "Jane Doe",
            Address = "Somewhere",
            Age = 28,
            Department = "Finance",
            Salary = 75000,
            Email = "jane.doe@example.com",
            IsActive = true
        };

        _mockRepo.Setup(r => r.Add(It.IsAny<EmployeeModel>()))
                 .Returns((EmployeeModel e) => e); // Return the employee that was passed in

        // Act
        var result = _service.Create(employee);

        // Assert
        Assert.NotEqual(Guid.Empty, result.Guid);     // Guid should be assigned
        Assert.Equal(1, result.CreatedBy);            // CreatedBy should be set to 1
    }

    [Fact]
    public void Update_ReturnsUpdatedEmployee()
    {
        // Arrange 
        var guid = Guid.NewGuid();
        var employee = new EmployeeModel { Guid = guid };
        _mockRepo.Setup(r => r.GetByGuid(guid)).Returns(new EmployeeModel());  // “If someone calls GetByGuid() with this exact guid, return a new blank EmployeeModel.”
        _mockRepo.Setup(r => r.Update(It.IsAny<EmployeeModel>())).Returns(employee); // “If Update() is called with any EmployeeModel, just return this employee object.”

        // Act
        var result = _service.Update(employee);

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public void Update_EmployeeNotFound_ReturnsNull()
    {
        // Arrange
        var employee = new EmployeeModel { Guid = Guid.NewGuid() };

        _mockRepo.Setup(r => r.GetByGuid(employee.Guid)).Returns((EmployeeModel)null);

        // Act
        var result = _service.Update(employee);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void Delete_ReturnsTrue()
    {
        // Arrange 
        var guid = Guid.NewGuid();
        _mockRepo.Setup(r => r.Delete(guid)).Returns(true);

        // Act
        var result = _service.Delete(guid);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Get_ReturnsEmployee()
    {
        // Arrange 
        var guid = Guid.NewGuid();
        _mockRepo.Setup(r => r.GetByGuid(guid)).Returns(new EmployeeModel { Guid = guid });

        // Act
        var result = _service.Get(guid);

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public void IsEmailExists_ReturnsTrue()
    {
        // Arrange 
        _mockRepo.Setup(r => r.IsEmailExists("test@email.com", It.IsAny<Guid>())).Returns(true);

        // Act
        var result = _service.IsEmailExists("test@email.com");

        // Assert
        Assert.True(result);
    }
}


