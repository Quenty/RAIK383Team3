using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using PracticalWerewolf.Models.Trucks;
using System.Linq;
using Moq;
using PracticalWerewolf.Tests.Stores.DbContext;
using PracticalWerewolf.Stores.Interfaces;
using PracticalWerewolf.Stores;
using PracticalWerewolf.Models;
using System.Data.Entity.Spatial;
using PracticalWerewolf.Application;
using System.Data.Entity;

namespace PracticalWerewolf.Tests.Stores
{
    [TestClass]
    public class TruckStoreTest
    {
        private static TruckCapacityUnit unit = new TruckCapacityUnit { TruckCapacityUnitGuid = Guid.NewGuid() };
        private static DbGeography location = null;

        private static IEnumerable<Truck> _trucks = new List<Truck>
        {
            new Truck {TruckGuid = Guid.NewGuid(), UsedCapacity = unit, MaxCapacity = unit, Location = location, LicenseNumber = "Hergedy1"},
            new Truck {TruckGuid = Guid.NewGuid(), UsedCapacity = unit, MaxCapacity = unit, Location = location, LicenseNumber = "Hergedy2"},
            new Truck {TruckGuid = Guid.NewGuid(), UsedCapacity = unit, MaxCapacity = unit, Location = location, LicenseNumber = "Hergedy3"}
        };

        [TestMethod]
        public void GetAll_EmptyDbSetList_SizeZero()
        {
            var store = GetTruckStoreWithDbSet(new MockTruckDbSet());

            Assert.AreEqual(0, store.GetAllTrucks().Count());
        }

        [TestMethod]
        public void GetAll_ThreeTrucks_SizeZero()
        {
            var dbSet = new MockTruckDbSet();
            dbSet.AddRange(_trucks);
            var store = GetTruckStoreWithDbSet(dbSet);

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
            var dbSet = new MockTruckDbSet();
            dbSet.AddRange(_trucks);
            var store = GetTruckStoreWithDbSet(dbSet);

            var truck = store.Get(chosenGuid);

            Assert.IsNotNull(truck);
            Assert.AreEqual(_trucks.ElementAt(0), truck);
        }

        [TestMethod]
        public void Get_NoMatchingTrucks_ReturnTruck()
        {
            var chosenGuid = Guid.NewGuid();
            var dbSet = new MockTruckDbSet();
            dbSet.AddRange(_trucks);
            var store = GetTruckStoreWithDbSet(dbSet);

            var truck = store.Get(chosenGuid);

            Assert.IsNull(truck);
        }

        [TestMethod]
        public void Get_MultipleMatchingTrucks_ReturnTruck()
        {
            var chosenGuid = _trucks.ElementAt(0).TruckGuid;
            var dbSet = new MockTruckDbSet();
            dbSet.AddRange(_trucks);
            dbSet.Add(new Truck() { TruckGuid = chosenGuid, Location = location, UsedCapacity = unit, MaxCapacity = unit, LicenseNumber = "James" });
            var store = GetTruckStoreWithDbSet(dbSet);

            var truck = store.Get(chosenGuid);

            Assert.IsNotNull(truck);
            Assert.AreEqual(_trucks.ElementAt(0), truck);
        }

        [TestMethod]
        public void Update_ValidTruck_UpdatesTruck()
        {
            var guid = Guid.NewGuid();
            var capacity = new TruckCapacityUnit() { Mass = 12, Volume = 12, TruckCapacityUnitGuid = Guid.NewGuid() };
            var capacityGuid = Guid.NewGuid();
            var truck = new Truck() { TruckGuid = guid, UsedCapacity = capacity, MaxCapacity = capacity, LicenseNumber = "Abbie"};
            var dbSet = new MockTruckDbSet();
            dbSet.Add(truck);
            var store = GetTruckStoreWithDbSet(dbSet);

            var newCapacity = new TruckCapacityUnit() { Mass = 24, Volume = 24, TruckCapacityUnitGuid = capacityGuid};
            truck = store.Get(guid);
            truck.MaxCapacity = newCapacity;
            truck.UsedCapacity = newCapacity;
            truck.LicenseNumber = "Matt";
            store.Update(truck);

            var result = store.Get(guid);
            Assert.AreEqual(guid, result.TruckGuid);
            Assert.AreEqual(newCapacity, result.MaxCapacity);
            Assert.AreEqual(newCapacity, result.UsedCapacity);
            Assert.AreEqual("Matt", result.LicenseNumber);
        }

        [TestMethod]
        public void Update_InvalidTruck_NoChangesMade()
        {
            var guid = Guid.NewGuid();
            var capacity = new TruckCapacityUnit() { Mass = 12, Volume = 12, TruckCapacityUnitGuid = Guid.NewGuid() };
            var capacityGuid = Guid.NewGuid();
            var truck = new Truck() { TruckGuid = guid, UsedCapacity = capacity, MaxCapacity = capacity,  LicenseNumber = "Jessee"};
            var dbSet = new MockTruckDbSet();
            dbSet.Add(truck);
            var store = GetTruckStoreWithDbSet(dbSet);

            var newCapacity = new TruckCapacityUnit() { Mass = 24, Volume = 24, TruckCapacityUnitGuid = capacityGuid };
            var newTruck = new Truck() { TruckGuid = Guid.NewGuid(), UsedCapacity = newCapacity, MaxCapacity = newCapacity,  LicenseNumber = "Not James"};
            store.Update(newTruck);

            var result = store.Get(guid);
            Assert.AreEqual(guid, result.TruckGuid);
            Assert.AreEqual(capacity, result.MaxCapacity);
            Assert.AreEqual(capacity, result.UsedCapacity);
            Assert.AreEqual("Jessee", result.LicenseNumber);
        }

        [TestMethod, ExpectedException(typeof(ArgumentNullException))]
        public void Update_NullTruck_ThrowsException()
        {
            
            var dbSet = new MockTruckDbSet();
            var store = GetTruckStoreWithDbSet(dbSet);

            Truck truck = null;
            store.Update(truck);
        }

        [TestMethod]
        public void Create_ValidTruck_AddsToDbSet()
        {
            var dbSet = new MockTruckDbSet();
            var store = GetTruckStoreWithDbSet(dbSet);

            var guid = Guid.NewGuid();
            var capacity = new TruckCapacityUnit() { Mass = 12, Volume = 12, TruckCapacityUnitGuid = Guid.NewGuid() };
            var capacityGuid = Guid.NewGuid();
            var truck = new Truck() { TruckGuid = guid, UsedCapacity = capacity, MaxCapacity = capacity, LicenseNumber =  "Cooper"};
            store.Create(truck);

            var result = store.Get(guid);
            Assert.AreEqual(truck, result);
        }

        [TestMethod, ExpectedException(typeof(ArgumentNullException))]
        public void Create_NullTruck_ThrowsException()
        {
            var dbSet = new MockTruckDbSet();
            var store = GetTruckStoreWithDbSet(dbSet);

            Truck truck = null;
            store.Create(truck);
        }





        public static ITruckStore GetTruckStoreWithDbSet(DbSet<Truck> dbSet)
        {
            var dbContext = new Mock<IDbSetFactory>();
            dbContext.Setup(x => x.CreateDbSet<Truck>()).Returns(dbSet);
            ITruckStore store = new TruckStore(dbContext.Object);

            return store;
        }

    }
}
