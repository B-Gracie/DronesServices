using AutoMapper;
using Drones.API.Controllers;
using Drones.DAL;
using Drones.DAL.DronesDTO;
using Drones.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Drones.API.Test.ControllerTests;

namespace Drones.API.Test;

public class DroneControllerTest
{
    private readonly Mock<IDroneService> _mockDroneService;
    private readonly Mock<IMapper> _mockMapper;
    private readonly DroneController _dronesController;

    public DroneControllerTest()
    {
        _mockDroneService = new Mock<IDroneService>();
        _mockMapper = new Mock<IMapper>();
        _dronesController = new DroneController(_mockDroneService.Object, _mockMapper.Object, null);
    }

    [Fact]
    public async Task CreateDroneAsync_ValidDto_ReturnsOkResult()
    {
        // Arrange
        var droneDto = new DronesDto
        {
            // Initialize with valid DTO data
            Id = 101,
            SerialNumber = "X101",
            WeightLimit = 500.0,
            Model = "Middleweight",
            BatteryCapacity = 80,
            State = "IDLE"
        };

        _mockDroneService.Setup(service => service.CreateDroneAsync(It.IsAny<Drone>()))
            .ReturnsAsync(new Drone());

        // Act
        var result = await _dronesController.CreateDroneAsync(droneDto);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.NotNull(okResult);
    }

    [Fact]
    public async Task GetAllDrones_ReturnsOkResultWithDrones()
    {
        // Arrange
        _mockDroneService.Setup(service => service.GetAllDronesAsync())
            .ReturnsAsync(new List<Drone>()); // Mock the service method to return a list of Drones

        // Act
        var result = await _dronesController.GetAllDrones();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.NotNull(okResult);
        var drones = Assert.IsType<List<Drone>>(okResult.Value);
        // You can add more assertions based on your specific implementation
    }

    [Fact]
    public async Task GetDroneById_ExistingDrone_ReturnsOkResult()
    {
        // Arrange
        var existingDrone = new Drone { Id = 101, SerialNumber = "X101" };
        _mockDroneService.Setup(service => service.GetDroneByIdAsync(101))
            .ReturnsAsync(existingDrone);

        // Act
        var result = await _dronesController.GetDroneById(101);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var drone = Assert.IsType<Drone>(okResult.Value);
        Assert.Equal(existingDrone.Id, drone.Id);
        Assert.Equal(existingDrone.SerialNumber, drone.SerialNumber);
    }

    [Fact]
    public async Task GetDroneById_NonExistingDrone_ReturnsNotFound()
    {
        // Arrange
        _mockDroneService.Setup(service => service.GetDroneByIdAsync(1))
            .ReturnsAsync((Drone)null);

        // Act
        var result = await _dronesController.GetDroneById(1);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal("Drone with ID 1 not found.", notFoundResult.Value);
    }

    [Fact]
    public async Task UpdateDroneState_ValidData_ReturnsOkResult()
    {
        // Arrange
        var newState = DroneState.LOADING;
        _mockDroneService.Setup(service => service.UpdateDroneStateAsync(101, newState))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _dronesController.UpdateDroneState(101, newState);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal($"Drone 101 state updated to {newState}", okResult.Value);
    }

    [Fact]
    public async Task UpdateDroneBatteryCapacity_ValidData_ReturnsOkResult()
    {
        // Arrange
        var newBatteryCapacity = 90;
        _mockDroneService.Setup(service => service.UpdateDroneBatteryCapacityAsync(101, newBatteryCapacity))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _dronesController.UpdateDroneBatteryCapacityAsync(101, newBatteryCapacity);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal("Drone battery capacity updated successfully.", okResult.Value);
    }

    [Fact]
    public async Task IsDroneAvailableForLoading_Available_ReturnsOkResult()
    {
        // Arrange
        var mockDroneService = new Mock<IDroneService>();
        mockDroneService.Setup(service => service.IsDroneAvailableForLoadingAsync(It.IsAny<int>(), It.IsAny<double>()))
            .ReturnsAsync(true); // Mock the service method to return true for availability

        var controller = new DroneController(mockDroneService.Object, null, null);

        // Act
        var result = await controller.IsDroneAvailableForLoading(1, 500.0); // Provide valid droneId and medicationWeight
       
        
        //Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsType<DroneAvailabilityResponse>(okResult.Value);
        
        Assert.Equal("Drone is available for loading.", response.Message);
        Assert.True(response.IsAvailable);

    }

    [Fact]
    public async Task IsDroneAvailableForLoading_NotAvailable_ReturnsOkResult()
    {
        // Arrange
        var droneId = 1;
        var medicationWeight = 200.0;
        _mockDroneService.Setup(service => service.IsDroneAvailableForLoadingAsync(droneId, medicationWeight))
            .ReturnsAsync(false);

        // Act
        var result = await _dronesController.IsDroneAvailableForLoading(droneId, medicationWeight);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsType<DroneAvailabilityResponse>(okResult.Value);
        Assert.Equal("Drone is not available for loading.", response.Message);
        Assert.False(response.IsAvailable);
    }

}