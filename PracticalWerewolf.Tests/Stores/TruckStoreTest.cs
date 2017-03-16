using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using PracticalWerewolf.Models.Trucks;
using System.Linq;
using Moq;
using PracticalWerewolf.Tests.Stores.DbContext;
using PracticalWerewolf.Stores.Interfaces;
using PracticalWerewolf.Stores;
using System.Device.Location;
using PracticalWerewolf.Models;

namespace PracticalWerewolf.Tests.Stores
{
    [TestClass]
    public class TruckStoreTest
    {
        private static TruckCapacityUnit unit = new TruckCapacityUnit { TruckCapacityUnitGuid = Guid.NewGuid() };
        private static GeoCoordinate location = null;

        private static IEnumerable<Truck> _trucks = new List<Truck>
        {
            new Truck {TruckGuid = Guid.NewGuid(), CurrentCapacity = unit, MaxCapacity = unit, Location = location, LicenseNumber = "Hergedy1"},
            new Truck {TruckGuid = Guid.NewGuid(), CurrentCapacity = unit, MaxCapacity = unit, Location = location, LicenseNumber = "Hergedy2"},
            new Truck {TruckGuid = Guid.NewGuid(), CurrentCapacity = unit, MaxCapacity = unit, Location = location, LicenseNumber = "Hergedy3"}
        };

        [TestMethod]
        public void GetAll_EmptyDbSetList_SizeZero()
        {
            var dbContext = new Mock<ApplicationDbContext>();
            dbContext.Setup(x => x.Truck).Returns(new MockDbSet<Truck>());
            ITruckStore store = new TruckStore(dbContext.Object);

            Assert.AreEqual(0, store.GetAllTrucks().Count());
        }

        [TestMethod]
        public void GetAll_ThreeTrucks_SizeZero()
        {
            var dbContext = new Mock<ApplicationDbContext>();
            var dbSet = new MockTruckDbSet();
            dbSet.AddRange(_trucks);
            dbContext.Setup(x => x.Truck).Returns(dbSet);
            ITruckStore store = new TruckStore(dbContext.Object);

            var trucks = store.GetAllTrucks();

            Assert.AreEqual(3, trucks.Count());
            Assert.IsTrue(trucks.Contains(_trucks.ElementAt(0)));
            Assert.IsTrue(trucks.Contains(_trucks.ElementAt(1)));
            Assert.IsTrue(trucks.Contains(_trucks.ElementAt(2)));
        }

        [TestMethod]
        public void Get_OneMatchingTruck_ReturnTruck()
        {
            var chosenGuid = _trucks.ElementAt(0).TruckGuid;
            var context = new Mock<ApplicationDbContext>();
            var dbSet = new MockTruckDbSet();
            dbSet.AddRange(_trucks);
            context.Setup(x => x.Truck).Returns(dbSet);
            ITruckStore store = new TruckStore(context.Object);

            var truck = store.Get(chosenGuid);

            Assert.IsNotNull(truck);
            Assert.AreEqual(_trucks.ElementAt(0), truck);
        }

        [TestMethod]
        public void Get_NoMatchingTrucks_ReturnTruck()
        {
            var chosenGuid = Guid.NewGuid();
            var context = new Mock<ApplicationDbContext>();
            var dbSet = new MockTruckDbSet();
            dbSet.AddRange(_trucks);
            context.Setup(x => x.Truck).Returns(dbSet);
            ITruckStore store = new TruckStore(context.Object);

            var truck = store.Get(chosenGuid);

            Assert.IsNull(truck);
        }

        [TestMethod]
        public void Get_MultipleMatchingTrucks_ReturnTruck()
        {
            var chosenGuid = _trucks.ElementAt(0).TruckGuid;
            var context = new Mock<ApplicationDbContext>();
            var dbSet = new MockTruckDbSet();
            dbSet.AddRange(_trucks);
            dbSet.Add(new Truck() { TruckGuid = chosenGuid, Location = location, CurrentCapacity = unit, MaxCapacity = unit, LicenseNumber = "James" });
            context.Setup(x => x.Truck).Returns(dbSet);
            ITruckStore store = new TruckStore(context.Object);

            var truck = store.Get(chosenGuid);

            Assert.IsNotNull(truck);
            Assert.AreEqual(_trucks.ElementAt(0), truck);
        }

