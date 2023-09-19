using System.ComponentModel.DataAnnotations;

namespace Drones.DAL.DronesDTO;

public class MedicationDto
{
    [Required]
    [StringLength(100)]
    public string Name { get; set; }
    public string ImagePath { get; set; }
    public int Id { get; set; }
    public int DroneId { get; set; }
    public double Weight { get; set; }

    [Required]
    [RegularExpression("^[A-Z0-9_-]*$")] // Define an appropriate regex pattern
    [StringLength(50)]
    public string Code { get; set; }
}
