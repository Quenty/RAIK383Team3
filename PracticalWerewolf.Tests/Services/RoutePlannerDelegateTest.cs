using Microsoft.VisualStudio.TestTools.UnitTesting;
using PracticalWerewolf.Controllers;
using PracticalWerewolf.Helpers;
using PracticalWerewolf.Models.Orders;
using PracticalWerewolf.Models.Routes;
using PracticalWerewolf.Models.Trucks;
using PracticalWerewolf.Services;
using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;

namespace PracticalWerewolf.Tests.Services
{
    [TestClass]
    public class RoutePlannerDelegateTest
    {

        private static string email = "jesseelzhang@gmail.com";

        [TestMethod]
        public void UserWithNoRoute_HomeToPickUpToDropOff()
        {
            var user = ServiceTestUtils.CreateUser(email);
            user.ContractorInfo.Truck.Location = LocationHelper.CreatePoint(43.5318683, -96.7271978);
            user.ContractorInfo.Truck.MaxCapacity = new TruckCapacityUnit { Mass = 200, Volume = 200 };
            user.ContractorInfo.Truck.UsedCapacity = new TruckCapacityUnit { Mass = 0, Volume = 0 };

            CivicAddressDb pickUpAddress = new CivicAddressDb{ RawInputAddress = "11718 Nicholas St, Omaha, NE 68154" };
            CivicAddressDb dropOffAddress = new CivicAddressDb { RawInputAddress = "5800 W 107th St, Overland Park, KS 66207" };
            TruckCapacityUnit orderSize = new TruckCapacityUnit { Mass = 90, Volume = 90 };
            Order order = ServiceTestUtils.CreateOrder(user, Models.Orders.OrderStatus.Queued, pickUpAddress, dropOffAddress, orderSize);

            List<RouteStop> route = new List<RouteStop>();
            RoutePlannerDelegate routePlanner = new RoutePlannerDelegate(user.ContractorInfo, order, route);


            routePlanner.CalculateOptimalRoute();

            Assert.IsTrue(routePlanner.WillWork);
            Assert.AreEqual(user.ContractorInfo, routePlanner.Contractor);
            Assert.IsNotNull(routePlanner.PickUp);
            Assert.AreEqual(order, routePlanner.PickUp.Order);
            Assert.AreEqual(StopType.PickUp, routePlanner.PickUp.Type);

            Assert.IsNotNull(routePlanner.DropOff);
            Assert.AreEqual(order, routePlanner.DropOff.Order);
            Assert.AreEqual(StopType.DropOff, routePlanner.DropOff.Type);

            //check that it's within 5% of the expected distance
            double epsilon = 0.05;
            int expectedMeters = 637300;
            Assert.AreEqual(expectedMeters, routePlanner.DistanceChanged, expectedMeters * epsilon);
            int metersPickUpToDropOff = 337962;
            Assert.AreEqual(metersPickUpToDropOff, routePlanner.PickUp.DistanceToNextStop, metersPickUpToDropOff * epsilon);
        }

