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
    public class CustomerService : ICustomerService
    {
        private ICustomerStore CustomerStore;

        public CustomerService(ICustomerStore store)
        {
            CustomerStore = store;
        }

        public void CancelOrder(Guid orderId)
        {
            throw new NotImplementedException();
        }

        public void ChangeOrder(Order order)
        {
            throw new NotImplementedException();
        }

        public void CreateOrderRequest(OrderRequestInfo orderRequestInfo)
        {
            throw new NotImplementedException();
        }

        public void DeleteOrder(Guid orderId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Order> ViewCurrentOrders()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Order> ViewPreviousOrders()
        {
            throw new NotImplementedException();
        }
    }
}