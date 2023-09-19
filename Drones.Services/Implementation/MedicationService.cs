using Drones.DAL;
using Drones.DAL.DronesDTO;
using Drones.DAL.Repository.Interfaces;
using Drones.Services.Interfaces;


namespace Drones.Services.Implementation;

public class MedicationService : IMedicationService
    {
        private readonly IMedicationRepository _medicationRepository;
        private readonly IDroneRepository _droneRepository;

        public MedicationService(IMedicationRepository medicationRepository, IDroneRepository droneRepository)
        {
            _medicationRepository = medicationRepository;
            _droneRepository = droneRepository;
        }
        public async Task<IEnumerable<Medication>> GetLoadedMedications()
        {
            try
            {
                // Call the repository method to get medications
                var medications = await _medicationRepository.GetLoadedMedications();

                // Handle null values for ImagePath
                foreach (var medication in medications)
                {
                    if (medication.ImagePath == null)
                    {
                        // Handle null ImagePath, for example, set a default value
                        medication.ImagePath = "https://images.pexels.com/photos/360622/pexels-photo-360622.jpeg?auto=compress&cs=tinysrgb&w=600";
                    }
                }

                return medications;
            }
            catch (Exception ex)
            {
                // Log the error (use a logging framework like Serilog or Microsoft.Extensions.Logging)
                throw; // Re-throw the exception for the controller layer to handle
            }
        }
        

        public async Task<Medication> CreateMedicationAndLoadAsync(int droneId, MedicationDto medicationDto)
        {
            var newMedication = new Medication
            {
                Name = medicationDto.Name,
                Weight = medicationDto.Weight,
                Code = medicationDto.Code,
                DroneId = droneId 
            };

            var createdMedication = await _medicationRepository.CreateMedicationAsync(newMedication);

            // Load the newly created medication onto the drone
            await _droneRepository.LoadMedicationsAsync(droneId, new List<int> { createdMedication.Id });

            // Return the created medication
            return createdMedication;
        }
        

        public async Task<Medication> GetMedicationByIdAsync(int medicationId)
            {
                return await _medicationRepository.GetMedicationByIdAsync(medicationId);
            }
            public async Task<double> CalculateTotalMedicationWeightAsync(IEnumerable<int> medicationIds)
            {
                double totalWeight = 0.0;

                if (medicationIds == null || !medicationIds.Any())
                {
                    return totalWeight; // Return 0 if the list is empty or null
                }

                // Fetch medication weights from the repository
                var medications = await _medicationRepository.GetMedicationsByIdsAsync(medicationIds);

                if (medications != null && medications.Any())
                {
                    totalWeight = medications.Sum(m => m.Weight);
                }

                return totalWeight;
            }
    }

