using Drones.DAL;

namespace Drones.Services.Interfaces;

public interface IBatteryLogService
{
    Task LogBatteryLevelAsync(int droneId, int batteryLevel);
    Task<IEnumerable<BatteryLog>> GetBatteryLogsAsync(int droneId, int count);
}
