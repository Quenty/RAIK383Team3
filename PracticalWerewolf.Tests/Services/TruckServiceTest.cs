using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PracticalWerewolf.Models.Trucks;
using System.Collections.Generic;
using PracticalWerewolf.Stores.Interfaces;
using Moq;
using PracticalWerewolf.Services;
using System.Linq;

namespace PracticalWerewolf.Tests.Services
{
    [TestClass]
    public class TruckServiceTest
    {
        private static TruckCapacityUnit unit = new TruckCapacityUnit { TruckCapacityUnitGuid = Guid.NewGuid()};
        private static System.Data.Entity.Spatial.DbGeography location = null;

        private static IEnumerable<Truck> _trucks = new List<Truck>
        {
            new Truck {TruckGuid = Guid.NewGuid(), CurrentCapacity = unit, MaxCapacity = unit, Location = location},
            new Truck {TruckGuid = Guid.NewGuid(), CurrentCapacity = unit, MaxCapacity = unit, Location = location},
            new Truck {TruckGuid = Guid.NewGuid(), CurrentCapacity = unit, MaxCapacity = unit, Location = location}
        };

        [TestMethod]
        public void GetAllTrucks_ThreeTrucks_GetAll()
        {
            var truckStore = new Mock<ITruckStore>();
            truckStore.Setup(x => x.GetAllTrucks()).Returns(_trucks);
            var truckService = new TruckService(truckStore.Object);
            
            var trucks = truckService.GetAllTrucks();

            Assert.AreEqual(3, trucks.Count());
            Assert.IsTrue(trucks.Contains(_trucks.ElementAt(0)));
            Assert.IsTrue(trucks.Contains(_trucks.ElementAt(1)));
            Assert.IsTrue(trucks.Contains(_trucks.ElementAt(2)));
        }

    }
}
