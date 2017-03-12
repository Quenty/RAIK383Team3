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

namespace PracticalWerewolf.Tests.Stores
{
    [TestClass]
    public class TruckStoreTest
    {
        private static TruckCapacityUnit unit = new TruckCapacityUnit { TruckCapacityUnitGuid = Guid.NewGuid() };
        private static System.Data.Entity.Spatial.DbGeography location = null;

        private static IEnumerable<Truck> _trucks = new List<Truck>
        {
            new Truck {TruckGuid = Guid.NewGuid(), CurrentCapacity = unit, MaxCapacity = unit, Location = location},
            new Truck {TruckGuid = Guid.NewGuid(), CurrentCapacity = unit, MaxCapacity = unit, Location = location},
            new Truck {TruckGuid = Guid.NewGuid(), CurrentCapacity = unit, MaxCapacity = unit, Location = location}
        };

        [TestMethod]
        public void TestGetAll_EmptyDbSetList_SizeZero()
        {
            //TruckDbSet
            Mock<ITruckDbContext> dbContext = new Mock<ITruckDbContext>();
            dbContext.Setup(x => x.Truck).Returns(new MockDbSet<Truck>());
            ITruckStore store = new TruckStore(dbContext.Object);

            Assert.AreEqual(0, store.GetAllTrucks().Count());
        }

        [TestMethod]
        public void TestGetAll_ThreeTrucks_SizeZero()
        {
            //TruckDbSet
            Mock<ITruckDbContext> dbContext = new Mock<ITruckDbContext>();
            var dbSet = new MockDbSet<Truck>();
            dbSet.AddRange(_trucks);
            dbContext.Setup(x => x.Truck).Returns(dbSet);
            ITruckStore store = new TruckStore(dbContext.Object);

            var trucks = store.GetAllTrucks();
            Assert.AreEqual(3, trucks.Count());
            Assert.IsTrue(trucks.Contains(_trucks.ElementAt(0)));
            Assert.IsTrue(trucks.Contains(_trucks.ElementAt(1)));
            Assert.IsTrue(trucks.Contains(_trucks.ElementAt(2)));
        }
    }
}
