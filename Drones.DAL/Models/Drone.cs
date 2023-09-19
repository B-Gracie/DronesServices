using System.ComponentModel.DataAnnotations.Schema;

namespace Drones.DAL;

[Table("Drones", Schema = "DronesServices")]
public class Drone
{
    public int Id { get; set; }
    public string SerialNumber { get; set; } // 100 characters max
    public string Model { get; set; } // Lightweight, Middleweight, Cruiserweight, Heavyweight
    public double WeightLimit { get; set; } // 500 grams max
    public int BatteryCapacity { get; set; } // Percentage
    
    public DroneState State { get; set; } // IDLE, LOADING, LOADED, DELIVERING, DELIVERED, RETURNING

    // Navigation property for loaded medications
    public List<LoadedMedication> LoadedMedications { get; set; }
}

public enum DroneState
{
    IDLE = 1,
    LOADING = 2,
    LOADED = 3,
    DELIVERING = 4,
    DELIVERED = 5,
    RETURNING = 6
}
