using PracticalWerewolf.Repository.Interfaces;
using PracticalWerewolf.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PracticalWerewolf.Models;

namespace PracticalWerewolf.Services
{
    public class CustomerManager : ICustomerManager
    {
        private ICustomerStore CustomerStore;

        public CustomerManager(ICustomerStore store)
        {
            CustomerStore = store;
        }

        public void CreateOrderRequest(IOrderRequestInfo orderRequestInfo)
        {
            throw new NotImplementedException();
        }

        public void DeleteOrder()
        {
            throw new NotImplementedException();
        }

        public IBillingInfo GetBillingInfo()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IOrder> GetCurrentOrders()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IOrder> GetPreviousOrders()
        {
            throw new NotImplementedException();
        }

        public void UpdateBillingInfo(IBillingInfo billingInfo)
        {
            throw new NotImplementedException();
        }

        public void UpdateOrder(IOrder order)
        {
            throw new NotImplementedException();
        }
    }
}