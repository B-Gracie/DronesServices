using AutoMapper;
using Drones.API.Test.ControllerTests;
using Drones.DAL;
using Drones.DAL.DronesDTO;
using Drones.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Drones.API.Controllers;

[ApiController]
[Route("api/drones")]
public class DroneController : ControllerBase
{
    private readonly IDroneService _droneService;
    private readonly IMedicationService _medicationService;
    private readonly IMapper _mapper;

    public DroneController(IDroneService droneService, IMapper mapper, IMedicationService medicationService)
    {
        _droneService = droneService;
        _mapper = mapper;
        _medicationService = medicationService;
    }
    

    [HttpGet ("getall")]
    public async Task<IActionResult> GetAllDrones()
    {
        try
        {
            var drones = await _droneService.GetAllDronesAsync();
            return Ok(drones);
        }
        catch (Exception ex)
        {
            return BadRequest($"Error: {ex.Message}");
        }
    }
    
    [HttpPost("create")]
    public async Task<IActionResult> CreateDroneAsync([FromBody] DronesDto droneDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var drone = MapDronesDtoToDrone(droneDto);

        try
        {
            var createdDrone = await _droneService.CreateDroneAsync(drone);
            return Ok(createdDrone);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
    private Drone MapDronesDtoToDrone(DronesDto droneDto)
    {
        // Map the DTO properties to the Drone model properties
        return new Drone
        {
            Id = droneDto.Id,
            SerialNumber = droneDto.SerialNumber,
            Model = droneDto.Model,
            WeightLimit = droneDto.WeightLimit,
            BatteryCapacity = droneDto.BatteryCapacity,
            State = Enum.Parse<DroneState>(droneDto.State) // Convert string state to enum
        };
    }
    
    
    [HttpGet("{droneId}")]
        public async Task<IActionResult> GetDroneById(int droneId)
        {
            try
            {
                var drone = await _droneService.GetDroneByIdAsync(droneId);
                if (drone == null)
                {
                    return NotFound($"Drone with ID {droneId} not found.");
                }
                return Ok(drone);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }
    
    

        [HttpPost("{droneId}/updateState")]
        public async Task<IActionResult> UpdateDroneState(int droneId, [FromBody] DroneState newState)
        {
            try
            {
                await _droneService.UpdateDroneStateAsync(droneId, newState);
                return Ok($"Drone {droneId} state updated to {newState}");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpPut("{droneId}/update-battery")]
        public async Task<IActionResult> UpdateDroneBatteryCapacityAsync(int droneId, [FromBody] int newBatteryCapacity)
        {
            try
            {
                await _droneService.UpdateDroneBatteryCapacityAsync(droneId, newBatteryCapacity);
                return Ok("Drone battery capacity updated successfully.");
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "An error occurred while updating the drone's battery capacity.");
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }


        [HttpGet("{droneId}/isAvailableForLoading")]
        public async Task<IActionResult> IsDroneAvailableForLoading(int droneId, double medicationWeight)
        {
                var isAvailable = await _droneService.IsDroneAvailableForLoadingAsync(droneId, medicationWeight);

                if (isAvailable)
                {
                    var response = new DroneAvailabilityResponse
                    {
                        Message = "Drone is available for loading.",
                        IsAvailable = true
                    };
                    return Ok(response);
                }

                var unavailableResponse = new DroneAvailabilityResponse
                {
                    Message = "Drone is not available for loading.",
                    IsAvailable = false
                };
                return Ok(unavailableResponse);
            }

        }