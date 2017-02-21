using PracticalWerewolf.Models;
using PracticalWerewolf.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PracticalWerewolf.Repository
{
    public class OrderStore : IOrderStore
    {
        private ApplicationDbContext Db;

        public OrderStore(ApplicationDbContext dbContext)
        {
            this.Db = dbContext;
        }

        public void Add(IEnumerable<IOrder> orderList)
        {
            throw new NotImplementedException();
        }

        public void Add(IOrder order)
        {
            throw new NotImplementedException();
        }

        public void Delete(IEnumerable<IOrder> orderList)
        {
            throw new NotImplementedException();
        }

        public void Delete(IOrder order)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IOrder> Get()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IOrder> Get(IEnumerable<Guid> guids)
        {
            throw new NotImplementedException();
        }

        public IOrder Get(Guid guid)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IOrder> GetByTruck(ITruck truck)
        {
            throw new NotImplementedException();
        }

        public void Update(IEnumerable<IOrder> orderList)
        {
            throw new NotImplementedException();
        }

        public void Update(IOrder order)
        {
            throw new NotImplementedException();
        }
    }
}