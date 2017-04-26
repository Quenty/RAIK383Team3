using PracticalWerewolf.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PracticalWerewolf.Models.Orders;
using PracticalWerewolf.Stores.Interfaces;


namespace PracticalWerewolf.Services
{
    public class OrderRequestService : IOrderRequestService
    {
        private readonly IOrderStore OrderStore;

        public OrderRequestService(IOrderStore OrderStore)
        {
            this.OrderStore = OrderStore;
        }

        public IEnumerable<Order> GetCustomerOrders(Guid customerInfoGuid)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Order> GetCustomerOrders(Guid customerInfoGuid, OrderStatus orderStatus)
        {
            throw new NotImplementedException();
        }

        public Order CreateOrder(Order order)
        {
            if (order.OrderGuid == Guid.Empty)
            {
                order.OrderGuid = Guid.NewGuid();
            }

            OrderStore.Insert(order);

            return order;
        }
    }
}