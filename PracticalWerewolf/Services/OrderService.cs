using PracticalWerewolf.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PracticalWerewolf.Models.Orders;
using PracticalWerewolf.Models.UserInfos;
using PracticalWerewolf.Stores.Interfaces;

namespace PracticalWerewolf.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderStore _orderStore;

        public OrderService (IOrderStore OrderStore)
        {
            _orderStore = OrderStore;
        }


        public void CancelOrder(Guid orderGuid)
        {
            throw new NotImplementedException();
        }

        public void CreateOrder(OrderRequestInfo order)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Order> GetByUserGuids(Guid userId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Order> GetInprogressOrders(ContractorInfo contractor)
        {
            var allOrders = _orderStore.Find(o => o.TrackInfo.Assignee.ContractorInfoGuid == contractor.ContractorInfoGuid);
            return allOrders.Where(o => o.TrackInfo.OrderStatus == OrderStatus.InProgress).ToList();
        }

        public Order GetOrder(Guid orderGuid)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Order> GetOrders()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Order> GetOrders(OrderStatus orderStatus)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Order> GetOrdersByCustomerInfo(Guid customerInfoGuid)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Order> GetQueuedOrders(ContractorInfo contractor)
        {
            var allOrders = _orderStore.Find(o => o.TrackInfo.Assignee.ContractorInfoGuid == contractor.ContractorInfoGuid);
            return allOrders.Where(o => o.TrackInfo.OrderStatus == OrderStatus.Queued).ToList();
        }
    }
}