using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GoogleMapsApi.Entities.Directions.Response;
using System.Linq;
using System.Device.Location;
using PracticalWerewolf.Helpers;

namespace PracticalWerewolf.Tests.Helpers
{
    [TestClass]
    public class LocationHelperTest
    {
        [TestMethod]
        public void GetRouteBetweenLocations_TestDistanceBewteenJesseesHomes()
        {
            string origin = "3025 North 169th Avenue, Omaha, NE 68116";
            string destination = "630 North 14th Street, Kauffman Hall, Lincoln, NE 68508";

            DirectionsResponse result = LocationHelper.GetRouteBetweenLocations(origin, destination);

            if(result.Status != DirectionsStatusCodes.OVER_QUERY_LIMIT)
            {
                Assert.AreEqual(DirectionsStatusCodes.OK, result.Status);
                Assert.AreEqual("53.1 mi", result.Routes.ElementAt(0).Legs.ElementAt(0).Distance.Text);
                Assert.AreEqual(85456, result.Routes.ElementAt(0).Legs.ElementAt(0).Distance.Value, 100);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetRouteBetweenLocations_NullArgument_TestDistanceBewteenJesseesHomes()
        {
            string origin = null;
            string destination = "630 North 14th Street, Kauffman Hall, Lincoln, NE 68508";

            DirectionsResponse result = LocationHelper.GetRouteBetweenLocations(origin, destination);

            Assert.Fail();
        }

        [TestMethod]
        public void GetRouteBetweenLocations_CivicAddressDb_TestDistanceBewteenJesseesHomes()
        {
            CivicAddressDb origin = new CivicAddressDb()
            {
                RawInputAddress = "3025 North 169th Avenue, Omaha, NE 68116",
                StreetNumber = "3025 North 169th Avenue",
                City = "Omaha",
                State = "NE",
                ZipCode = "68116",
                Country = "USA"
            };

            CivicAddressDb destination = new CivicAddressDb()
            {
                RawInputAddress = "630 North 14th Street, Kauffman Hall, Lincoln, NE 68508",
                StreetNumber = "630 North 14th Street, Kauffman Hall",
                City = "Lincoln",
                State = "NE",
                ZipCode = "68508",
                Country = "USA"
            };
            DirectionsResponse result = LocationHelper.GetRouteBetweenLocations(origin, destination);

            if (result.Status != DirectionsStatusCodes.OVER_QUERY_LIMIT)
            {
                Assert.AreEqual(DirectionsStatusCodes.OK, result.Status);
                Assert.AreEqual("53.1 mi", result.Routes.ElementAt(0).Legs.ElementAt(0).Distance.Text);
                Assert.AreEqual(85456, result.Routes.ElementAt(0).Legs.ElementAt(0).Distance.Value, 100);
            }
        }

        [TestMethod]
        public void GetRouteBetweenLocations_CivicAddressDbNoRawInputAddress_TestDistanceBewteenJesseesHomes()
        {
            CivicAddressDb origin = new CivicAddressDb()
            {
                StreetNumber = "3025 North 169th Avenue",
                City = "Omaha",
                State = "NE",
                ZipCode = "68116",
                Country = "USA"
            };

            CivicAddressDb destination = new CivicAddressDb()
            {
                StreetNumber = "630 North 14th Street, Kauffman Hall",
                City = "Lincoln",
                State = "NE",
                ZipCode = "68508",
                Country = "USA"
            };
            DirectionsResponse result = LocationHelper.GetRouteBetweenLocations(origin, destination);

            if (result.Status != DirectionsStatusCodes.OVER_QUERY_LIMIT)
            {
                Assert.AreEqual(DirectionsStatusCodes.OK, result.Status);
                Assert.AreEqual("53.1 mi", result.Routes.ElementAt(0).Legs.ElementAt(0).Distance.Text);
                Assert.AreEqual(85456, result.Routes.ElementAt(0).Legs.ElementAt(0).Distance.Value, 100);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetRouteBetweenLocations_NullCivicAddressDb_TestDistanceBewteenJesseesHomes()
        {
            CivicAddressDb origin = new CivicAddressDb()
            {
                RawInputAddress = "3025 North 169th Avenue, Omaha, NE 68116",
                StreetNumber = "3025 North 169th Avenue",
                City = "Omaha",
                State = "NE",
                ZipCode = "68116",
                Country = "USA"
            };

            CivicAddressDb destination = null;

            DirectionsResponse result = LocationHelper.GetRouteBetweenLocations(origin, destination);

            Assert.Fail();
        }
    }
}
