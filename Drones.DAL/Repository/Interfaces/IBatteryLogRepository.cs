namespace Drones.DAL.Repository.Interfaces;

public interface IBatteryLogRepository
{
    Task LogBatteryLevelAsync(int droneId, int batteryLevel);
    Task<IEnumerable<BatteryLog>> GetBatteryLogsAsync(int droneId, int count);
}