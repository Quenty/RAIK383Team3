using PracticalWerewolf.Stores.Interfaces;
using PracticalWerewolf.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PracticalWerewolf.Services
{
    public class CustomerService : ICustomerManager
    {
        private ICustomerStore CustomerStore;

        public CustomerService(ICustomerStore store)
        {
            CustomerStore = store;
        }
    }
}