        [TestMethod]
        public void UserWithNoRoute_AddToEndOfRoute()
        {
            var user = ServiceTestUtils.CreateUser(email);
            user.ContractorInfo.Truck.Location = LocationHelper.CreatePoint(43.5318683, -96.7271978);
            user.ContractorInfo.Truck.MaxCapacity = new TruckCapacityUnit { Mass = 200, Volume = 200 };
            user.ContractorInfo.Truck.UsedCapacity = new TruckCapacityUnit { Mass = 0, Volume = 0 };

            CivicAddressDb pickUpAddress = new CivicAddressDb { RawInputAddress = "2915 Island Home Ave, Knoxville, TN 37920" };
            CivicAddressDb dropOffAddress = new CivicAddressDb { RawInputAddress = "Walt Disney World Resort, Orlando, FL 32830" };
            TruckCapacityUnit orderSize = new TruckCapacityUnit { Mass = 90, Volume = 90 };
            Order order = ServiceTestUtils.CreateOrder(user, OrderStatus.Queued, pickUpAddress, dropOffAddress, orderSize);

            //Create Route Stops

            CivicAddressDb pickUpAddress1 = new CivicAddressDb { RawInputAddress = "1 Microsoft Way, Redmond, WA 98052" };
            CivicAddressDb dropOffAddress1 = new CivicAddressDb { RawInputAddress = "60 E South Temple # 1800, Salt Lake City, UT 84111" };
            TruckCapacityUnit orderSize1 = new TruckCapacityUnit { Mass = 90, Volume = 90 };
            Order order1 = ServiceTestUtils.CreateOrder(user, OrderStatus.InProgress, pickUpAddress1, dropOffAddress1, orderSize1);

            CivicAddressDb pickUpAddress2 = new CivicAddressDb { RawInputAddress = "60 E 3rd Ave, San Mateo, CA 94401" };
            CivicAddressDb dropOffAddress2 = new CivicAddressDb { RawInputAddress = "5800 W 107th St, Overland Park, KS 66207" };
            TruckCapacityUnit orderSize2 = new TruckCapacityUnit { Mass = 90, Volume = 90 };
            Order order2 = ServiceTestUtils.CreateOrder(user, OrderStatus.Queued, pickUpAddress2, dropOffAddress2, orderSize2);

            //PickUp Microsoft Washington
            //Pickup Roblox California
            //DropOff Mormon Church
            //DropOff Kansas

            List<RouteStop> route = new List<RouteStop>()
            {
                new RouteStop {Order = order1, Type = StopType.PickUp,  DistanceToNextStop =  1339330, EstimatedTimeToNextStop = new TimeSpan(13, 4, 0), StopOrder = 0},
                new RouteStop {Order = order2, Type = StopType.PickUp,  DistanceToNextStop =  1212579, EstimatedTimeToNextStop = new TimeSpan(11, 6, 0), StopOrder = 1},
                new RouteStop {Order = order1, Type = StopType.DropOff,  DistanceToNextStop = 1752026, EstimatedTimeToNextStop = new TimeSpan(15, 55, 0), StopOrder = 2},
                new RouteStop {Order = order2, Type = StopType.DropOff,  DistanceToNextStop = 0, EstimatedTimeToNextStop = TimeSpan.Zero, StopOrder = 3}
            };
            RoutePlannerDelegate routePlanner = new RoutePlannerDelegate(user.ContractorInfo, order, route);


            routePlanner.CalculateOptimalRoute();


            Assert.IsTrue(routePlanner.WillWork);
            Assert.AreEqual(user.ContractorInfo, routePlanner.Contractor);
            Assert.IsNotNull(routePlanner.PickUp);
            Assert.AreEqual(order, routePlanner.PickUp.Order);
            Assert.AreEqual(StopType.PickUp, routePlanner.PickUp.Type);

            Assert.IsNotNull(routePlanner.DropOff);
            Assert.AreEqual(order, routePlanner.DropOff.Order);
            Assert.AreEqual(StopType.DropOff, routePlanner.DropOff.Type);

            //check that it's within 5% of the expected distance
            double epsilon = 0.05;
            int expectedDistanceChange = 2257209;
            Assert.AreEqual(expectedDistanceChange, routePlanner.DistanceChanged, expectedDistanceChange * epsilon);

            int metersLastStopToPickUp = 1198961;
            Assert.AreEqual(metersLastStopToPickUp, routePlanner.Route[3].DistanceToNextStop, metersLastStopToPickUp * epsilon);

            int metersPickUpToDropOff = 1058948;
            Assert.AreEqual(metersPickUpToDropOff, routePlanner.PickUp.DistanceToNextStop, metersPickUpToDropOff * epsilon);
            Assert.AreEqual(0, routePlanner.DropOff.DistanceToNextStop);
        }

        [TestMethod]
        public void UserWithNoRoute_OrderThatWontFitInCar()
        {
            var user = ServiceTestUtils.CreateUser(email);
            user.ContractorInfo.Truck.Location = LocationHelper.CreatePoint(43.5318683, -96.7271978);
            user.ContractorInfo.Truck.MaxCapacity = new TruckCapacityUnit { Mass = 180, Volume = 180 };
            user.ContractorInfo.Truck.UsedCapacity = new TruckCapacityUnit { Mass = 0, Volume = 0 };

            CivicAddressDb pickUpAddress = new CivicAddressDb { RawInputAddress = "11718 Nicholas St, Omaha, NE 68154" };
            CivicAddressDb dropOffAddress = new CivicAddressDb { RawInputAddress = "5800 W 107th St, Overland Park, KS 66207" };
            TruckCapacityUnit orderSize = new TruckCapacityUnit { Mass = 200, Volume = 200 };
            Order order = ServiceTestUtils.CreateOrder(user, Models.Orders.OrderStatus.Queued, pickUpAddress, dropOffAddress, orderSize);

            List<RouteStop> route = new List<RouteStop>();
            RoutePlannerDelegate routePlanner = new RoutePlannerDelegate(user.ContractorInfo, order, route);


            routePlanner.CalculateOptimalRoute();


            Assert.IsFalse(routePlanner.WillWork);
        }


