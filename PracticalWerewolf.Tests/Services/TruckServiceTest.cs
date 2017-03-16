using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PracticalWerewolf.Models.Trucks;
using System.Collections.Generic;
using PracticalWerewolf.Stores.Interfaces;
using Moq;
using PracticalWerewolf.Services;
using System.Linq;
using PracticalWerewolf.Models;
using System.Device.Location;
using PracticalWerewolf.Tests.Stores.DbContext;
using PracticalWerewolf.Stores;

namespace PracticalWerewolf.Tests.Services
{
    [TestClass]
    public class TruckServiceTest
    {
        private static TruckCapacityUnit unit = new TruckCapacityUnit { TruckCapacityUnitGuid = Guid.NewGuid()};
        private static GeoCoordinate location = null;

        private static IEnumerable<Truck> _trucks = new List<Truck>
        {
            new Truck {TruckGuid = Guid.NewGuid(), CurrentCapacity = unit, MaxCapacity = unit, Location = location, LicenseNumber = "Hergedy1"},
            new Truck {TruckGuid = Guid.NewGuid(), CurrentCapacity = unit, MaxCapacity = unit, Location = location, LicenseNumber = "Hergedy2"},
            new Truck {TruckGuid = Guid.NewGuid(), CurrentCapacity = unit, MaxCapacity = unit, Location = location, LicenseNumber = "Hergedy3"}
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

        [TestMethod]
        public void UpdateTruckMaxCapacity_ValidTruck_TruckIsUpdated()
        {
            var dbSet = new MockTruckDbSet();
            var guid = Guid.NewGuid();
            var location = new GeoCoordinate(4, 7);
            var capacity = new TruckCapacityUnit() { TruckCapacityUnitGuid = Guid.NewGuid(), Mass = 3.14, Volume = 7 };
            var newCapacity = new TruckCapacityUnit() { TruckCapacityUnitGuid = Guid.NewGuid(), Mass = 51, Volume = 7 };
            dbSet.Add(new Truck() { TruckGuid = guid, CurrentCapacity = capacity, MaxCapacity = capacity, Location = location , LicenseNumber = "James"});
            var mockContext = new Mock<ITruckDbContext>();
            mockContext.Setup(x => x.Truck).Returns(dbSet);
            var store = new TruckStore(mockContext.Object);
            var dbContext = new Mock<ApplicationDbContext>();
            var truckService = new TruckService(store, dbContext.Object);

            truckService.UpdateTruckMaxCapacity(guid, newCapacity);

            var truck = truckService.GetTruck(guid);
            Assert.AreEqual(guid, truck.TruckGuid);
            Assert.AreEqual(location, truck.Location);
            Assert.AreEqual(capacity, truck.CurrentCapacity);
            Assert.AreEqual(newCapacity, truck.MaxCapacity);
            Assert.AreEqual("James", truck.LicenseNumber);
        }
        
        [TestMethod]
        public void UpdateTruckMaxCapacity_InvalidGuid_DoesNothing()
        {
            var dbSet = new MockTruckDbSet();
            var guid = Guid.NewGuid();
            var location = new GeoCoordinate(4, 7);
            var capacity = new TruckCapacityUnit() { TruckCapacityUnitGuid = Guid.NewGuid(), Mass = 3.14, Volume = 7 };
            var newCapacity = new TruckCapacityUnit() { TruckCapacityUnitGuid = Guid.NewGuid(), Mass = 51, Volume = 7 };
            dbSet.Add(new Truck() { TruckGuid = guid, CurrentCapacity = capacity, MaxCapacity = capacity, Location = location, LicenseNumber = "Abbie" });
            var mockContext = new Mock<ITruckDbContext>();
            mockContext.Setup(x => x.Truck).Returns(dbSet);
            var store = new TruckStore(mockContext.Object);
            var dbContext = new Mock<ApplicationDbContext>();
            var truckService = new TruckService(store, dbContext.Object);

            var newGuid = Guid.NewGuid();
            truckService.UpdateTruckMaxCapacity(newGuid, newCapacity);

            var truck = truckService.GetTruck(guid);
            Assert.AreEqual(guid, truck.TruckGuid);
            Assert.AreEqual(location, truck.Location);
            Assert.AreEqual(capacity, truck.CurrentCapacity);
            Assert.AreEqual(capacity, truck.MaxCapacity);
            Assert.IsNull(truckService.GetTruck(newGuid));
            Assert.AreEqual("Abbie", truck.LicenseNumber);
        }

        [TestMethod, ExpectedException(typeof(ArgumentNullException))]
        public void UpdateTruckMaxCapacity_NullCapacity_ThrowsException()
        {
            var dbSet = new MockTruckDbSet();
            var guid = Guid.NewGuid();
            var location = new GeoCoordinate(4, 7);
            var capacity = new TruckCapacityUnit() { TruckCapacityUnitGuid = Guid.NewGuid(), Mass = 3.14, Volume = 7 };
            TruckCapacityUnit newCapacity = null;
            dbSet.Add(new Truck() { TruckGuid = guid, CurrentCapacity = capacity, MaxCapacity = capacity, Location = location, LicenseNumber = "Matt" });
            var mockContext = new Mock<ITruckDbContext>();
            mockContext.Setup(x => x.Truck).Returns(dbSet);
            var store = new TruckStore(mockContext.Object);
            var dbContext = new Mock<ApplicationDbContext>();
            var truckService = new TruckService(store, dbContext.Object);

            var newGuid = Guid.NewGuid();
            truckService.UpdateTruckMaxCapacity(newGuid, newCapacity);
        }


        [TestMethod]
        public void CreateTruck_ValidTruck_TestExists()
        {
            var dbSet = new MockTruckDbSet();
            var mockContext = new Mock<ITruckDbContext>();
            mockContext.Setup(x => x.Truck).Returns(dbSet);
            var truckStore = new TruckStore(mockContext.Object);
            var mockDbContext = new Mock<ApplicationDbContext>();
            var truckService = new TruckService(truckStore, mockDbContext.Object);

            var guid = Guid.NewGuid();
            var location = new GeoCoordinate(4, 7);
            var capacity = new TruckCapacityUnit() { TruckCapacityUnitGuid = Guid.NewGuid(), Mass = 3.14, Volume = 7 };
            var truck = new Truck { TruckGuid = guid, MaxCapacity = capacity, CurrentCapacity = capacity, LicenseNumber = "Matt", Location = location };
            truckService.CreateTruck(truck);

            var result = truckService.GetTruck(guid);
            Assert.AreEqual(truck, result);
        }

        [TestMethod, ExpectedException(typeof(ArgumentNullException))]
        public void CreateTruck_NullTruck_ThrowsException()
        {
            var dbSet = new MockTruckDbSet();
            var mockContext = new Mock<ITruckDbContext>();
            mockContext.Setup(x => x.Truck).Returns(dbSet);
            var truckStore = new TruckStore(mockContext.Object);
            var mockDbContext = new Mock<ApplicationDbContext>();
            var truckService = new TruckService(truckStore, mockDbContext.Object);

            Truck truck = null;
            truckService.CreateTruck(truck);
        }

    }
}
