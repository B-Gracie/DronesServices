using Drones.DAL.Repository.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Drones.Services.Implementation;

public class BatteryLevelCheckService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly TimeSpan _interval = TimeSpan.FromMinutes(15); // Adjust the interval as needed

    public BatteryLevelCheckService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            // Check battery levels and log events here
            using var scope = _serviceProvider.CreateScope();
            var batteryLogRepository = scope.ServiceProvider.GetRequiredService<IBatteryLogRepository>();
            var droneRepository = scope.ServiceProvider.GetRequiredService<IDroneRepository>();

            // Iterate through drones and check battery levels
            var drones = await droneRepository.GetAllDronesAsync();
            foreach (var drone in drones)
            {
                // Check battery level and log if below threshold
                if (drone.BatteryCapacity < 25)
                {
                    // Log the event in BatteryLogRepository
                    await batteryLogRepository.LogBatteryLevelAsync(drone.Id, drone.BatteryCapacity);
                }
            }

            // Sleep for the specified interval
            await Task.Delay(_interval, stoppingToken);
        }
    }
}