        [TestMethod]
        public void UserWithNoRoute_TruckDoesntFitTemporarily()
        {
            var user = ServiceTestUtils.CreateUser(email);
            user.ContractorInfo.Truck.Location = LocationHelper.CreatePoint(43.5318683, -96.7271978);
            user.ContractorInfo.Truck.MaxCapacity = new TruckCapacityUnit { Mass = 200, Volume = 200 };
            user.ContractorInfo.Truck.UsedCapacity = new TruckCapacityUnit { Mass = 0, Volume = 0 };

            CivicAddressDb pickUpAddress = new CivicAddressDb { RawInputAddress = "60 E 3rd Ave, San Mateo, CA 94401" };
            CivicAddressDb dropOffAddress = new CivicAddressDb { RawInputAddress = "Walt Disney World Resort, Orlando, FL 32830" };
            TruckCapacityUnit orderSize = new TruckCapacityUnit { Mass = 90, Volume = 90 };
            Order order = ServiceTestUtils.CreateOrder(user, OrderStatus.Queued, pickUpAddress, dropOffAddress, orderSize);

            //Create Route Stops

            CivicAddressDb pickUpAddress1 = new CivicAddressDb { RawInputAddress = "1 Microsoft Way, Redmond, WA 98052" };
            CivicAddressDb dropOffAddress1 = new CivicAddressDb { RawInputAddress = "5800 W 107th St, Overland Park, KS 66207" };
            TruckCapacityUnit orderSize1 = new TruckCapacityUnit { Mass = 90, Volume = 90 };
            Order order1 = ServiceTestUtils.CreateOrder(user, OrderStatus.InProgress, pickUpAddress1, dropOffAddress1, orderSize1);

            //60 E 3rd Ave, San Mateo, CA 94401
            CivicAddressDb pickUpAddress2 = new CivicAddressDb { RawInputAddress = "60 E South Temple # 1800, Salt Lake City, UT 84111" };
            CivicAddressDb dropOffAddress2 = new CivicAddressDb { RawInputAddress = "2915 Island Home Ave, Knoxville, TN 37920" };
            TruckCapacityUnit orderSize2 = new TruckCapacityUnit { Mass = 90, Volume = 90 };
            Order order2 = ServiceTestUtils.CreateOrder(user, OrderStatus.Queued, pickUpAddress2, dropOffAddress2, orderSize2);

            //PickUp Microsoft Washington
            //Pickup Mormon Church
            //DropOff Kansas
            //DropOff Tennesee


            //total = 4278517
            List<RouteStop> route = new List<RouteStop>()
            {
                new RouteStop {Order = order1, Type = StopType.PickUp,  DistanceToNextStop = 1328280, EstimatedTimeToNextStop = new TimeSpan(12, 35, 0), StopOrder = 0},
                new RouteStop {Order = order2, Type = StopType.PickUp,  DistanceToNextStop = 1752026, EstimatedTimeToNextStop = new TimeSpan(15, 55, 0), StopOrder = 1},
                new RouteStop {Order = order1, Type = StopType.DropOff,  DistanceToNextStop = 1198211, EstimatedTimeToNextStop = new TimeSpan(10, 49, 0), StopOrder = 2},
                new RouteStop {Order = order2, Type = StopType.DropOff,  DistanceToNextStop = 0, EstimatedTimeToNextStop = TimeSpan.Zero, StopOrder = 3}
            };
            RoutePlannerDelegate routePlanner = new RoutePlannerDelegate(user.ContractorInfo, order, route);


            routePlanner.CalculateOptimalRoute();


            Assert.IsTrue(routePlanner.WillWork);
            Assert.AreEqual(user.ContractorInfo, routePlanner.Contractor);
            Assert.IsNotNull(routePlanner.PickUp);
            Assert.AreEqual(order, routePlanner.PickUp.Order);
            Assert.AreEqual(StopType.PickUp, routePlanner.PickUp.Type);

            Assert.IsNotNull(routePlanner.DropOff);
            Assert.AreEqual(order, routePlanner.DropOff.Order);
            Assert.AreEqual(StopType.DropOff, routePlanner.DropOff.Type);

            Assert.AreEqual(3, routePlanner.Route.IndexOf(routePlanner.PickUp));
            Assert.AreEqual(5, routePlanner.Route.IndexOf(routePlanner.DropOff));


            //check that it's within 5% of the expected distance
            IEnumerable<int> expectedDistances = new List<int>() { 1328280, 1752026, 2958015, 3979330, 1059572, 0 };

            double epsilon = 0.05;
            int expectedDistanceChange = 6798706;
            Assert.AreEqual(expectedDistanceChange, routePlanner.DistanceChanged, expectedDistanceChange * epsilon);

            for(int i = 0; i < expectedDistances.Count(); i++)
            {
                Assert.AreEqual(expectedDistances.ElementAt(i), routePlanner.Route.ElementAt(i).DistanceToNextStop, expectedDistances.ElementAt(i) * epsilon);
            }
        }


