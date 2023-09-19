using AutoMapper;
using Drones.DAL.DbContext;
using Drones.DAL.DronesDTO;
using Drones.DAL.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Drones.DAL.Repository.Implementation;


public class DroneRepository : IDroneRepository
{
    private readonly DroneDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly ILogger<DroneRepository> _logger;

    public DroneRepository(DroneDbContext dbContext, IMapper mapper, ILogger<DroneRepository> logger)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<Drone> CreateDroneAsync(Drone drone)
    {
        // You should add validation and error handling here if necessary
        if (drone == null)
        {
            throw new ArgumentNullException(nameof(drone));
        }

        try
        {
            // Add the drone entity to the database
            _dbContext.Drones.Add(drone);
            await _dbContext.SaveChangesAsync();

            return drone; // Return the created drone entity with its ID populated
        }
        catch (Exception ex)
        {
            // Handle database-related exceptions or other errors here
            // You may log the error or rethrow it as needed
            throw new Exception("Error creating the drone.", ex);
        }
    }


    public async Task<IEnumerable<Drone>> GetAllDronesAsync()
    {
        return await _dbContext.Drones.ToListAsync();
    }

    public async Task<Drone>  GetDroneByIdAsync(int droneId)
    {
        return await _dbContext.Drones.FindAsync(droneId);
    }
    

    public async Task<bool> IsDroneAvailableForLoadingAsync(int droneId, double medicationWeight)
    {
        var drone = await _dbContext.Drones.FindAsync(droneId);

        if (drone == null)
        {
            return false; 
        }

        if (drone.State != DroneState.IDLE)
        {
            return false;
        }

        // Check battery level
        if (drone.BatteryCapacity < 25)
        {
            return false; 
        }
        
        if (medicationWeight > drone.WeightLimit)
        {
            return false; 
        }

        return true; 
    }





    public async Task UpdateDroneBatteryCapacityAsync(int droneId, int newBatteryCapacity)
    {
        var drone = await _dbContext.Drones.FindAsync(droneId);

        if (drone != null)
        {
            drone.BatteryCapacity = newBatteryCapacity;
            await _dbContext.SaveChangesAsync();
        }
    }
    
    
    public async Task UpdateDroneAsync(Drone drone)
    {

        _dbContext.Entry(drone).State = EntityState.Modified;
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateDroneStateAsync(int droneId, DroneState newState)
    {
        var drone = await _dbContext.Drones.FindAsync(droneId);
        if (drone != null)
        {
            if (newState == DroneState.LOADING)
            {
                // Check the battery level before allowing the LOADING state
                if (drone.BatteryCapacity < 25)
                {
                    throw new Exception("Drone cannot enter LOADING state due to low battery level.");
                }
            }

            drone.State = newState;
            _dbContext.Entry(drone).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }
    }
    
    public async Task LoadMedicationsAsync(int droneId, IEnumerable<int> medicationIds)
    {
        try
        {
            var drone = await _dbContext.Drones.FindAsync(droneId);

            if (drone != null)
            {
                // Create a list to store the loaded medications
                var loadedMedications = new List<LoadedMedication>();

                foreach (var medicationId in medicationIds)
                {
                    var medication = await _dbContext.Medications.FindAsync(medicationId);

                    if (medication != null)
                    {
                        // Set the DroneId property for each medication
                        medication.DroneId = droneId;

                        // Create a new LoadedMedication instance and add it to the collection
                        var loadedMedication = new LoadedMedication
                        {
                            DroneId = droneId,
                            MedicationId = medicationId
                        };
                        loadedMedications.Add(loadedMedication);
                    }
                }

                // Add the loaded medications to the drone
                drone.LoadedMedications.AddRange(loadedMedications);

                // Mark the drone entity as modified
                _dbContext.Entry(drone).State = EntityState.Modified;

                // Save changes to update both the drone and medications
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                // Handle the case where the drone is not found
                _logger.LogError("Drone with ID {DroneId} not found.", droneId);
            }
        }
        catch (Exception ex)
        {
            // Log the exception for debugging purposes
            _logger.LogError(ex, "An error occurred while loading medications.");
        }
    }


}