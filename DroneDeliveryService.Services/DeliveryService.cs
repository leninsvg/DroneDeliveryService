using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DroneDeliveryService.Models;

namespace DroneDeliveryService.Services;

public interface IDeliveryService
{
    List<DeliveryModel> GenerateDeliveries(List<DroneModel> drones, List<LocationModel> locations);
    (List<DroneModel>, List<LocationModel>) ReadFileDronesAndLocations(string textFilePath);
    void WriteDeliveryFile(List<DeliveryModel> deliveries, string textFilePath);
}

public class DeliveryService : IDeliveryService
{
    public List<DeliveryModel> GenerateDeliveries(List<DroneModel> drones, List<LocationModel> locations)
    {
        List<DeliveryModel> deliveries = drones.Select((drone, index) => new DeliveryModel()
        {
            Drone = drone,
            DronePosition = index,
            Trips = new List<TripModel>()
        }).OrderByDescending(delivery => delivery.Drone.Weight).ToList();
        this.SetDeliveryTrips(deliveries, locations);
        return deliveries;
    }

    public (List<DroneModel>, List<LocationModel>) ReadFileDronesAndLocations(string textFilePath)
    {
        List<DroneModel> drones = new List<DroneModel>();
        List<LocationModel> locations = new List<LocationModel>();
        string[] lines = File.ReadAllLines(textFilePath);
        for (int i = 0; i < lines.Length; i++)
        {
            string[] fragments = lines[i].Split(",");
            DroneModel drone = null;
            LocationModel location = null;
            for (int j = 0; j < fragments.Length; j++)
            {
                int mod = j % 2;
                if (i == 0)
                {
                    if (mod == 0)
                    {
                        drone = new DroneModel();
                        drone.Name = this.RemoveBracesTextValue(fragments[j]);
                    }
                    else
                    {
                        drone.Weight = Convert.ToInt32(this.RemoveBracesTextValue(fragments[j]));
                        drones.Add(drone);
                    }
                }
                else
                {
                    if (mod == 0)
                    {
                        location = new LocationModel();
                        location.Name = this.RemoveBracesTextValue(fragments[j]);
                    }
                    else
                    {
                        location.Weight = Convert.ToInt32(this.RemoveBracesTextValue(fragments[j]));
                        locations.Add(location);
                    }
                }
            }
        }
        return (drones, locations);
    }

    public void WriteDeliveryFile(List<DeliveryModel> deliveries, string textFilePath)
    {
        List<string> lines = new List<string>();
        foreach (DeliveryModel delivery in deliveries.OrderBy(x => x.DronePosition).ToList())
        {
            lines.Add(this.SetBracesTextValue(delivery.Drone.Name));
            if (delivery.Trips.Count == 0)
            {
                lines.Add("");
                continue;
            }
            delivery.Trips.Select((trip, index) => string.Join(",", trip.Locations.Select(location => this.SetBracesTextValue(location.Name))));
            for (int i = 0; i < delivery.Trips.Count; i++)
            {
                lines.Add($"Trip #{i + 1}");
                var locations = delivery.Trips[i].Locations.Select(location => this.SetBracesTextValue(location.Name));
                lines.Add(string.Join(", ", locations));
            }
            lines.Add("");
        }
        File.WriteAllLines(textFilePath, lines);
    }

    private void SetDeliveryTrips(List<DeliveryModel> deliveries, List<LocationModel> locations)
    {
        bool overWeight = false;
        foreach (var location in locations)
        {
            bool addNewTrip = overWeight && !deliveries.Any(x => x.Trips.LastOrDefault()?.CapacityWeight >= location.Weight);
            foreach (var delivery in deliveries)
            {
                TripModel newTrip = new()
                {
                    CapacityWeight = delivery.Drone.Weight - location.Weight,
                    Locations = new List<LocationModel>() { location }
                };
                if (addNewTrip)
                {
                    overWeight = false;
                    delivery.Trips.Add(newTrip);
                    break;
                }
                if (delivery.Trips.Count == 0 && delivery.Drone.Weight >= location.Weight)
                {
                    delivery.Trips.Add(newTrip);
                    break;
                }
                TripModel currentTrip = delivery.Trips.LastOrDefault();
                if (currentTrip.CapacityWeight == 0)
                {
                    delivery.Trips.Add(newTrip);
                    break;
                }
                if (currentTrip.CapacityWeight >= location.Weight)
                {
                    currentTrip.CapacityWeight -= location.Weight;
                    currentTrip.Locations.Add(location);
                    break;
                }
                overWeight = true;
            }
        }
    }

    private string RemoveBracesTextValue(string text)
    {
        return text.Replace("[", "").Replace("]", "");
    }

    private string SetBracesTextValue(string text)
    {
        return $"[{text}]";
    }
}