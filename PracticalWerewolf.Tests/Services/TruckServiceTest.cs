using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PracticalWerewolf.Models.Trucks;
using System.Collections.Generic;
using PracticalWerewolf.Stores.Interfaces;
using Moq;
using PracticalWerewolf.Services;
using System.Linq;
using PracticalWerewolf.Models;
using PracticalWerewolf.Tests.Stores.DbContext;
using PracticalWerewolf.Stores;
using System.Data.Entity.Spatial;
using PracticalWerewolf.Controllers.UnitOfWork;
using PracticalWerewolf.Application;
using System.Data.Entity;

namespace PracticalWerewolf.Tests.Services
{
    [TestClass]
    public class TruckServiceTest
    {
        private static TruckCapacityUnit unit = new TruckCapacityUnit { TruckCapacityUnitGuid = Guid.NewGuid()};
        private static string point = String.Format("POINT({0} {1})", 3.14, 2.18);
        private static DbGeography location = DbGeography.FromText(point);

        private static IEnumerable<Truck> _trucks = new List<Truck>
        {
            new Truck {TruckGuid = Guid.NewGuid(), UsedCapacity = unit, MaxCapacity = unit, Location = location, LicenseNumber = "Hergedy1"},
            new Truck {TruckGuid = Guid.NewGuid(), UsedCapacity = unit, MaxCapacity = unit, Location = location, LicenseNumber = "Hergedy2"},
            new Truck {TruckGuid = Guid.NewGuid(), UsedCapacity = unit, MaxCapacity = unit, Location = location, LicenseNumber = "Hergedy3"}
        };


        [TestMethod]
        public void GetAllTrucks_NoTrucks_EmptyList()
        {
            var truckStore = new Mock<ITruckStore>();
            truckStore.Setup(x => x.GetAllTrucks()).Returns(new List<Truck>());
            var truckService = new TruckService(truckStore.Object);

            var trucks = truckService.GetAllTrucks();

            Assert.AreEqual(0, trucks.Count());
        }

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



        [TestMethod]
        public void GetTruck_ExistingTruck_FindsTruck()
        {
            var chosenGuid = _trucks.ElementAt(0).TruckGuid;
            
            var truckStore = new Mock<ITruckStore>();
            truckStore.Setup(x => x.Get(It.IsAny<Guid>())).Returns(_trucks.ElementAt(0));
            var truckService = new TruckService(truckStore.Object);

            var truck = truckService.GetTruck(chosenGuid);

            Assert.AreEqual(_trucks.ElementAt(0), truck);
        }

        [TestMethod]
        public void GetTruck_NoTruckWithThatGuid_ReturnNull()
        {
            var guid = Guid.NewGuid();
            var truckStore = new Mock<ITruckStore>();
            truckStore.Setup(x => x.Get(It.IsAny<Guid>())).Returns((Truck) null);
            var truckService = new TruckService(truckStore.Object);

            var truck = truckService.GetTruck(guid);

            Assert.IsNull(truck);
        }

        [TestMethod]
        public void UpdateTruckMaxCapacity_ValidTruck_TruckIsUpdated()
        {
            var dbSet = new MockTruckDbSet();
            var guid = Guid.NewGuid();
            var point = String.Format("POINT({0} {1})", 3.14, 2.18);
            var location = DbGeography.FromText(point);
            var capacity = new TruckCapacityUnit() { TruckCapacityUnitGuid = Guid.NewGuid(), Mass = 3.14, Volume = 7 };
            var newCapacity = new TruckCapacityUnit() { TruckCapacityUnitGuid = Guid.NewGuid(), Mass = 51, Volume = 7 };
            var truck = new Truck() { TruckGuid = guid, UsedCapacity = capacity, MaxCapacity = capacity, LicenseNumber = "James", Location = location };
            dbSet.Add(truck);
            var truckService = GetTruckServiceWithDbSet(dbSet);

            truck.MaxCapacity = newCapacity;
            truckService.Update(truck);

            var result = truckService.GetTruck(guid);
            Assert.AreEqual(guid, result.TruckGuid);
            Assert.AreEqual(capacity, result.UsedCapacity);
            Assert.AreEqual(newCapacity, result.MaxCapacity);
            Assert.AreEqual("James", result.LicenseNumber);
            Assert.AreEqual(location, result.Location);
        }
        
