using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PracticalWerewolf.Models.Orders;
using System.Device.Location;
using System.Linq;
using PracticalWerewolf.Models.Trucks;
using PracticalWerewolf.Controllers;
using PracticalWerewolf.Models;
using PracticalWerewolf.Models.UserInfos;
using System.Collections;
using PracticalWerewolf.Models.Routes;
using System.Collections.Generic;
using PracticalWerewolf.Tests.Stores.DbContext;
using Moq;
using PracticalWerewolf.Stores.Interfaces;
using PracticalWerewolf.Services;

namespace PracticalWerewolf.Tests.Services
{
    [TestClass]
    public class RouteStopServiceTest
    {
        private static string email = "jesseelzhang@gmail.com";
        private static string otherEmail = "nope@nope.com";


        [TestMethod]
        public void GetContractorRoute_EmptyDbSet_EmptyList()
        {
            var user = CreateUser(email);

            var dbSet = new MockRouteStopDbSet();
            var dbContext = new Mock<IDbSetFactory>();
            dbContext.Setup(x => x.CreateDbSet<RouteStop>()).Returns(dbSet);
            var routeStopStore = new RouteStopStore(dbContext.Object);
            var routeStopService = new RouteStopService(routeStopStore);

            var result = routeStopService.GetContractorRoute(user.ContractorInfo);

            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count());
        }

        [TestMethod]
        public void GetContractorRoute_DbSet_OrderedList_SevenItems()
        {
            var user = CreateUser(email);
            var otherUser = CreateUser(otherEmail);
            var stops = CreateRouteStops(user, otherUser);

            var dbSet = new MockRouteStopDbSet();
            dbSet.AddRange(stops);
            var dbContext = new Mock<IDbSetFactory>();
            dbContext.Setup(x => x.CreateDbSet<RouteStop>()).Returns(dbSet);
            var routeStopStore = new RouteStopStore(dbContext.Object);
            var routeStopService = new RouteStopService(routeStopStore);

            var result = routeStopService.GetContractorRoute(user.ContractorInfo);

            Assert.IsNotNull(result);
            Assert.AreEqual(7, result.Count());
            for(int i = 0; i < 7; i++)
            {
                Assert.AreEqual(stops.ElementAt(i), result.ElementAt(i));
                Assert.AreEqual(i, result.ElementAt(i).StopOrder);
            }
        }

