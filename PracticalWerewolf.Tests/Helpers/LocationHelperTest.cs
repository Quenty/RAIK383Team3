using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GoogleMapsApi.Entities.Directions.Response;
using System.Linq;
using System.Device.Location;
using PracticalWerewolf.Helpers;
using PracticalWerewolf.Helpers.Interfaces;

namespace PracticalWerewolf.Tests.Helpers
{
    [TestClass]
    public class LocationHelperTest
    {
        private ILocationHelper locationHelper = new LocationHelper();

        private CivicAddressDb origin = new CivicAddressDb
        {
            RawInputAddress = "3025 North 169th Avenue, Omaha, NE 68116",
            Route = "3025",
            StreetNumber = "North 169th Avenue",
            City = "Omaha",
            State = "NE",
            ZipCode = "68116",
            Country = "USA"
        };

        CivicAddressDb destination = new CivicAddressDb()
        {
            RawInputAddress = "630 North 14th Street, Kauffman Hall, Lincoln, NE 68508",
            Route = "630",
            StreetNumber = "North 14th Street, Kauffman Hall",
            City = "Lincoln",
            State = "NE",
            ZipCode = "68508",
            Country = "USA"
        };

        private int expectedDistance = 83642;
        private TimeSpan expectedDuration = new TimeSpan(0, 58, 0);
        private double epsilon = 0.1;


        [TestMethod]
        public void GetRouteBetweenLocations_TestDistanceBewteenJesseesHomes()
        {
            DirectionsResult result = null;

            try
            {
                result = locationHelper.GetDirections(origin, destination);
            }
            catch (Exception e)
            {
                Assert.Inconclusive(e.Message);
            }

            Assert.AreEqual(origin, result.Origin);
            Assert.AreEqual(destination, result.Destination);
            Assert.AreEqual(expectedDistance, result.Distance, expectedDistance * epsilon);
            Assert.AreEqual(expectedDuration.Ticks, result.Duration.Ticks, expectedDuration.Ticks * epsilon);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetRouteBetweenLocations_NullArgument_TestDistanceBewteenJesseesHomes()
        {
            CivicAddressDb nullOrigin = null;

            DirectionsResult result = locationHelper.GetDirections(nullOrigin, destination);

            Assert.Fail();
        }
    }
}
