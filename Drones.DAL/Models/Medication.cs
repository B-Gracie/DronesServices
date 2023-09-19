using System.ComponentModel.DataAnnotations.Schema;

namespace Drones.DAL;

[Table("Medications", Schema = "DronesServices")]
public class Medication
{
    public int Id { get; set; }
    public int DroneId { get; set; }
    public string Name { get; set; } // allowed only letters, numbers, '-', '_'
    public double Weight { get; set; }
    
    public string? ImagePath { get; set; }
    public string Code { get; set; } // allowed only upper case letters, underscore, and numbers
}