        [TestMethod]
        public void GetContractorRoute_DbSetNoInProgressOrQueued_EmptyList()
        {
            var user = CreateUser(email);
            var otherUser = CreateUser(otherEmail);
            var stops = CreateRouteStops(user, otherUser);

            var dbSet = new MockRouteStopDbSet();
            dbSet.AddRange(stops);
            var dbContext = new Mock<IDbSetFactory>();
            dbContext.Setup(x => x.CreateDbSet<RouteStop>()).Returns(dbSet);
            var routeStopStore = new RouteStopStore(dbContext.Object);
            var routeStopService = new RouteStopService(routeStopStore);

            var result = routeStopService.GetContractorRoute(otherUser.ContractorInfo);

            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetContractorRoute_NullContractor_ArgumentNullExceptioon()
        {
            var dbSet = new MockRouteStopDbSet();
            var dbContext = new Mock<IDbSetFactory>();
            dbContext.Setup(x => x.CreateDbSet<RouteStop>()).Returns(dbSet);
            var routeStopStore = new RouteStopStore(dbContext.Object);
            var routeStopService = new RouteStopService(routeStopStore);
            ContractorInfo info = null;

            var result = routeStopService.GetContractorRoute(info);

            Assert.Fail();
        }


        [TestMethod]
        public void GetContractorCurrentAssignment_EmptyDbSet_Null()
        {
            var user = CreateUser(email);

            var dbSet = new MockRouteStopDbSet();
            var dbContext = new Mock<IDbSetFactory>();
            dbContext.Setup(x => x.CreateDbSet<RouteStop>()).Returns(dbSet);
            var routeStopStore = new RouteStopStore(dbContext.Object);
            var routeStopService = new RouteStopService(routeStopStore);

            var result = routeStopService.GetContractorCurrentAssignment(user.ContractorInfo);

            Assert.IsNull(result);
        }

        [TestMethod]
        public void GetContractorCurrentAssignment_DbSetWithOneInProgress_NotNull()
        {
            var user = CreateUser(email);
            var otherUser = CreateUser(otherEmail);
            var stops = CreateRouteStops(user, otherUser);

            var dbSet = new MockRouteStopDbSet();
            dbSet.AddRange(stops);
            var dbContext = new Mock<IDbSetFactory>();
            dbContext.Setup(x => x.CreateDbSet<RouteStop>()).Returns(dbSet);
            var routeStopStore = new RouteStopStore(dbContext.Object);
            var routeStopService = new RouteStopService(routeStopStore);

            var result = routeStopService.GetContractorCurrentAssignment(user.ContractorInfo);

            Assert.IsNotNull(result);
            Assert.AreEqual(stops.ElementAt(0), result);
        }

        [TestMethod]
        public void GetContractorCurrentAssignment_DbSetWithZeroInProgress_Null()
        {
            var user = CreateUser(email);
            var otherUser = CreateUser(otherEmail);
            var stops = CreateRouteStops(user, otherUser);

            var dbSet = new MockRouteStopDbSet();
            dbSet.AddRange(stops);
            var dbContext = new Mock<IDbSetFactory>();
            dbContext.Setup(x => x.CreateDbSet<RouteStop>()).Returns(dbSet);
            var routeStopStore = new RouteStopStore(dbContext.Object);
            var routeStopService = new RouteStopService(routeStopStore);

            var result = routeStopService.GetContractorCurrentAssignment(otherUser.ContractorInfo);

            Assert.IsNull(result);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetContractorCurrentAssignment_NullContractor_ArgumentNullException()
        {
            var dbSet = new MockRouteStopDbSet();
            var dbContext = new Mock<IDbSetFactory>();
            dbContext.Setup(x => x.CreateDbSet<RouteStop>()).Returns(dbSet);
            var routeStopStore = new RouteStopStore(dbContext.Object);
            var routeStopService = new RouteStopService(routeStopStore);
            ContractorInfo info = null;

            var result = routeStopService.GetContractorCurrentAssignment(info);

            Assert.Fail();
        }








        private static Random random = new Random();

        private static ApplicationUser CreateUser(string email)
        {
            var license = RandomString(8);
            var address = CreateRandomAddress();
            var truck = CreateRandomTruck();

            var user = new ApplicationUser
            {
                Id = email,
                UserName = email,
                Email = email,
                ContractorInfo = new ContractorInfo
                {
                    ContractorInfoGuid = Guid.NewGuid(),
                    DriversLicenseId = license,
                    HomeAddress = address,
                    ApprovalState = ContractorApprovalState.Approved,
                    IsAvailable = true,
                    Truck = truck
                },
                CustomerInfo = new CustomerInfo
                {
                    CustomerInfoGuid = Guid.NewGuid()
                }
            };

            return user;
        }

        private static Order CreateRandomOrder(ApplicationUser user, OrderStatus status)
        {
            return new Order
            {
                OrderGuid = Guid.NewGuid(),
                RequestInfo = new OrderRequestInfo
                {
                    OrderRequestInfoGuid = Guid.NewGuid(),
                    RequestDate = DateTime.Now,
                    Requester = user.CustomerInfo,
                    DropOffAddress = CreateRandomAddress(),
                    PickUpAddress = CreateRandomAddress(),
                    Size = new TruckCapacityUnit() { TruckCapacityUnitGuid = Guid.NewGuid(), Mass = random.Next(), Volume = random.Next() }
                },
                TrackInfo = new OrderTrackInfo
                {
                    Assignee = user.ContractorInfo,
                    CurrentTruck = user.ContractorInfo.Truck,
                    OrderStatus = status,
                    OrderTrackInfoGuid = Guid.NewGuid()
                }
            };
        }

        private static CivicAddressDb CreateRandomAddress()
        {
            return new CivicAddressDb
            {
                CivicAddressGuid = Guid.NewGuid(),
                City = RandomString(8),
                State = RandomString(8),
                Country = RandomString(8),
                StreetNumber = RandomString(8),
                ZipCode = RandomString(8),
                RawInputAddress = RandomString(25),
                Route = RandomString(8)
            };
        }

        private static Truck CreateRandomTruck()
        {
            return new Truck()
            {
                LicenseNumber = RandomString(8),
                Location = LocationHelper.CreatePoint(random.Next(-89, 89), random.Next(-89, 89)),
                MaxCapacity = new TruckCapacityUnit() { TruckCapacityUnitGuid = Guid.NewGuid(), Mass = random.Next(), Volume = random.Next() },
                UsedCapacity = new TruckCapacityUnit() { TruckCapacityUnitGuid = Guid.NewGuid(), Mass = random.Next(), Volume = random.Next() },
                TruckGuid = Guid.NewGuid()
            };
        }

        private static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private static IEnumerable<RouteStop> CreateRouteStops(ApplicationUser user, ApplicationUser otherUser)
        {
            return new List<RouteStop>()
            {
                new RouteStop() {Order = CreateRandomOrder(user, OrderStatus.InProgress), RouteStopGuid = Guid.NewGuid(), Type = StopType.PickUp, EstimatedTimeToNextStop = TimeSpan.Zero, StopOrder = 0 },
                new RouteStop() {Order = CreateRandomOrder(user, OrderStatus.Queued), RouteStopGuid = Guid.NewGuid(), Type = StopType.DropOff, EstimatedTimeToNextStop = TimeSpan.Zero, StopOrder = 1 },
                new RouteStop() {Order = CreateRandomOrder(user, OrderStatus.Queued), RouteStopGuid = Guid.NewGuid(), Type = StopType.DropOff, EstimatedTimeToNextStop = TimeSpan.Zero, StopOrder = 2 },
                new RouteStop() {Order = CreateRandomOrder(user, OrderStatus.Queued), RouteStopGuid = Guid.NewGuid(), Type = StopType.PickUp, EstimatedTimeToNextStop = TimeSpan.Zero, StopOrder = 3 },
                new RouteStop() {Order = CreateRandomOrder(user, OrderStatus.Queued), RouteStopGuid = Guid.NewGuid(), Type = StopType.DropOff, EstimatedTimeToNextStop = TimeSpan.Zero, StopOrder = 4 },
                new RouteStop() {Order = CreateRandomOrder(user, OrderStatus.Queued), RouteStopGuid = Guid.NewGuid(), Type = StopType.PickUp, EstimatedTimeToNextStop = TimeSpan.Zero, StopOrder = 5 },
                new RouteStop() {Order = CreateRandomOrder(user, OrderStatus.Queued), RouteStopGuid = Guid.NewGuid(), Type = StopType.DropOff, EstimatedTimeToNextStop = TimeSpan.Zero, StopOrder = 6 },
                new RouteStop() {Order = CreateRandomOrder(user, OrderStatus.Cancelled), RouteStopGuid = Guid.NewGuid(), Type = StopType.DropOff, EstimatedTimeToNextStop = TimeSpan.Zero, StopOrder = -1 },
                new RouteStop() {Order = CreateRandomOrder(user, OrderStatus.Complete), RouteStopGuid = Guid.NewGuid(), Type = StopType.PickUp, EstimatedTimeToNextStop = TimeSpan.Zero, StopOrder = -2 },
                new RouteStop() {Order = CreateRandomOrder(user, OrderStatus.Complete), RouteStopGuid = Guid.NewGuid(), Type = StopType.DropOff, EstimatedTimeToNextStop = TimeSpan.Zero, StopOrder = -3 },

                new RouteStop() {Order = CreateRandomOrder(otherUser, OrderStatus.Cancelled), RouteStopGuid = Guid.NewGuid(), Type = StopType.PickUp, EstimatedTimeToNextStop = TimeSpan.Zero, StopOrder = 0 },
                new RouteStop() {Order = CreateRandomOrder(otherUser, OrderStatus.Complete), RouteStopGuid = Guid.NewGuid(), Type = StopType.DropOff, EstimatedTimeToNextStop = TimeSpan.Zero, StopOrder = 2 },
                new RouteStop() {Order = CreateRandomOrder(otherUser, OrderStatus.Complete), RouteStopGuid = Guid.NewGuid(), Type = StopType.DropOff, EstimatedTimeToNextStop = TimeSpan.Zero, StopOrder = 1 },
                new RouteStop() {Order = CreateRandomOrder(otherUser, OrderStatus.Complete), RouteStopGuid = Guid.NewGuid(), Type = StopType.PickUp, EstimatedTimeToNextStop = TimeSpan.Zero, StopOrder = -1 },
                new RouteStop() {Order = CreateRandomOrder(otherUser, OrderStatus.Complete), RouteStopGuid = Guid.NewGuid(), Type = StopType.PickUp, EstimatedTimeToNextStop = TimeSpan.Zero, StopOrder = -2 },

            };
        }
    }
}
