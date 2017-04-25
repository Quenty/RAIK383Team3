using Microsoft.VisualStudio.TestTools.UnitTesting;
using PracticalWerewolf.Models.Trucks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PracticalWerewolf.Tests
{
    [TestClass]
    public class TruckCapacityUnitTests
    {
        private TruckCapacityUnit Zero;
        private TruckCapacityUnit ZeroDifferentGuid;
        private TruckCapacityUnit OneMass;
        private TruckCapacityUnit OneVolume;
        private TruckCapacityUnit One;
        private TruckCapacityUnit TwoMass;
        private TruckCapacityUnit TwoVolume;
        private TruckCapacityUnit Two;
        private TruckCapacityUnit OneHundredMass;
        private TruckCapacityUnit OneHundredVolume;
        private TruckCapacityUnit OneHundred;

        public TruckCapacityUnitTests()
        {
            Zero = new TruckCapacityUnit()
            {
                Mass = 0,
                Volume = 0,
                TruckCapacityUnitGuid = Guid.NewGuid()
            };

            ZeroDifferentGuid = new TruckCapacityUnit()
            {
                Mass = 0,
                Volume = 0,
                TruckCapacityUnitGuid = Guid.NewGuid()
            };

            One = new TruckCapacityUnit()
            {
                Mass = 1,
                Volume = 1,
                TruckCapacityUnitGuid = Guid.NewGuid()
            };

            OneMass = new TruckCapacityUnit()
            {
                Mass = 1,
                Volume = 0,
                TruckCapacityUnitGuid = Guid.NewGuid()
            };

            OneVolume = new TruckCapacityUnit()
            {
                Mass = 0,
                Volume = 1,
                TruckCapacityUnitGuid = Guid.NewGuid()
            };

            Two = new TruckCapacityUnit()
            {
                Mass = 2,
                Volume = 2,
                TruckCapacityUnitGuid = Guid.NewGuid()
            };

            TwoMass = new TruckCapacityUnit()
            {
                Mass = 2,
                Volume = 0,
                TruckCapacityUnitGuid = Guid.NewGuid()
            };

            TwoVolume = new TruckCapacityUnit()
            {
                Mass = 0,
                Volume = 2,
                TruckCapacityUnitGuid = Guid.NewGuid()
            };

            OneHundred = new TruckCapacityUnit()
            {
                Mass = 100,
                Volume = 100,
                TruckCapacityUnitGuid = Guid.NewGuid()
            };

            OneHundredMass = new TruckCapacityUnit()
            {
                Mass = 100,
                Volume = 0,
                TruckCapacityUnitGuid = Guid.NewGuid()
            };

            OneHundredVolume = new TruckCapacityUnit()
            {
                Mass = 0,
                Volume = 100,
                TruckCapacityUnitGuid = Guid.NewGuid()
            };
        }

        [TestMethod]
        public void Test_FitsIn()
        {
            Assert.IsTrue(Zero.FitsIn(Zero));
            Assert.IsTrue(Zero.FitsIn(One));
            Assert.IsTrue(Zero.FitsIn(OneMass));
            Assert.IsTrue(Zero.FitsIn(OneVolume));

            Assert.IsTrue(OneMass.FitsIn(One));
            Assert.IsTrue(OneMass.FitsIn(OneMass));
            Assert.IsFalse(OneMass.FitsIn(OneVolume));

            Assert.IsTrue(OneVolume.FitsIn(One));
            Assert.IsFalse(OneVolume.FitsIn(OneMass));
            Assert.IsTrue(OneVolume.FitsIn(OneVolume));

            Assert.IsTrue(One.FitsIn(One));
            Assert.IsFalse(One.FitsIn(OneMass));
            Assert.IsFalse(One.FitsIn(OneVolume));

            Assert.IsTrue(One.FitsIn(OneHundred));
            Assert.IsTrue(Two.FitsIn(OneHundred));

            Assert.IsFalse(OneHundred.FitsIn(Zero));
            Assert.IsFalse(OneHundred.FitsIn(OneMass));
            Assert.IsFalse(OneHundred.FitsIn(OneVolume));
        }

        [TestMethod]
        public void TestEquality()
        {
            Assert.Equals(Zero, Zero);
            Assert.Equals(Zero, ZeroDifferentGuid);

            Assert.AreNotEqual(OneVolume, OneMass);
            Assert.AreEqual(OneVolume, OneVolume);
            Assert.AreEqual(Two, Two);
        }

        [TestMethod]
        public void TestEquality_Override()
        {
            Assert.IsTrue(Zero == Zero);
            Assert.IsTrue(Zero == ZeroDifferentGuid);

            Assert.IsFalse(Zero != Zero);
            Assert.IsFalse(Zero != ZeroDifferentGuid);
            
            Assert.AreNotEqual(OneVolume, OneMass);
            Assert.AreEqual(OneVolume, OneVolume);
            Assert.AreEqual(Two, Two);
        }

        [TestMethod]
        public void TestAddition()
        {
            Assert.Equals(One + One, Two);
            Assert.Equals(OneMass + OneVolume, Two);

            foreach (var item in new List<TruckCapacityUnit>() { Zero, One, OneMass, OneVolume, Two, TwoMass, TwoVolume, OneHundred, OneHundredMass, OneHundredVolume })
            {
                Assert.Equals(item + Zero, item);
                Assert.Equals(Zero + item, item);
                Assert.AreNotEqual(item + One, item);
                Assert.AreNotEqual(One + item, item);
            }
        }


        [TestMethod]
        public void TestSubtraction()
        {
            Assert.Equals(One - OneVolume, OneMass);
            Assert.Equals(One - OneMass, OneVolume);
            Assert.AreNotEqual(One - OneHundred, OneMass);
            Assert.AreNotEqual(One - OneHundred, OneHundred);
            Assert.AreNotEqual(One - OneHundred, One);

            foreach (var item in new List<TruckCapacityUnit>() { Zero, One, OneMass, OneVolume, Two, TwoMass, TwoVolume, OneHundred, OneHundredMass, OneHundredVolume })
            {
                Assert.Equals(item - Zero, item);
                Assert.AreNotEqual(item - One, item);
            }
        }
    }
}
