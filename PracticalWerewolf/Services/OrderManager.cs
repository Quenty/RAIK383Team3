using PracticalWerewolf.Repository.Interfaces;
using PracticalWerewolf.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PracticalWerewolf.Services
{
    public class OrderManager : IOrderManager
    {
        private IOrderStore orderStore;

        public OrderManager(IOrderStore store)
        {
            orderStore = store;
        }
    }
}