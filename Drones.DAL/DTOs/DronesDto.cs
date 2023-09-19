namespace Drones.DAL.DronesDTO;

public class DronesDto
{

    public int Id { get; set; }
    public string SerialNumber { get; set; }
    public string Model { get; set; }
    public double WeightLimit { get; set; }
    public int BatteryCapacity { get; set; }
    public string State { get; set; } // Represent the state as a string in DTO

        
}
