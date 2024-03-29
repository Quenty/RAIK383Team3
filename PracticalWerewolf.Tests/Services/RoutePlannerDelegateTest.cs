﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using PracticalWerewolf.Helpers;
using PracticalWerewolf.Helpers.Interfaces;
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
        private static ILocationHelper locationHelper = new LocationHelper();
        private static string email = "jesseelzhang@gmail.com";

        private static CivicAddressDb home = new CivicAddressDb
        {
            RawInputAddress = "1504 S 1st Ave Sioux Falls, SD 57105",
            StreetNumber = "1504",
            Route = "S 1st Ave",
            City = "Sioux Falls",
            State = "SD",
            ZipCode = "57105",
            Country = "US"
        };

        private static CivicAddressDb buildertrend = new CivicAddressDb
        {
            RawInputAddress = "11718 Nicholas St, Omaha, NE 68154",
            StreetNumber = "11718",
            Route = "Nicholas St",
            City = "Omaha",
            State = "NE",
            ZipCode = "68154",
            Country = "US"
        };

        private static CivicAddressDb sms = new CivicAddressDb
        {
            RawInputAddress = "5800 W 107th St, Overland Park, KS 66207",
            StreetNumber = "5800",
            Route = "W 107th St",
            City = "Overland Park",
            State = "KS",
            ZipCode = "66207",
            Country = "US"
        };

        private static CivicAddressDb knoxville = new CivicAddressDb
        {
            RawInputAddress = "2915 Island Home Ave, Knoxville, TN 37920",
            StreetNumber = "2915",
            Route = "Island Home Ave",
            City = "Knoxville",
            State = "Tn",
            ZipCode = "37920",
            Country = "US"
        };

        private static CivicAddressDb disney = new CivicAddressDb
        {
            RawInputAddress = "Walt Disney World Resort, Orlando, FL 32830",
            Route = "1",
            StreetNumber = "Walt Disney World Resort",
            City = "Orlando",
            State = "FL",
            ZipCode = "32830",
            Country = "US"
        };

        private static CivicAddressDb microsoft = new CivicAddressDb
        {
            RawInputAddress = "1 Microsoft Way, Redmond, WA 98052",
            StreetNumber = "1",
            Route = "Microsoft Way",
            City = "Redmond",
            State = "WA",
            ZipCode = "98052",
            Country = "US"
        };

        private static CivicAddressDb roblox = new CivicAddressDb
        {
            RawInputAddress = "60 E 3rd Ave, San Mateo, CA 94401",
            StreetNumber = "60",
            Route = "E 3rd Ave",
            City = "San Mateo",
            State = "CA",
            ZipCode = "94401",
            Country = "US"
        };

        private static CivicAddressDb mormonChurch = new CivicAddressDb
        {
            RawInputAddress = "241 N 300 W, Salt Lake City, UT 84103",
            StreetNumber = "141",
            Route = "N 300 W",
            City = "Salt Lake City",
            State = "UT",
            ZipCode = "84103",
            Country = "US"
        };

        [TestMethod]
        public void UserWithNoRoute_HomeToPickUpToDropOff()
        {
            var user = ServiceTestUtils.CreateUser(email);
            user.ContractorInfo.HomeAddress = home;
            user.ContractorInfo.Truck.Location = LocationHelper.CreatePoint(43.5318683, -96.7271978);
            user.ContractorInfo.Truck.MaxCapacity = new TruckCapacityUnit { Mass = 200, Volume = 200 };
            user.ContractorInfo.Truck.UsedCapacity = new TruckCapacityUnit { Mass = 0, Volume = 0 };

            CivicAddressDb pickUpAddress = buildertrend;
            CivicAddressDb dropOffAddress = sms;
            TruckCapacityUnit orderSize = new TruckCapacityUnit { Mass = 90, Volume = 90 };
            Order order = ServiceTestUtils.CreateOrder(user, Models.Orders.OrderStatus.Queued, pickUpAddress, dropOffAddress, orderSize);

            List<RouteStop> route = new List<RouteStop>();
            ContractorRoutePlanner routePlanner = new ContractorRoutePlanner(user.ContractorInfo, order, route, locationHelper);


            routePlanner.CalculateOptimalRoute();
            locationHelper.Refresh();


            Assert.IsTrue(routePlanner.WillWork);
            Assert.AreEqual(user.ContractorInfo, routePlanner.Contractor);
            Assert.IsNotNull(routePlanner.PickUp);
            Assert.AreEqual(StopType.PickUp, routePlanner.PickUp.Type);

            Assert.IsNotNull(routePlanner.DropOff);
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
            user.ContractorInfo.HomeAddress = home;
            user.ContractorInfo.Truck.Location = LocationHelper.CreatePoint(43.5318683, -96.7271978);
            user.ContractorInfo.Truck.MaxCapacity = new TruckCapacityUnit { Mass = 200, Volume = 200 };
            user.ContractorInfo.Truck.UsedCapacity = new TruckCapacityUnit { Mass = 0, Volume = 0 };

            CivicAddressDb pickUpAddress = knoxville;
            CivicAddressDb dropOffAddress = disney;
            TruckCapacityUnit orderSize = new TruckCapacityUnit { Mass = 90, Volume = 90 };
            Order order = ServiceTestUtils.CreateOrder(user, OrderStatus.Queued, pickUpAddress, dropOffAddress, orderSize);

            //Create Route Stops

            CivicAddressDb pickUpAddress1 = microsoft;
            CivicAddressDb dropOffAddress1 = mormonChurch;
            TruckCapacityUnit orderSize1 = new TruckCapacityUnit { Mass = 90, Volume = 90 };
            Order order1 = ServiceTestUtils.CreateOrder(user, OrderStatus.InProgress, pickUpAddress1, dropOffAddress1, orderSize1);

            CivicAddressDb pickUpAddress2 = roblox;
            CivicAddressDb dropOffAddress2 = sms;
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
            ContractorRoutePlanner routePlanner = new ContractorRoutePlanner(user.ContractorInfo, order, route, locationHelper);


            routePlanner.CalculateOptimalRoute();
            locationHelper.Refresh();


            Assert.IsTrue(routePlanner.WillWork);
            Assert.AreEqual(user.ContractorInfo, routePlanner.Contractor);
            Assert.IsNotNull(routePlanner.PickUp);
            Assert.AreEqual(StopType.PickUp, routePlanner.PickUp.Type);

            Assert.IsNotNull(routePlanner.DropOff);
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
            user.ContractorInfo.HomeAddress = home;
            user.ContractorInfo.Truck.Location = LocationHelper.CreatePoint(43.5318683, -96.7271978);
            user.ContractorInfo.Truck.MaxCapacity = new TruckCapacityUnit { Mass = 180, Volume = 180 };
            user.ContractorInfo.Truck.UsedCapacity = new TruckCapacityUnit { Mass = 0, Volume = 0 };

            CivicAddressDb pickUpAddress = buildertrend;
            CivicAddressDb dropOffAddress = sms;
            TruckCapacityUnit orderSize = new TruckCapacityUnit { Mass = 200, Volume = 200 };
            Order order = ServiceTestUtils.CreateOrder(user, Models.Orders.OrderStatus.Queued, pickUpAddress, dropOffAddress, orderSize);

            List<RouteStop> route = new List<RouteStop>();
            ContractorRoutePlanner routePlanner = new ContractorRoutePlanner(user.ContractorInfo, order, route, locationHelper);


            routePlanner.CalculateOptimalRoute();
            locationHelper.Refresh();


            Assert.IsFalse(routePlanner.WillWork);
        }
    }
}
