namespace DroneDeliveryService.Models;

public class DeliveryModel
{
    public DroneModel Drone { get; set; }
    public int DronePosition { get; set; }
    public List<TripModel> Trips { get; set; }
}