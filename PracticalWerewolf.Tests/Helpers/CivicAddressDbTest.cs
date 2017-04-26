using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PracticalWerewolf.Tests.Helpers
{
    [TestClass]
    public class CivicAddressDbTest
    {
        private CivicAddressDb AddressA;
        private CivicAddressDb AddressADifferentGuid;
        private CivicAddressDb AddressB;
        private CivicAddressDb AddressBDifferentGuid;

        public CivicAddressDbTest()
        {
            AddressA = new CivicAddressDb()
            {
                Route = "3025",
                State = "NE",
                ZipCode = "68116",
                StreetNumber = "North 169th Avenue",
                City = "Omaha",
                Country = "USA",
                CivicAddressGuid = Guid.NewGuid(),
                RawInputAddress = "3025 North 169th Avenue, Omaha, NE 68116"
            };
            AddressADifferentGuid =  new CivicAddressDb(AddressA)
            {
                CivicAddressGuid = Guid.NewGuid()
            };
            AddressB = new CivicAddressDb()
            {
                Route = "630 Kauffman Hall",
                State = "NE",
                City = "Lincoln",
                ZipCode = "68508",
                StreetNumber = "North 14th Street",
                Country = "USA",
                CivicAddressGuid = Guid.NewGuid(),
                RawInputAddress = "630 North 14th Street, Kauffman Hall, Lincoln, NE 68508"
            };
            AddressBDifferentGuid = new CivicAddressDb(AddressB)
            {
                CivicAddressGuid = Guid.NewGuid()
            };
        }


        [TestMethod]
        public void TestInequality()
        {
            Assert.IsFalse(AddressA != AddressA);
            Assert.IsFalse(AddressB != AddressB);
            Assert.IsFalse(AddressB != AddressBDifferentGuid);
            Assert.IsFalse(AddressA != AddressADifferentGuid);
            Assert.IsTrue(AddressA != AddressB);
            Assert.IsTrue(AddressA != AddressBDifferentGuid);
            Assert.IsTrue(AddressADifferentGuid != AddressB);
            Assert.IsTrue(AddressADifferentGuid != AddressBDifferentGuid);
        }



        [TestMethod]
        public void GetHashCode_TestHash()
        {
            ISet<CivicAddressDb> set = new HashSet<CivicAddressDb>();
            set.Add(AddressA);

            Assert.IsTrue(set.Contains(AddressA));
            Assert.IsFalse(set.Contains(AddressB));
            Assert.AreEqual(set.Count(), 1);
            Assert.AreEqual(set.First(), AddressA);

            for (int i=0; i < 5; i++)
            {
                CivicAddressDb copy = new CivicAddressDb(AddressA);
                set.Add(copy);
            }

            Assert.IsTrue(set.Contains(AddressA));
            Assert.IsFalse(set.Contains(AddressB));
            Assert.AreEqual(set.Count(), 1);
            Assert.AreEqual(set.First(), AddressA);

            for (int i = 0; i < 5; i++)
            {
                CivicAddressDb copy = new CivicAddressDb(AddressADifferentGuid);
                set.Add(copy);
            }

            Assert.IsTrue(set.Contains(AddressA));
            Assert.IsFalse(set.Contains(AddressB));
            Assert.AreEqual(set.Count(), 1);
            Assert.AreEqual(set.First(), AddressA);

            set.Add(AddressB);
            Assert.IsTrue(set.Contains(AddressB));
            Assert.IsTrue(set.Contains(AddressB));
            Assert.AreEqual(set.Count(), 2);

            for (int i = 0; i < 5; i++)
            {
                CivicAddressDb copy = new CivicAddressDb(AddressB);
                set.Add(copy);
            }

            Assert.IsTrue(set.Contains(AddressB));
            Assert.IsTrue(set.Contains(AddressB));
            Assert.AreEqual(set.Count(), 2);

            for (int i = 0; i < 5; i++)
            {
                CivicAddressDb copy = new CivicAddressDb(AddressBDifferentGuid);
                set.Add(copy);
            }

            Assert.IsTrue(set.Contains(AddressB));
            Assert.IsTrue(set.Contains(AddressB));
            Assert.AreEqual(set.Count(), 2);
        }

        [TestMethod]
        public void GetHashCode_TestEquality()
        {
            Assert.AreEqual(AddressA, AddressA);
            Assert.IsTrue(AddressA == AddressADifferentGuid);
            Assert.IsTrue(AddressA.GetHashCode() == AddressA.GetHashCode());
            Assert.IsTrue(AddressA.Equals(AddressA));
        }

        [TestMethod]
        public void CopyConstructor_Test()
        {
            CivicAddressDb copy = new CivicAddressDb(AddressA);
            Assert.IsTrue(AddressA.Equals(copy));
            Assert.AreEqual(AddressA, AddressADifferentGuid);
            Assert.AreNotEqual(AddressA, AddressBDifferentGuid);
        }
    }
}
