using Drones.DAL;
using Drones.DAL.DronesDTO;

namespace Drones.Services.Interfaces;

public interface IDroneService
{
    //Task<DronesDto> CreateDroneAsync(DronesDto droneDto);
    Task<Drone> CreateDroneAsync(Drone drone);
    Task UpdateDroneBatteryCapacityAsync(int droneId, int newBatteryCapacity);
    Task<IEnumerable<Drone>> GetAllDronesAsync();
    Task<Drone> GetDroneByIdAsync(int droneId);
    Task<bool> IsDroneAvailableForLoadingAsync(int droneId, double medicationWeight);
    //Task<bool> CanDroneBeLoadedAsync(int droneId, double totalWeight);
    Task UpdateDroneStateAsync(int droneId, DroneState newState);
    //Task LoadMedicationsAsync(int droneId, List<int> medicationIds);
    Task LoadMedicationsAsync(int droneId, IEnumerable<int> medicationIds);
    
}