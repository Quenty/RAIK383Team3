﻿using PracticalWerewolf.Services.Interfaces;
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
        private readonly IOrderStore OrderStore;
        private readonly IContractorStore ContractorStore;
        private readonly ApplicationUserManager UserManager;

        public OrderService (IOrderStore orderStore, IContractorStore contractorStore, ApplicationUserManager userManager)
        {
            this.OrderStore = orderStore;
            this.ContractorStore = contractorStore;
            this.UserManager = userManager;
        }

        public IEnumerable<Order> GetDeliveredOrders(ContractorInfo contractor)
        {
            var allOrders = OrderStore.Find(o => o.TrackInfo.Assignee.ContractorInfoGuid == contractor.ContractorInfoGuid).ToList();
            return allOrders.Where(o => o.TrackInfo.OrderStatus == OrderStatus.Complete).ToList();
        }

        public IEnumerable<Order> GetInprogressOrders(ContractorInfo contractor)
        {
            var allOrders = OrderStore.Find(o => o.TrackInfo.Assignee.ContractorInfoGuid == contractor.ContractorInfoGuid).ToList();
            return allOrders.Where(o => o.TrackInfo.OrderStatus == OrderStatus.InProgress).ToList();
        }

        public Order GetOrder(Guid orderGuid)
        {
            Order order = OrderStore.Single(o => o.OrderGuid == orderGuid);
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
            OrderStore.Update(order);
        }

        public void AssignOrder(Guid orderGuid, ContractorInfo contractor)
        {
            Order order = GetOrder(orderGuid);
            OrderTrackInfo orderTrackInfo = order.TrackInfo ?? new OrderTrackInfo();
            orderTrackInfo.OrderStatus = OrderStatus.InProgress;
            orderTrackInfo.Assignee = contractor;
            OrderStore.Update(order);
        }

        public void AssignOrders()
        {
            var Orders = GetUnassignedOrders().ToList();
            
            // Initial slow, trivial algorithm
            foreach (Order order in Orders)
            {
                double volume = order.RequestInfo.Size.Volume;
                IQueryable<ContractorInfo> ContractorQuery = ContractorStore.getAvailableContractorsQuery()
                    .Where(x => x.Truck.MaxCapacity.Volume >= x.Truck.UsedCapacity.Volume + volume);

                ContractorInfo contractorInfo = ContractorQuery.FirstOrDefault();
                if (contractorInfo != null)
                {
                    AssignOrder(order.OrderGuid, contractorInfo);
                }
                else
                {
                    logger.Warn("Failed to assign order, no available contractors");
                }
            }
        }

        public IEnumerable<Order> GetUnassignedOrders()
        {
            return OrderStore.Find(o => o.TrackInfo.OrderStatus == OrderStatus.Queued, 
                o => o.TrackInfo, 
                o => o.TrackInfo.Assignee,
                o => o.RequestInfo,
                o => o.RequestInfo.Size)
                .Where(o => o.TrackInfo.Assignee == null);
        }

        public IEnumerable<Order> GetQueuedOrders(ContractorInfo contractor)
        {
            var allOrders = OrderStore.Find(o => o.TrackInfo.Assignee.ContractorInfoGuid == contractor.ContractorInfoGuid).ToList();
            return allOrders.Where(o => o.TrackInfo.OrderStatus == OrderStatus.Queued).ToList();
        }

        public async void SetOrderAsComplete(Guid orderId)
        {
            var order = OrderStore.Find(orderId);
            if (order != null)
            {
                order.TrackInfo.OrderStatus = OrderStatus.Complete;
                OrderStore.Update(order);

                var customerId = order.RequestInfo.Requester.CustomerInfoGuid;
                var customer = UserManager.Users.Single(x => x.CustomerInfo.CustomerInfoGuid == customerId);

                await EmailHelper.SendOrderDeliveredEmail(order.RequestInfo, customer);
            }
            else
            {
                logger.Error($"SetOrderAsComplete() - No order with id {orderId.ToString()}");
            }
        }

        public async void SetOrderAsInProgress(Guid orderId)
        {
            var order = OrderStore.Find(orderId);
            if (order != null)
            {
                order.TrackInfo.OrderStatus = OrderStatus.InProgress;
                OrderStore.Update(order);

                var customerId = order.RequestInfo.Requester.CustomerInfoGuid;
                var customer = UserManager.Users.Single(x => x.CustomerInfo.CustomerInfoGuid == customerId);

                await EmailHelper.SendOrderShippedEmail(order.RequestInfo, customer);
            }
            else
            {
                logger.Error($"SetOrderAsInProgress() - No order with id {orderId.ToString()}");
            }
        }
    }
}