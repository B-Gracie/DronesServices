using Drones.DAL;
using Drones.DAL.DronesDTO;
using Drones.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Drones.API.Controllers;

[ApiController]
[Route("api/medications")]
public class MedicationController : ControllerBase
{
    private readonly IMedicationService _medicationService;
    private readonly ILogger<MedicationController> _logger;

    public MedicationController(IMedicationService medicationService, ILogger<MedicationController> logger)
    {
        _medicationService = medicationService;
        _logger = logger;
    }
    
    [HttpGet("getloaded-medication")]
    public async Task<IActionResult> GetLoadedMedicationsAsync()
    {
        try
        {
            // Call the service method to get medications
            var medications = await _medicationService.GetLoadedMedications();

            return Ok(medications);
        }
        catch (Exception ex)
        {
            // Log the error (use a logging framework like Serilog or Microsoft.Extensions.Logging)
            return StatusCode(500, "An error occurred while processing the request.");
        }
    }
    

    [HttpGet("{medicationId}")]
    public async Task<ActionResult<Medication>> GetMedicationByIdAsync(int medicationId)
    {
        var medication = await _medicationService.GetMedicationByIdAsync(medicationId);

        if (medication == null)
        {
            return NotFound(); // Medication with the specified ID not found
        }

        return Ok(medication);
    }

    [HttpPost("{droneId}/medications")]
    public async Task<IActionResult> CreateMedicationAndLoadAsync(int droneId, [FromBody] MedicationDto medicationDto)
    {
        try
        {
            var createdMedication = await _medicationService.CreateMedicationAndLoadAsync(droneId, medicationDto);
            return Ok(createdMedication); // Return the newly created medication
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while creating medication and loading it onto the drone.");
            return StatusCode(500, "An error occurred while processing the request.");
        }
    }

}