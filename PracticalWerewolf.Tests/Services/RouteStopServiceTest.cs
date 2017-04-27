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
using PracticalWerewolf.Helpers.Interfaces;

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
            var user = ServiceTestUtils.CreateUser(email);

            var dbSet = new MockRouteStopDbSet();
            var dbContext = new Mock<IDbSetFactory>();
            dbContext.Setup(x => x.CreateDbSet<RouteStop>()).Returns(dbSet);
            var routeStopStore = new RouteStopStore(dbContext.Object);
            var locationHelper = new Mock<ILocationHelper>();
            var routeStopService = new RouteStopService(routeStopStore, locationHelper.Object);

            var result = routeStopService.GetContractorRouteAsNoTracking(user.ContractorInfo);

            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count());
        }

        [TestMethod]
        public void GetContractorRoute_DbSet_OrderedList_SevenItems()
        {
            var user = ServiceTestUtils.CreateUser(email);
            var otherUser = ServiceTestUtils.CreateUser(otherEmail);
            var stops = CreateRouteStops(user, otherUser);

            var dbSet = new MockRouteStopDbSet();
            dbSet.AddRange(stops);
            var dbContext = new Mock<IDbSetFactory>();
            dbContext.Setup(x => x.CreateDbSet<RouteStop>()).Returns(dbSet);
            var routeStopStore = new RouteStopStore(dbContext.Object);
            var locationHelper = new Mock<ILocationHelper>();
            var routeStopService = new RouteStopService(routeStopStore, locationHelper.Object);

            var result = routeStopService.GetContractorRouteAsNoTracking(user.ContractorInfo);

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
            var user = ServiceTestUtils.CreateUser(email);
            var otherUser = ServiceTestUtils.CreateUser(otherEmail);
            var stops = CreateRouteStops(user, otherUser);

            var dbSet = new MockRouteStopDbSet();
            dbSet.AddRange(stops);
            var dbContext = new Mock<IDbSetFactory>();
            dbContext.Setup(x => x.CreateDbSet<RouteStop>()).Returns(dbSet);
            var routeStopStore = new RouteStopStore(dbContext.Object);
            var locationHelper = new Mock<ILocationHelper>();
            var routeStopService = new RouteStopService(routeStopStore, locationHelper.Object);

            var result = routeStopService.GetContractorRouteAsNoTracking(otherUser.ContractorInfo);

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
            var locationHelper = new Mock<ILocationHelper>();
            var routeStopService = new RouteStopService(routeStopStore, locationHelper.Object);

            ContractorInfo info = null;

            var result = routeStopService.GetContractorRouteAsNoTracking(info);

            Assert.Fail();
        }


        [TestMethod]
        public void GetContractorCurrentAssignment_EmptyDbSet_Null()
        {
            var user = ServiceTestUtils.CreateUser(email);

            var dbSet = new MockRouteStopDbSet();
            var dbContext = new Mock<IDbSetFactory>();
            dbContext.Setup(x => x.CreateDbSet<RouteStop>()).Returns(dbSet);
            var routeStopStore = new RouteStopStore(dbContext.Object);
            var locationHelper = new Mock<ILocationHelper>();
            var routeStopService = new RouteStopService(routeStopStore, locationHelper.Object);


            var result = routeStopService.GetContractorCurrentAssignment(user.ContractorInfo);

            Assert.IsNull(result);
        }

        [TestMethod]
        public void GetContractorCurrentAssignment_DbSetWithOneInProgress_NotNull()
        {
            var user = ServiceTestUtils.CreateUser(email);
            var otherUser = ServiceTestUtils.CreateUser(otherEmail);
            var stops = CreateRouteStops(user, otherUser);

            var dbSet = new MockRouteStopDbSet();
            dbSet.AddRange(stops);
            var dbContext = new Mock<IDbSetFactory>();
            dbContext.Setup(x => x.CreateDbSet<RouteStop>()).Returns(dbSet);
            var routeStopStore = new RouteStopStore(dbContext.Object);
            var locationHelper = new Mock<ILocationHelper>();
            var routeStopService = new RouteStopService(routeStopStore, locationHelper.Object);


            var result = routeStopService.GetContractorCurrentAssignment(user.ContractorInfo);

            Assert.IsNotNull(result);
            Assert.AreEqual(stops.ElementAt(0), result);
        }

        [TestMethod]
        public void GetContractorCurrentAssignment_DbSetWithZeroInProgress_Null()
        {
            var user = ServiceTestUtils.CreateUser(email);
            var otherUser = ServiceTestUtils.CreateUser(otherEmail);
            var stops = CreateRouteStops(user, otherUser);

            var dbSet = new MockRouteStopDbSet();
            dbSet.AddRange(stops);
            var dbContext = new Mock<IDbSetFactory>();
            dbContext.Setup(x => x.CreateDbSet<RouteStop>()).Returns(dbSet);
            var routeStopStore = new RouteStopStore(dbContext.Object);
            var locationHelper = new Mock<ILocationHelper>();
            var routeStopService = new RouteStopService(routeStopStore, locationHelper.Object);


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
            var locationHelper = new Mock<ILocationHelper>();
            var routeStopService = new RouteStopService(routeStopStore, locationHelper.Object);

            ContractorInfo info = null;

            var result = routeStopService.GetContractorCurrentAssignment(info);

            Assert.Fail();
        }





        private static IEnumerable<RouteStop> CreateRouteStops(ApplicationUser user, ApplicationUser otherUser)
        {
            return new List<RouteStop>()
            {
                new RouteStop() {Order = ServiceTestUtils.CreateRandomOrder(user, OrderStatus.InProgress), RouteStopGuid = Guid.NewGuid(), Type = StopType.PickUp, EstimatedTimeToNextStop = TimeSpan.Zero, StopOrder = 0 },
                new RouteStop() {Order = ServiceTestUtils.CreateRandomOrder(user, OrderStatus.Queued), RouteStopGuid = Guid.NewGuid(), Type = StopType.DropOff, EstimatedTimeToNextStop = TimeSpan.Zero, StopOrder = 1 },
                new RouteStop() {Order = ServiceTestUtils.CreateRandomOrder(user, OrderStatus.Queued), RouteStopGuid = Guid.NewGuid(), Type = StopType.DropOff, EstimatedTimeToNextStop = TimeSpan.Zero, StopOrder = 2 },
                new RouteStop() {Order = ServiceTestUtils.CreateRandomOrder(user, OrderStatus.Queued), RouteStopGuid = Guid.NewGuid(), Type = StopType.PickUp, EstimatedTimeToNextStop = TimeSpan.Zero, StopOrder = 3 },
                new RouteStop() {Order = ServiceTestUtils.CreateRandomOrder(user, OrderStatus.Queued), RouteStopGuid = Guid.NewGuid(), Type = StopType.DropOff, EstimatedTimeToNextStop = TimeSpan.Zero, StopOrder = 4 },
                new RouteStop() {Order = ServiceTestUtils.CreateRandomOrder(user, OrderStatus.Queued), RouteStopGuid = Guid.NewGuid(), Type = StopType.PickUp, EstimatedTimeToNextStop = TimeSpan.Zero, StopOrder = 5 },
                new RouteStop() {Order = ServiceTestUtils.CreateRandomOrder(user, OrderStatus.Queued), RouteStopGuid = Guid.NewGuid(), Type = StopType.DropOff, EstimatedTimeToNextStop = TimeSpan.Zero, StopOrder = 6 },
                new RouteStop() {Order = ServiceTestUtils.CreateRandomOrder(user, OrderStatus.Cancelled), RouteStopGuid = Guid.NewGuid(), Type = StopType.DropOff, EstimatedTimeToNextStop = TimeSpan.Zero, StopOrder = -1 },
                new RouteStop() {Order = ServiceTestUtils.CreateRandomOrder(user, OrderStatus.Complete), RouteStopGuid = Guid.NewGuid(), Type = StopType.PickUp, EstimatedTimeToNextStop = TimeSpan.Zero, StopOrder = -2 },
                new RouteStop() {Order = ServiceTestUtils.CreateRandomOrder(user, OrderStatus.Complete), RouteStopGuid = Guid.NewGuid(), Type = StopType.DropOff, EstimatedTimeToNextStop = TimeSpan.Zero, StopOrder = -3 },

                new RouteStop() {Order = ServiceTestUtils.CreateRandomOrder(otherUser, OrderStatus.Cancelled), RouteStopGuid = Guid.NewGuid(), Type = StopType.PickUp, EstimatedTimeToNextStop = TimeSpan.Zero, StopOrder = 0 },
                new RouteStop() {Order = ServiceTestUtils.CreateRandomOrder(otherUser, OrderStatus.Complete), RouteStopGuid = Guid.NewGuid(), Type = StopType.DropOff, EstimatedTimeToNextStop = TimeSpan.Zero, StopOrder = 2 },
                new RouteStop() {Order = ServiceTestUtils.CreateRandomOrder(otherUser, OrderStatus.Complete), RouteStopGuid = Guid.NewGuid(), Type = StopType.DropOff, EstimatedTimeToNextStop = TimeSpan.Zero, StopOrder = 1 },
                new RouteStop() {Order = ServiceTestUtils.CreateRandomOrder(otherUser, OrderStatus.Complete), RouteStopGuid = Guid.NewGuid(), Type = StopType.PickUp, EstimatedTimeToNextStop = TimeSpan.Zero, StopOrder = -1 },
                new RouteStop() {Order = ServiceTestUtils.CreateRandomOrder(otherUser, OrderStatus.Complete), RouteStopGuid = Guid.NewGuid(), Type = StopType.PickUp, EstimatedTimeToNextStop = TimeSpan.Zero, StopOrder = -2 },

            };
        }
    }
}