        [TestMethod]
        public void UpdateTruckMaxCapacity_InvalidGuid_DoesNothing()
        {
            var dbSet = new MockTruckDbSet();
            var guid = Guid.NewGuid();
            var point = String.Format("POINT({0} {1})", 3.14, 2.18);
            var location = DbGeography.FromText(point);
            var capacity = new TruckCapacityUnit() { TruckCapacityUnitGuid = Guid.NewGuid(), Mass = 3.14, Volume = 7 };
            var newCapacity = new TruckCapacityUnit() { TruckCapacityUnitGuid = Guid.NewGuid(), Mass = 51, Volume = 7 };
            dbSet.Add(new Truck() { TruckGuid = guid, UsedCapacity = capacity, MaxCapacity = capacity,  LicenseNumber = "Abbie", Location = location });
            var truckService = GetTruckServiceWithDbSet(dbSet);

            var newGuid = Guid.NewGuid();
            var newTruck = new Truck() { TruckGuid = newGuid };
            truckService.Update(newTruck);

            var truck = truckService.GetTruck(guid);
            Assert.AreEqual(guid, truck.TruckGuid);
            Assert.AreEqual(capacity, truck.UsedCapacity);
            Assert.AreEqual(capacity, truck.MaxCapacity);
            Assert.AreEqual("Abbie", truck.LicenseNumber);
            Assert.AreEqual(location, truck.Location);
            Assert.IsNull(truckService.GetTruck(newGuid));
        }

        [TestMethod, ExpectedException(typeof(ArgumentNullException))]
        public void UpdateTruckMaxCapacity_NullTruck_ThrowsException()
        {
            var dbSet = new MockTruckDbSet();
            var guid = Guid.NewGuid();
            var point = String.Format("POINT({0} {1})", 3.14, 2.18);
            var location = DbGeography.FromText(point);
            var capacity = new TruckCapacityUnit() { TruckCapacityUnitGuid = Guid.NewGuid(), Mass = 3.14, Volume = 7 };
            dbSet.Add(new Truck() { TruckGuid = guid, UsedCapacity = capacity, MaxCapacity = capacity, LicenseNumber = "Matt" , Location = location});
            var truckService = GetTruckServiceWithDbSet(dbSet);

            Truck newTruck = null;
            truckService.Update(newTruck);
        }


        [TestMethod]
        public void CreateTruck_ValidTruck_TestExists()
        {
            var dbSet = new MockTruckDbSet();
            var truckService = GetTruckServiceWithDbSet(dbSet);

            var guid = Guid.NewGuid();
            var capacity = new TruckCapacityUnit() { TruckCapacityUnitGuid = Guid.NewGuid(), Mass = 3.14, Volume = 7 };
            var truck = new Truck { TruckGuid = guid, MaxCapacity = capacity, UsedCapacity = capacity, LicenseNumber = "Matt" };
            truckService.CreateTruck(truck);

            var result = truckService.GetTruck(guid);
            Assert.AreEqual(truck, result);
        }

        [TestMethod, ExpectedException(typeof(ArgumentNullException))]
        public void CreateTruck_NullTruck_ThrowsException()
        {
            var dbSet = new MockTruckDbSet();
            var truckService = GetTruckServiceWithDbSet(dbSet);

            Truck truck = null;
            truckService.CreateTruck(truck);
        }



        public static TruckService GetTruckServiceWithDbSet(DbSet<Truck> dbSet)
        {

            var mockContext = new Mock<IDbSetFactory>();
            mockContext.Setup(x => x.CreateDbSet<Truck>()).Returns(dbSet);
            var store = new TruckStore(mockContext.Object);
            var truckService = new TruckService(store);

            return truckService;
        }
    }
}
