using Drones.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Drones.API.ViewModel;

namespace Drones.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BatteryLogController : ControllerBase
{
    private readonly IBatteryLogService _batteryLogService;

    public BatteryLogController(IBatteryLogService batteryLogService)
    {
        _batteryLogService = batteryLogService;
    }

    [HttpPost("log")]
    public async Task<IActionResult> LogBatteryLevelAsync([FromBody] BatteryLogRequestModel model)
    {
        try
        {
            // Call the service method to log the battery level
            await _batteryLogService.LogBatteryLevelAsync(model.DroneId, model.BatteryLevel);

            // Return a successful response
            return Ok("Battery level logged successfully.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred: {ex.Message}");
        }
    }

    [HttpGet("{droneId}/logs")]
    public async Task<IActionResult> GetBatteryLogsAsync(int droneId, int count)
    {
        try
        {
            
            var batteryLogs = await _batteryLogService.GetBatteryLogsAsync(droneId, count);
            
            return Ok(batteryLogs);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred: {ex.Message}");
        }
    }
}