using PracticalWerewolf.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PracticalWerewolf.Models.Orders;
using PracticalWerewolf.Stores.Interfaces;
using PracticalWerewolf.Models.UserInfos;
using log4net;


namespace PracticalWerewolf.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderStore OrderStore;
        private readonly IContractorStore ContractorStore;
        private static readonly ILog log = LogManager.GetLogger(typeof(IOrderService));

        public OrderService(IOrderStore OrderStore, IContractorStore ContractorStore)
        {
            this.OrderStore = OrderStore;
            this.ContractorStore = ContractorStore;
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

        public OrderService(IOrderStore OrderStore)
        {
            this.OrderStore = OrderStore;
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
                    log.Warn("Failed to assign order, no available contractors");
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

        public IEnumerable<Order> GetDeliveredOrders(ContractorInfo contractor)
        {
            var allOrders = OrderStore.Find(o => o.TrackInfo.Assignee.ContractorInfoGuid == contractor.ContractorInfoGuid);
            return allOrders.Where(o => o.TrackInfo.OrderStatus == OrderStatus.Complete).ToList();
        }

        public IEnumerable<Order> GetInprogressOrders(ContractorInfo contractor)
        {
            var allOrders = OrderStore.Find(o => o.TrackInfo.Assignee.ContractorInfoGuid == contractor.ContractorInfoGuid);
            return allOrders.Where(o => o.TrackInfo.OrderStatus == OrderStatus.InProgress).ToList();
        }

        public IEnumerable<Order> GetQueuedOrders(ContractorInfo contractor)
        {
            var allOrders = OrderStore.Find(o => o.TrackInfo.Assignee.ContractorInfoGuid == contractor.ContractorInfoGuid);
            return allOrders.Where(o => o.TrackInfo.OrderStatus == OrderStatus.Queued).ToList();
        }
    }
}