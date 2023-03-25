using System.Collections.Generic;

namespace DroneDeliveryService.Models;

public class TripModel
{
    public int CapacityWeight { get; set; }
    public List<LocationModel> Locations { get; set; }
}