
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Drones.DAL
{
    
    [Table("LoadedMedications", Schema = "DronesServices")]
    
    public class LoadedMedication
    {
        public int Id { get; set; }  // Synthetic primary key
        public int MedicationId { get; set; }
        public int DroneId { get; set; }

        // Navigation properties
        public Medication Medication { get; set; }
        public Drone Drone { get; set; }
    }

}