        [TestMethod]
        public void UserWithNoRoute_AddBetweenSameStop()
        {
            var user = ServiceTestUtils.CreateUser(email);
            user.ContractorInfo.Truck.Location = LocationHelper.CreatePoint(43.5318683, -96.7271978);
            user.ContractorInfo.Truck.MaxCapacity = new TruckCapacityUnit { Mass = 200, Volume = 200 };
            user.ContractorInfo.Truck.UsedCapacity = new TruckCapacityUnit { Mass = 0, Volume = 0 };

            CivicAddressDb pickUpAddress = new CivicAddressDb { RawInputAddress = "60 E South Temple # 1800, Salt Lake City, UT 84111" }; 
            CivicAddressDb dropOffAddress = new CivicAddressDb { RawInputAddress = "5800 W 107th St, Overland Park, KS 66207" }; 
            TruckCapacityUnit orderSize = new TruckCapacityUnit { Mass = 90, Volume = 90 };
            Order order = ServiceTestUtils.CreateOrder(user, OrderStatus.Queued, pickUpAddress, dropOffAddress, orderSize);

            //Create Route Stops

            CivicAddressDb pickUpAddress1 = new CivicAddressDb { RawInputAddress = "1 Microsoft Way, Redmond, WA 98052" };
            CivicAddressDb dropOffAddress1 = new CivicAddressDb { RawInputAddress = "2915 Island Home Ave, Knoxville, TN 37920" };
            TruckCapacityUnit orderSize1 = new TruckCapacityUnit { Mass = 45, Volume = 45 };
            Order order1 = ServiceTestUtils.CreateOrder(user, OrderStatus.InProgress, pickUpAddress1, dropOffAddress1, orderSize1);

            CivicAddressDb pickUpAddress2 = new CivicAddressDb { RawInputAddress = "60 E 3rd Ave, San Mateo, CA 94401" };
            CivicAddressDb dropOffAddress2 = new CivicAddressDb { RawInputAddress = "Walt Disney World Resort, Orlando, FL 32830" };
            TruckCapacityUnit orderSize2 = new TruckCapacityUnit { Mass = 45, Volume = 45 };
            Order order2 = ServiceTestUtils.CreateOrder(user, OrderStatus.Queued, pickUpAddress2, dropOffAddress2, orderSize2);

            //PickUp Microsoft Washington
            //Pickup Roblox California
            //DropOff Tennessee
            //DropOff Disney

            List<RouteStop> route = new List<RouteStop>()
            {
                new RouteStop {Order = order1, Type = StopType.PickUp,  DistanceToNextStop = 1339330, EstimatedTimeToNextStop = new TimeSpan(13, 4, 0), StopOrder = 0},
                new RouteStop {Order = order2, Type = StopType.PickUp,  DistanceToNextStop = 3980202, EstimatedTimeToNextStop = new TimeSpan(1, 12, 6, 0), StopOrder = 1},
                new RouteStop {Order = order1, Type = StopType.DropOff,  DistanceToNextStop = 1059572, EstimatedTimeToNextStop = new TimeSpan(9, 20, 0), StopOrder = 2},
                new RouteStop {Order = order2, Type = StopType.DropOff,  DistanceToNextStop = 0, EstimatedTimeToNextStop = TimeSpan.Zero, StopOrder = 3}
            };
            RoutePlannerDelegate routePlanner = new RoutePlannerDelegate(user.ContractorInfo, order, route);


            routePlanner.CalculateOptimalRoute();


            Assert.IsTrue(routePlanner.WillWork);
            Assert.AreEqual(user.ContractorInfo, routePlanner.Contractor);
            Assert.IsNotNull(routePlanner.PickUp);
            Assert.AreEqual(order, routePlanner.PickUp.Order);
            Assert.AreEqual(StopType.PickUp, routePlanner.PickUp.Type);

            Assert.IsNotNull(routePlanner.DropOff);
            Assert.AreEqual(order, routePlanner.DropOff.Order);
            Assert.AreEqual(StopType.DropOff, routePlanner.DropOff.Type);

            //check that it's within 5% of the expected distance
            IEnumerable<int> expectedDistances = new List<int>() { 1339330, 1212579, 1752026, 1198961, 1059572, 0 };

            double epsilon = 0.05;
            int expectedDistanceChange = 183364;
            Assert.AreEqual(expectedDistanceChange, routePlanner.DistanceChanged, expectedDistanceChange * epsilon);

            for (int i = 0; i < expectedDistances.Count(); i++)
            {
                Assert.AreEqual(expectedDistances.ElementAt(i), routePlanner.Route.ElementAt(i).DistanceToNextStop, expectedDistances.ElementAt(i) * epsilon);
            }
        }
    }
}
