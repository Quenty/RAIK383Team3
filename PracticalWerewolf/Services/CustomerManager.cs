using PracticalWerewolf.Repository.Interfaces;
using PracticalWerewolf.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PracticalWerewolf.Services
{
    public class CustomerManager : UserManager, ICustomerManager
    {
        private ICustomerStore customerStore;

        public CustomerManager(ICustomerStore store) : base(store) { }
    }
}