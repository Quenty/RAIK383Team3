using PracticalWerewolf.Stores.Interfaces;
using PracticalWerewolf.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PracticalWerewolf.Models;
using PracticalWerewolf.Models.Orders;

namespace PracticalWerewolf.Services
{
    public class OrderService : IOrderService
    {
        private IOrderStore OrderStore;

        public OrderService(IOrderStore store)
        {
            OrderStore = store;
        }

        public void CreateOrder(Order order)
        {
            throw new NotImplementedException();
        }

        public void DeleteOrder(Guid orderGuid)
        {
            throw new NotImplementedException();
        }

        public Order GetOrder(Guid orderGuid)
        {
            throw new NotImplementedException();
        }

        public void UpdateOrder(Order order)
        {
            throw new NotImplementedException();
        }
    }
}