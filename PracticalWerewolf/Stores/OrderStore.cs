using PracticalWerewolf.Models;
using PracticalWerewolf.Models.Orders;
using PracticalWerewolf.Models.Trucks;
using PracticalWerewolf.Stores.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PracticalWerewolf.Stores
{
    public class OrderStore : EntityStore<Order>, IOrderStore 
    {
        private readonly ApplicationDbContext context;

        public OrderStore(ApplicationDbContext context) : base(context.Order)
        {
            this.context = context;
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

        public IEnumerable<Order> Get(OrderStatus orderStatus)
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

        public IEnumerable<Order> GetByUserGuids(IEnumerable<Guid> userGuids)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Order> GetOrdersByContractorInfoGuid(Guid contractorInfoGuid)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Order> GetOrdersByCustomerInfoGuid(Guid customerInfoGuid)
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
