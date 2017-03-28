using PracticalWerewolf.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PracticalWerewolf.Models.Orders;

namespace PracticalWerewolf.Services
{
    public class OrderRequestService : IOrderRequestService
    {
        public IEnumerable<Order> GetCustomerOrders(Guid customerInfoGuid)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Order> GetCustomerOrders(Guid customerInfoGuid, OrderStatus orderStatus)
        {
            throw new NotImplementedException();
        }

        public void CreateOrderRequestInfo(OrderRequestInfo requestInfo)
        {
            throw new NotImplementedException();
        }
    }
}