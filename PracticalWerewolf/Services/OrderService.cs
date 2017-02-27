using PracticalWerewolf.Stores.Interfaces;
using PracticalWerewolf.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PracticalWerewolf.Services
{
    public class OrderService : IOrderManager
    {
        private IOrderStore OrderStore;

        public OrderService(IOrderStore store)
        {
            OrderStore = store;
        }
    }
}