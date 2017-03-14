using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using PracticalWerewolf.Models.Trucks;
using System.Linq;
using Moq;
using PracticalWerewolf.Stores.Interfaces.Contexts;
using PracticalWerewolf.Tests.Stores.DbContext;
using PracticalWerewolf.Stores.Interfaces;
using PracticalWerewolf.Stores;
using System.Device.Location;

namespace PracticalWerewolf.Tests.Stores
{
    [TestClass]
    public class TruckStoreTest
    {
        private static TruckCapacityUnit unit = new TruckCapacityUnit { TruckCapacityUnitGuid = Guid.NewGuid() };
        private static GeoCoordinate location = null;

        private static IEnumerable<Truck> _trucks = new List<Truck>
        {
            new Truck {TruckGuid = Guid.NewGuid(), CurrentCapacity = unit, MaxCapacity = unit, Location = location},
            new Truck {TruckGuid = Guid.NewGuid(), CurrentCapacity = unit, MaxCapacity = unit, Location = location},
            new Truck {TruckGuid = Guid.NewGuid(), CurrentCapacity = unit, MaxCapacity = unit, Location = location}
        };

        [TestMethod]
        public void GetAll_EmptyDbSetList_SizeZero()
        {
            //TruckDbSet
            Mock<ITruckDbContext> dbContext = new Mock<ITruckDbContext>();
            dbContext.Setup(x => x.Truck).Returns(new MockDbSet<Truck>());
            ITruckStore store = new TruckStore(dbContext.Object);

            Assert.AreEqual(0, store.GetAllTrucks().Count());
        }

        [TestMethod]
        public void GetAll_ThreeTrucks_SizeZero()
        {
            Mock<ITruckDbContext> dbContext = new Mock<ITruckDbContext>();
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
            var context = new Mock<ITruckDbContext>();
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
            var context = new Mock<ITruckDbContext>();
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
            var context = new Mock<ITruckDbContext>();
            var dbSet = new MockTruckDbSet();
            dbSet.AddRange(_trucks);
            dbSet.Add(new Truck() { TruckGuid = chosenGuid, Location = location, CurrentCapacity = unit, MaxCapacity = unit });
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
            var truck = new Truck() { TruckGuid = guid, CurrentCapacity = capacity, MaxCapacity = capacity, Location = location};
            var context = new Mock<ITruckDbContext>();
            var dbSet = new MockTruckDbSet();
            dbSet.Add(truck);
            context.Setup(x => x.Truck).Returns(dbSet);

            var newLocation = new GeoCoordinate(3.14, 2.18);
            var newCapacity = new TruckCapacityUnit() { Mass = 24, Volume = 24, TruckCapacityUnitGuid = capacityGuid };
            var newTruck = new Truck() { TruckGuid = truck.TruckGuid, CurrentCapacity = newCapacity, MaxCapacity = newCapacity, Location = newLocation };

            //TODO finish this
        }


    }
}
