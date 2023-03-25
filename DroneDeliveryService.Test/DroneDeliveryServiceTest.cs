using System.Collections.Generic;
using System.Linq;
using DroneDeliveryService.Models;
using DroneDeliveryService.Services;

namespace DroneDeliveryService.Test;

public class DroneDeliveryServiceTest
{
    private readonly IDeliveryService _deliveryService;

    public DroneDeliveryServiceTest()
    {
        _deliveryService = new DeliveryService();
    }

    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void Test1()
    {
        List<DroneModel> drones = new List<DroneModel>()
        {
            new() { Name = "DroneA", Weight = 200 },
            new() { Name = "DroneB", Weight = 250 },
            new() { Name = "DroneC", Weight = 100 },
        };
        List<LocationModel> locations = new List<LocationModel>()
        {
            new() { Name = "LocationA", Weight = 200 },
            new() { Name = "LocationB", Weight = 150 },
            new() { Name = "LocationC", Weight = 50 },
            new() { Name = "LocationD", Weight = 150 },
            new() { Name = "LocationE", Weight = 100 },
            new() { Name = "LocationF", Weight = 200 },
            new() { Name = "LocationG", Weight = 50 },
            new() { Name = "LocationH", Weight = 80 },
            new() { Name = "LocationI", Weight = 70 },
            new() { Name = "LocationJ", Weight = 50 },
            new() { Name = "LocationK", Weight = 30 },
            new() { Name = "LocationL", Weight = 20 },
            new() { Name = "LocationM", Weight = 50 },
            new() { Name = "LocationN", Weight = 30 },
            new() { Name = "LocationO", Weight = 20 },
            new() { Name = "LocationP", Weight = 90 },
        };
        var result = this._deliveryService.GenerateDeliveries(drones, locations);
        Assert.AreEqual(result.Sum(x => x.Trips.Count()), 6);
    }

    [Test]
    public void Test2()
    {
        List<DroneModel> drones = new List<DroneModel>()
        {
            new() { Name = "DroneA", Weight = 300 },
            new() { Name = "DroneB", Weight = 350 },
            new() { Name = "DroneC", Weight = 200 },
        };
        List<LocationModel> locations = new List<LocationModel>()
        {
            new() { Name = "LocationA", Weight = 200 },
            new() { Name = "LocationB", Weight = 150 },
            new() { Name = "LocationC", Weight = 50 },
            new() { Name = "LocationD", Weight = 150 },
            new() { Name = "LocationE", Weight = 100 },
            new() { Name = "LocationF", Weight = 200 },
            new() { Name = "LocationG", Weight = 50 },
            new() { Name = "LocationH", Weight = 80 },
            new() { Name = "LocationI", Weight = 70 },
            new() { Name = "LocationJ", Weight = 50 },
        };
        var result = this._deliveryService.GenerateDeliveries(drones, locations);
        Assert.AreEqual(result.Sum(x => x.Trips.Count()), 4);
    }

    [Test]
    public void Test3()
    {
        List<DroneModel> drones = new List<DroneModel>()
        {
            new() { Name = "DroneA", Weight = 100 },
            new() { Name = "DroneB", Weight = 80 },
            new() { Name = "DroneC", Weight = 40 },
        };
        List<LocationModel> locations = new List<LocationModel>()
        {
            new() { Name = "LocationA", Weight = 80 },
            new() { Name = "LocationB", Weight = 60 },
            new() { Name = "LocationC", Weight = 30 },
            new() { Name = "LocationD", Weight = 30 },
            new() { Name = "LocationE", Weight = 20 },
            new() { Name = "LocationF", Weight = 10 },
            new() { Name = "LocationG", Weight = 40 },
            new() { Name = "LocationH", Weight = 100 },
            new() { Name = "LocationI", Weight = 20 },
            new() { Name = "LocationJ", Weight = 10 },
        };
        var result = this._deliveryService.GenerateDeliveries(drones, locations);
        Assert.AreEqual(result.Sum(x => x.Trips.Count()), 6);
    }

    [Test]
    public void TestReadFileDronesAndLocations()
    {
        var result = this._deliveryService.ReadFileDronesAndLocations("input.txt");
        Assert.AreEqual(6, 6);
    }


    [Test]
    public void TestWriteDeliveryFile()
    {
        var (drones, locations) = this._deliveryService.ReadFileDronesAndLocations("input.txt");
        var deliveries = this._deliveryService.GenerateDeliveries(drones, locations);
        this._deliveryService.WriteDeliveryFile(deliveries, "output.txt");
    }
}