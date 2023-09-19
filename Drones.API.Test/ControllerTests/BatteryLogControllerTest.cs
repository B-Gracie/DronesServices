using Drones.API.Controllers;
using Drones.API.ViewModel;
using Drones.DAL;
using Drones.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Drones.API.Test.ControllerTests;

public class BatteryLogControllerTests
{
    private readonly BatteryLogController _controller;

    public BatteryLogControllerTests()
    {
        // Create a mock for IBatteryLogService
        var batteryLogServiceMock = new Mock<IBatteryLogService>();

        // Initialize the controller with the mock service
        _controller = new BatteryLogController(batteryLogServiceMock.Object);
    }

    [Fact]
    public async Task LogBatteryLevelAsync_ReturnsOkResult()
    {
        // Arrange
        var model = new BatteryLogRequestModel
        {
            DroneId = 1,
            BatteryLevel = 90
        };

        // Act
        var result = await _controller.LogBatteryLevelAsync(model);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal("Battery level logged successfully.", okResult.Value);
    }
    [Fact]
    public async Task GetBatteryLogsAsync_ReturnsOkResult()
    {
        // Arrange
        var droneId = 1;
        var count = 5;
        var batteryLogs = new List<BatteryLog>(); // Your test data here

        // Mock the service to return battery logs
        var batteryLogServiceMock = new Mock<IBatteryLogService>();
        batteryLogServiceMock
            .Setup(service => service.GetBatteryLogsAsync(droneId, count))
            .ReturnsAsync(batteryLogs);

        var controller = new BatteryLogController(batteryLogServiceMock.Object);

        // Act
        var result = await controller.GetBatteryLogsAsync(droneId, count);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var model = Assert.IsAssignableFrom<IEnumerable<BatteryLog>>(okResult.Value);
        // Add more assertions here based on your expected behavior
    }

   }