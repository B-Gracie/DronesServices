using Drones.DAL.DronesDTO;

namespace Drones.DAL.Repository.Interfaces;

public interface IDroneRepository
{
    
        Task<Drone> CreateDroneAsync(Drone drone);
        Task<bool> IsDroneAvailableForLoadingAsync(int droneId, double medicationWeight);
        Task UpdateDroneBatteryCapacityAsync(int droneId, int newBatteryCapacity);

        Task UpdateDroneAsync(Drone drone);
        Task<IEnumerable<Drone>> GetAllDronesAsync();
        Task<Drone> GetDroneByIdAsync(int droneId);
        Task UpdateDroneStateAsync(int droneId, DroneState newState);
        Task LoadMedicationsAsync(int droneId, IEnumerable<int> medicationIds);

}