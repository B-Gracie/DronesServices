namespace Drones.DAL.Repository.Interfaces;

public interface IMedicationRepository
{
    Task<IEnumerable<Medication>> GetLoadedMedications();
    Task<Medication> CreateMedicationAsync(Medication medication);
    Task<Medication> GetMedicationByIdAsync(int medicationId);
    Task<IEnumerable<Medication>> GetMedicationsByIdsAsync(IEnumerable<int> medicationIds);

}