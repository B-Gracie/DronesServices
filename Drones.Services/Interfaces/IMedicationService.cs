using Drones.DAL;
using Drones.DAL.DronesDTO;

namespace Drones.Services.Interfaces;

public interface IMedicationService
{
    Task<IEnumerable<Medication>> GetLoadedMedications();
    Task<Medication> CreateMedicationAndLoadAsync(int droneId, MedicationDto medicationDto);
    Task<Medication> GetMedicationByIdAsync(int medicationId);
}