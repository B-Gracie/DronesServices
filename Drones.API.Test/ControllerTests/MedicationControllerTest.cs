using Drones.API.Controllers;
using Drones.DAL;
using Drones.DAL.DronesDTO;
using Drones.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace Drones.API.Test.ControllerTests;

public class MedicationControllerTests
{
    [Fact]
    public async Task GetLoadedMedicationsAsync_ReturnsOkResult()
    {
        // Arrange
        var medicationServiceMock = new Mock<IMedicationService>();
        var loggerMock = new Mock<ILogger<MedicationController>>();
        var controller = new MedicationController(medicationServiceMock.Object, loggerMock.Object);

        // Mock the service to return a list of medications
        var medications = new List<Medication>
        {
            new() { Id = 1, Name = "Medication A" },
            new() { Id = 2, Name = "Medication B" },
            new() { Id = 3, Name = "Medication C" }
        };
        medicationServiceMock.Setup(service => service.GetLoadedMedications()).ReturnsAsync(medications);
        
        // Act
        var result = await controller.GetLoadedMedicationsAsync();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var model = Assert.IsAssignableFrom<IEnumerable<Medication>>(okResult.Value);
        Assert.Equal(medications.Count, model.Count());
        
    }  
    
    
    [Fact]
    public async Task CreateMedicationAndLoadAsync_ReturnsOkResult_WhenMedicationIsCreated()
    {
        // Arrange
        int droneId = 1;
        var medicationDto = new MedicationDto(); 
        var createdMedication = new Medication { Id = 1 }; 

        var mockMedicationService = new Mock<IMedicationService>();
        mockMedicationService.Setup(service => service.CreateMedicationAndLoadAsync(droneId, medicationDto))
            .ReturnsAsync(createdMedication);

        var controller = new MedicationController(mockMedicationService.Object, Mock.Of<ILogger<MedicationController>>());

        // Act
        var result = await controller.CreateMedicationAndLoadAsync(droneId, medicationDto);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var medication = Assert.IsType<Medication>(okResult.Value);
        Assert.Equal(createdMedication.Id, medication.Id);
    }

}