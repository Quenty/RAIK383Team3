using PracticalWerewolf.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PracticalWerewolf.Models.Orders;

namespace PracticalWerewolf.Services
{
    public class OrderService : IOrderService
    {
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

        public IEnumerable<Order> GetQueuedOrders(OrderStatus orderStatus)
        {
            throw new NotImplementedException();
        }
    }
}