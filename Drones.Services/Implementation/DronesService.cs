using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Drones.DAL;
using Drones.DAL.DronesDTO;
using Drones.DAL.Repository.Interfaces;
using Drones.Services.Interfaces;

namespace Drones.Services.Implementation;

    public class DronesService : IDroneService
    {
        private readonly IDroneRepository _droneRepository;
        private readonly IMedicationRepository _medicationRepository;
        private readonly IMapper _mapper;

        public DronesService(IDroneRepository droneRepository, IMedicationRepository medicationRepository, IMapper mapper)
        {
            _droneRepository = droneRepository;
            _medicationRepository = medicationRepository;
            _mapper = mapper;
        }
        
        
        public async Task<Drone> CreateDroneAsync(Drone drone)
        {
            try
            {
                // Perform any business logic validation or processing here
                // Example: Check if the state is valid, perform other validations
                if (string.IsNullOrWhiteSpace(drone.SerialNumber))
                {
                    throw new ArgumentException("Serial number is required.", nameof(drone.SerialNumber));
                }

                // You can perform additional business logic checks here

                // Call the repository to create the drone
                var createdDrone = await _droneRepository.CreateDroneAsync(drone);

                return createdDrone;
            }
            catch (ArgumentException ex)
            {
                // Handle validation errors and provide a meaningful error response
                throw new ValidationException(ex.Message);
            }
            catch (Exception ex)
            {
                // Handle other exceptions and provide a meaningful error response
                throw new Exception("Error creating the drone. Please try again later.", ex);
            }
        }
        

        public async Task<IEnumerable<Drone>> GetAllDronesAsync()
        {
            return await _droneRepository.GetAllDronesAsync();
        }

        public async Task<Drone> GetDroneByIdAsync(int droneId)
        {
            return await _droneRepository.GetDroneByIdAsync(droneId);
        }
        
        public async Task UpdateDroneBatteryCapacityAsync(int droneId, int newBatteryCapacity)
        {
            await _droneRepository.UpdateDroneBatteryCapacityAsync(droneId, newBatteryCapacity);
        }


        public async Task<bool> IsDroneAvailableForLoadingAsync(int droneId, double medicationWeight)
        {
            var drone = await _droneRepository.GetDroneByIdAsync(droneId);

            if (drone == null)
            {
                return false; // Drone not found
            }

            if (drone.State != DroneState.IDLE)
            {
                return false; // Drone is not in IDLE state
            }

            // Check battery level
            if (drone.BatteryCapacity < 25)
            {
                return false; // Battery level is less than 25%
            }
            if (medicationWeight > drone.WeightLimit)
            {
                return false; // Medication weight exceeds the drone's capacity
            }

            return true; 
        }


        public async Task UpdateDroneStateAsync(int droneId, DroneState newState)
        {
            await _droneRepository.UpdateDroneStateAsync(droneId, newState);
        }

        public async Task LoadMedicationsAsync(int droneId, IEnumerable<int> medicationIds)
        {
            try
            {
                // Call the repository method to load medications onto the drone
                await _droneRepository.LoadMedicationsAsync(droneId, medicationIds);
            }
            catch (Exception ex)
            {
                // Handle exceptions or rethrow as needed
                throw new Exception("Error loading medications onto the drone.", ex);
            }
        }
      
    }
    
