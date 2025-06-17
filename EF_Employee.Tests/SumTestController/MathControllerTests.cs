using Xunit;
using Microsoft.AspNetCore.Mvc;
using MyWebApiProject.Controllers;

public class MathControllerTests
{
    [Fact]
    public void ReturnsCorrectResult()
    {
        // Arrange
        var controller = new MathController();

        // Act
        var result = controller.Sum(5, 7);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(12, okResult.Value);
    }
}
