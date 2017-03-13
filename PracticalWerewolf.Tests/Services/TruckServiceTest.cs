using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PracticalWerewolf.Models.Trucks;
using System.Collections.Generic;
using PracticalWerewolf.Stores.Interfaces;
using Moq;
using PracticalWerewolf.Services;
using System.Linq;
using PracticalWerewolf.Models;

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
        public void GetAllTrucks_NoTrucks_EmptyList()
        {
            var truckStore = new Mock<ITruckStore>();
            truckStore.Setup(x => x.GetAllTrucks()).Returns(new List<Truck>());
            var context = new Mock<ApplicationDbContext>();
            var truckService = new TruckService(truckStore.Object, context.Object);

            var trucks = truckService.GetAllTrucks();

            Assert.AreEqual(0, trucks.Count());
        }

        [TestMethod]
        public void GetAllTrucks_ThreeTrucks_GetAll()
        {
            var truckStore = new Mock<ITruckStore>();
            truckStore.Setup(x => x.GetAllTrucks()).Returns(_trucks);
            var context = new Mock<ApplicationDbContext>();
            var truckService = new TruckService(truckStore.Object, context.Object);
            
            var trucks = truckService.GetAllTrucks();

            Assert.AreEqual(3, trucks.Count());
            Assert.IsTrue(trucks.Contains(_trucks.ElementAt(0)));
            Assert.IsTrue(trucks.Contains(_trucks.ElementAt(1)));
            Assert.IsTrue(trucks.Contains(_trucks.ElementAt(2)));
        }



        [TestMethod]
        public void GetTruck_ExistingTruck_FindsTruck()
        {
            var chosenGuid = _trucks.ElementAt(0).TruckGuid;
            
            var truckStore = new Mock<ITruckStore>();
            truckStore.Setup(x => x.Get(It.IsAny<Guid>())).Returns(_trucks.ElementAt(0));
            var context = new Mock<ApplicationDbContext>();
            var truckService = new TruckService(truckStore.Object, context.Object);

            var truck = truckService.GetTruck(chosenGuid);

            Assert.AreEqual(_trucks.ElementAt(0), truck);
        }

        [TestMethod]
        public void GetTruck_NoTruckWithThatGuid_ReturnNull()
        {
            var guid = Guid.NewGuid();
            var truckStore = new Mock<ITruckStore>();
            truckStore.Setup(x => x.Get(It.IsAny<Guid>())).Returns((Truck) null);
            var context = new Mock<ApplicationDbContext>();
            var truckService = new TruckService(truckStore.Object, context.Object);

            var truck = truckService.GetTruck(guid);

            Assert.IsNull(truck);
        }


    }
}
