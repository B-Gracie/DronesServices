using Drones.DAL.DbContext;
using Drones.DAL.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Drones.DAL.Repository.Implementation;


public class BatteryLogRepository : IBatteryLogRepository
{
    private readonly DroneDbContext _dbContext;

    public BatteryLogRepository(DroneDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task LogBatteryLevelAsync(int droneId, int batteryLevel)
    {
        var batteryLog = new BatteryLog
        {
            DroneId = droneId,
            Timestamp = DateTime.UtcNow,
            BatteryLevel = batteryLevel
        };

        _dbContext.BatteryLogs.Add(batteryLog);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<BatteryLog>> GetBatteryLogsAsync(int droneId, int count)
    {
        return await _dbContext.BatteryLogs
            .Where(log => log.DroneId == droneId)
            .OrderByDescending(log => log.Timestamp)
            .Take(count)
            .ToListAsync();
    }
}
