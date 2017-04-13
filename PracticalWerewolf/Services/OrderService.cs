using PracticalWerewolf.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PracticalWerewolf.Models.Orders;
using PracticalWerewolf.Stores.Interfaces;
using System.Text;
using log4net;
using PracticalWerewolf.Helpers;
using PracticalWerewolf.Models.UserInfos;

namespace PracticalWerewolf.Services
{
    public class OrderService : IOrderService
    {
        private static ILog logger = LogManager.GetLogger(typeof(OrderService));
        private readonly IOrderStore _orderStore;
        private readonly ApplicationUserManager _userManager;

        public OrderService (IOrderStore orderStore, ApplicationUserManager userManager)
        {
            _orderStore = orderStore;
            _userManager = userManager;
        }

        public IEnumerable<Order> GetDeliveredOrders(ContractorInfo contractor)
        {
            var allOrders = _orderStore.Find(o => o.TrackInfo.Assignee.ContractorInfoGuid == contractor.ContractorInfoGuid);
            return allOrders.Where(o => o.TrackInfo.OrderStatus == OrderStatus.Complete).ToList();
        }

        public IEnumerable<Order> GetInprogressOrders(ContractorInfo contractor)
        {
            var allOrders = _orderStore.Find(o => o.TrackInfo.Assignee.ContractorInfoGuid == contractor.ContractorInfoGuid);
            return allOrders.Where(o => o.TrackInfo.OrderStatus == OrderStatus.InProgress).ToList();
        }

        public Order GetOrder(Guid orderGuid)
        {
            Order order = _orderStore.Single(o => o.OrderGuid == orderGuid);
            if (order == null)
            {
                throw new ArgumentException("Order does not exist in database");
            }
            return order;
        }

        public void CancelOrder(Guid orderGuid)
        {
            Order order = GetOrder(orderGuid);

            OrderTrackInfo orderTrackInfo = order.TrackInfo ?? new OrderTrackInfo();
            orderTrackInfo.OrderStatus = OrderStatus.Cancelled;
            _orderStore.Update(order);
        }

        public void AssignOrder(Guid orderGuid, ContractorInfo contractor)
        {
            Order order = GetOrder(orderGuid);
            OrderTrackInfo orderTrackInfo = order.TrackInfo ?? new OrderTrackInfo();
            orderTrackInfo.OrderStatus = OrderStatus.InProgress;
            orderTrackInfo.Assignee = contractor;
            _orderStore.Update(order);
        }

	public IEnumerable<Order> GetQueuedOrders(ContractorInfo contractor)
        {
            var allOrders = _orderStore.Find(o => o.TrackInfo.Assignee.ContractorInfoGuid == contractor.ContractorInfoGuid);
            return allOrders.Where(o => o.TrackInfo.OrderStatus == OrderStatus.Queued).ToList();
        }

        public async void SetOrderAsComplete(Guid orderId)
        {
            var order = _orderStore.Find(orderId);
            if (order != null)
            {
                order.TrackInfo.OrderStatus = OrderStatus.Complete;
                _orderStore.Update(order);

                var customerId = order.RequestInfo.Requester.CustomerInfoGuid;
                var customer = _userManager.Users.Single(x => x.CustomerInfo.CustomerInfoGuid == customerId);

                await EmailHelper.SendOrderDeliveredEmail(order.RequestInfo, customer);
            }
            else
            {
                logger.Error($"SetOrderAsComplete() - No order with id {orderId.ToString()}");
            }
        }

        public async void SetOrderAsInProgress(Guid orderId)
        {
            var order = _orderStore.Find(orderId);
            if (order != null)
            {
                order.TrackInfo.OrderStatus = OrderStatus.InProgress;
                _orderStore.Update(order);

                var customerId = order.RequestInfo.Requester.CustomerInfoGuid;
                var customer = _userManager.Users.Single(x => x.CustomerInfo.CustomerInfoGuid == customerId);

                await EmailHelper.SendOrderShippedEmail(order.RequestInfo, customer);
            }
            else
            {
                logger.Error($"SetOrderAsInProgress() - No order with id {orderId.ToString()}");
            }
        }
    }
}