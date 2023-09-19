using System.ComponentModel.DataAnnotations.Schema;

namespace Drones.DAL;

[Table("BatteryLogs", Schema = "DronesServices")]
public class BatteryLog
{
    public int Id { get; set; }
    public int DroneId { get; set; }
    public DateTime Timestamp { get; set; }
    public int BatteryLevel { get; set; }

    // Navigation property for the associated drone
    public Drone Drone { get; set; }
}