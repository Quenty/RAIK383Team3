using PracticalWerewolf.Models;
using PracticalWerewolf.Models.Orders;
using PracticalWerewolf.Models.Trucks;
using PracticalWerewolf.Stores.Interfaces;
using PracticalWerewolf.Stores.Interfaces.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PracticalWerewolf.Stores
{
    public class OrderStore : IOrderStore
    {
        private IOrderDbContext context;

        public OrderStore(IOrderDbContext orderDbContext)
        {
            context = orderDbContext;
        }

        public void Add(IEnumerable<Order> orderList)
        {
            throw new NotImplementedException();
        }

        public void Add(Order order)
        {
            throw new NotImplementedException();
        }

        public void Delete(IEnumerable<Order> orderList)
        {
            throw new NotImplementedException();
        }

        public void Delete(Order order)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Order> Get()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Order> Get(IEnumerable<Guid> guids)
        {
            throw new NotImplementedException();
        }

        public Order Get(Guid guid)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Order> GetByTruck(Truck truck)
        {
            throw new NotImplementedException();
        }

        public void Update(IEnumerable<Order> orderList)
        {
            throw new NotImplementedException();
        }

        public void Update(Order order)
        {
            throw new NotImplementedException();
        }
    }
}