        [TestMethod]
        public void Update_ValidTruck_UpdatesTruck()
        {
            var guid = Guid.NewGuid();
            var location = new GeoCoordinate(2.18, 3.14);
            var capacity = new TruckCapacityUnit() { Mass = 12, Volume = 12, TruckCapacityUnitGuid = Guid.NewGuid() };
            var capacityGuid = Guid.NewGuid();
            var truck = new Truck() { TruckGuid = guid, CurrentCapacity = capacity, MaxCapacity = capacity, Location = location, LicenseNumber = "Abbie"};
            var context = new Mock<ApplicationDbContext>();
            var dbSet = new MockTruckDbSet();
            dbSet.Add(truck);
            context.Setup(x => x.Truck).Returns(dbSet);
            var truckStore = new TruckStore(context.Object);

            var newLocation = new GeoCoordinate(3.14, 2.18);
            var newCapacity = new TruckCapacityUnit() { Mass = 24, Volume = 24, TruckCapacityUnitGuid = capacityGuid};
            var newTruck = new Truck() { TruckGuid = truck.TruckGuid, CurrentCapacity = newCapacity, MaxCapacity = newCapacity, Location = newLocation, LicenseNumber = "Matt" };
            truckStore.Update(newTruck);

            var result = truckStore.Get(guid);
            Assert.AreEqual(guid, result.TruckGuid);
            Assert.AreEqual(newLocation, result.Location);
            Assert.AreEqual(newCapacity, result.MaxCapacity);
            Assert.AreEqual(newCapacity, result.CurrentCapacity);
            Assert.AreEqual("Matt", result.LicenseNumber);
        }

        [TestMethod]
        public void Update_InvalidTruck_NoChangesMade()
        {
            var guid = Guid.NewGuid();
            var location = new GeoCoordinate(2.18, 3.14);
            var capacity = new TruckCapacityUnit() { Mass = 12, Volume = 12, TruckCapacityUnitGuid = Guid.NewGuid() };
            var capacityGuid = Guid.NewGuid();
            var truck = new Truck() { TruckGuid = guid, CurrentCapacity = capacity, MaxCapacity = capacity, Location = location , LicenseNumber = "Jessee"};
            var context = new Mock<ApplicationDbContext>();
            var dbSet = new MockTruckDbSet();
            dbSet.Add(truck);
            context.Setup(x => x.Truck).Returns(dbSet);
            var truckStore = new TruckStore(context.Object);

            var newLocation = new GeoCoordinate(3.14, 2.18);
            var newCapacity = new TruckCapacityUnit() { Mass = 24, Volume = 24, TruckCapacityUnitGuid = capacityGuid };
            var newTruck = new Truck() { TruckGuid = Guid.NewGuid(), CurrentCapacity = newCapacity, MaxCapacity = newCapacity, Location = newLocation , LicenseNumber = "Not James"};
            truckStore.Update(newTruck);

            var result = truckStore.Get(guid);
            Assert.AreEqual(guid, result.TruckGuid);
            Assert.AreEqual(location, result.Location);
            Assert.AreEqual(capacity, result.MaxCapacity);
            Assert.AreEqual(capacity, result.CurrentCapacity);
            Assert.AreEqual("Jessee", result.LicenseNumber);
        }

        [TestMethod, ExpectedException(typeof(ArgumentNullException))]
        public void Update_NullTruck_ThrowsException()
        {
            
            var context = new Mock<ApplicationDbContext>();
            var dbSet = new MockTruckDbSet();
            context.Setup(x => x.Truck).Returns(dbSet);
            var truckStore = new TruckStore(context.Object);
            Truck truck = null;

            truckStore.Update(truck);
        }

        [TestMethod]
        public void Create_ValidTruck_AddsToDbSet()
        {
            var context = new Mock<ApplicationDbContext>();
            var dbSet = new MockTruckDbSet();
            context.Setup(x => x.Truck).Returns(dbSet);
            var truckStore = new TruckStore(context.Object);

            var guid = Guid.NewGuid();
            var location = new GeoCoordinate(2.18, 3.14);
            var capacity = new TruckCapacityUnit() { Mass = 12, Volume = 12, TruckCapacityUnitGuid = Guid.NewGuid() };
            var capacityGuid = Guid.NewGuid();
            var truck = new Truck() { TruckGuid = guid, CurrentCapacity = capacity, MaxCapacity = capacity, Location = location, LicenseNumber =  "Cooper"};
            truckStore.Create(truck);

            var result = truckStore.Get(guid);
            Assert.AreEqual(truck, result);
        }

        [TestMethod, ExpectedException(typeof(ArgumentNullException))]
        public void Create_NullTruck_ThrowsException()
        {
            var context = new Mock<ApplicationDbContext>();
            var dbSet = new MockTruckDbSet();
            context.Setup(x => x.Truck).Returns(dbSet);
            var truckStore = new TruckStore(context.Object);

            Truck truck = null;
            truckStore.Create(truck);
        }
    }
}
