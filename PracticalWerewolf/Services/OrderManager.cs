using PracticalWerewolf.Repository.Interfaces;
using PracticalWerewolf.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PracticalWerewolf.Models;

namespace PracticalWerewolf.Services
{
    public class OrderManager : IOrderManager
    {
        private IOrderStore OrderStore;

        public OrderManager(IOrderStore store)
        {
            OrderStore = store;
        }

        public void CreateOrder(IOrder order)
        {
            throw new NotImplementedException();
        }

        public void DeleteOrder(Guid orderGuid)
        {
            throw new NotImplementedException();
        }

        public IOrder GetOrder(Guid orderGuid)
        {
            throw new NotImplementedException();
        }

        public void UpdateOrder(IOrder order)
        {
            throw new NotImplementedException();
        }
    }
}