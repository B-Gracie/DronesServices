using Drones.DAL.DbContext;
using Drones.DAL.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Drones.DAL.Repository.Implementation;

public class MedicationRepository : IMedicationRepository
{
    private readonly DroneDbContext _dbContext;

    public MedicationRepository(DroneDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<Medication> CreateMedicationAsync(Medication medication)
    {
        _dbContext.Medications.Add(medication);
        await _dbContext.SaveChangesAsync();
        return medication;
    }
    
  
    public async Task<IEnumerable<Medication>> GetLoadedMedications()
    {
        try
        {
            var medications = await _dbContext.Medications.ToListAsync();

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
            throw; // Re-throw the exception for the service or controller layer to handle
        }
    }


    // public async Task<IEnumerable<Medication>> GetAllMedicationsAsync()
    // {
    //     return await _dbContext.Medications.ToListAsync();
    // }

    public async Task<Medication> GetMedicationByIdAsync(int medicationId)
    {
        return await _dbContext.Medications.FindAsync(medicationId);
    }
    public async Task<IEnumerable<Medication>> GetMedicationsByIdsAsync(IEnumerable<int> medicationIds)
    {
        return await _dbContext.Medications
            .Where(m => medicationIds.Contains(m.Id))
            .ToListAsync();
    }
}
