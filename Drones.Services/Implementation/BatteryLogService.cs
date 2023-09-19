using Drones.DAL;
using Drones.DAL.Repository.Interfaces;
using Drones.Services.Interfaces;

namespace Drones.Services.Implementation;

    public class BatteryLogService : IBatteryLogService
    {
        private readonly IBatteryLogRepository _batteryLogRepository;

        public BatteryLogService(IBatteryLogRepository batteryLogRepository)
        {
            _batteryLogRepository = batteryLogRepository;
        }

        public async Task LogBatteryLevelAsync(int droneId, int batteryLevel)
        {
            await _batteryLogRepository.LogBatteryLevelAsync(droneId, batteryLevel);
        }

        public async Task<IEnumerable<BatteryLog>> GetBatteryLogsAsync(int droneId, int count)
        {
            return await _batteryLogRepository.GetBatteryLogsAsync(droneId, count);
        }
    }

