using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PracticalWerewolf.Models.Orders;
using System.Collections.Generic;
using PracticalWerewolf.Models.UserInfos;
using PracticalWerewolf.Models.Trucks;
using System.Device.Location;
using PracticalWerewolf.Tests.Stores.DbContext;
using PracticalWerewolf.Services;
using System.Data.Entity;
using PracticalWerewolf.Stores.Interfaces;
using Moq;
using PracticalWerewolf.Stores;
using System.Linq;

namespace PracticalWerewolf.Tests.Services
{
    [TestClass]
    public class OrderServiceTest
    {

        private static ContractorInfo contractor = new ContractorInfo()
        {
            ContractorInfoGuid = Guid.NewGuid(),
            ApprovalState = ContractorApprovalState.Approved,
            DriversLicenseId = "LicenseId",
            Truck = new Truck(),
            HomeAddress = new CivicAddressDb(),
            IsAvailable = true
        };

        private static Order completeOrder = new Order() { TrackInfo = new OrderTrackInfo() { Assignee = contractor, OrderStatus = OrderStatus.Complete } };
        private static Order cancelledOrder = new Order() { TrackInfo = new OrderTrackInfo() { Assignee = contractor, OrderStatus = OrderStatus.Cancelled } };
        private static Order inProgressOrder = new Order() { TrackInfo = new OrderTrackInfo() { Assignee = contractor, OrderStatus = OrderStatus.InProgress } };
        private static Order queuedOrder = new Order() { TrackInfo = new OrderTrackInfo() { Assignee = contractor, OrderStatus = OrderStatus.Queued } };

        private static IEnumerable<Order> orders = new List<Order>{ completeOrder, cancelledOrder, inProgressOrder, queuedOrder };

        [TestMethod]
        public void GetDeliveredOrders_Orders_OneOrder()
        {
            var orderDbSet = new MockOrderDbSet();
            orderDbSet.AddRange(orders);
            var service = GetOrderServiceWithDbSet(orderDbSet);

            var result = service.GetDeliveredOrders(contractor);

            Assert.AreEqual(1, result.Count());
            Assert.AreEqual(completeOrder, result.ElementAt(0));
        }

        [TestMethod]
        public void GetDeliveredOrders_EmptyOrders_ZeroOrders()
        {
            var orderDbSet = new MockOrderDbSet();
            var service = GetOrderServiceWithDbSet(orderDbSet);

            var result = service.GetDeliveredOrders(contractor);

            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count());
        }

        [TestMethod]
        public void GetInProgressOrders_Orders_OneOrder()
        {
            var orderDbSet = new MockOrderDbSet();
            orderDbSet.AddRange(orders);
            var service = GetOrderServiceWithDbSet(orderDbSet);

            var result = service.GetInprogressOrders(contractor);

            Assert.AreEqual(1, result.Count());
            Assert.AreEqual(inProgressOrder, result.ElementAt(0));
        }

        [TestMethod]
        public void GetInProgressOrders_EmptyOrders_ZeroOrders()
        {
            var orderDbSet = new MockOrderDbSet();
            var service = GetOrderServiceWithDbSet(orderDbSet);

            var result = service.GetInprogressOrders(contractor);

            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count());
        }

        [TestMethod]
        public void GetQueuedOrders_Orders_OneOrder()
        {
            var orderDbSet = new MockOrderDbSet();
            orderDbSet.AddRange(orders);
            var service = GetOrderServiceWithDbSet(orderDbSet);

            var result = service.GetQueuedOrders(contractor);

            Assert.AreEqual(1, result.Count());
            Assert.AreEqual(queuedOrder, result.ElementAt(0));
        }

        [TestMethod]
        public void GetQueuedOrders_EmptyOrders_ZeroOrders()
        {
            var orderDbSet = new MockOrderDbSet();
            var service = GetOrderServiceWithDbSet(orderDbSet);

            var result = service.GetQueuedOrders(contractor);

            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count());
        }


        private static OrderService GetOrderServiceWithDbSet(DbSet<Order> dbSet)
        {
            var mockContext = new Mock<IDbSetFactory>();
            mockContext.Setup(x => x.CreateDbSet<Order>()).Returns(dbSet);
            var store = new OrderStore(mockContext.Object);

            return new OrderService(store);
        }
    }
}
