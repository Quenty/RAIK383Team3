using PracticalWerewolf.Models;
using PracticalWerewolf.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PracticalWerewolf.Repository
{
    public class CustomerStore : ICustomerStore
    {
        private ApplicationDbContext Db;

        public CustomerStore(ApplicationDbContext dbContext)
        {
            Db = dbContext;
        }

        public void Add(IEnumerable<ICustomerInfo> customerInfoList)
        {
            throw new NotImplementedException();
        }

        public void Add(ICustomerInfo customerInfo)
        {
            throw new NotImplementedException();
        }

        public void Delete(IEnumerable<ICustomerInfo> customerInfoList)
        {
            throw new NotImplementedException();
        }

        public void Delete(ICustomerInfo customerInfo)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ICustomerInfo> Get()
        {
            throw new NotImplementedException();
        }

        public ICustomerInfo Get(Guid guid)
        {
            throw new NotImplementedException();
        }

        public IApplicationUser GetUser(Guid guid)
        {
            throw new NotImplementedException();
        }

        public void Update(IEnumerable<ICustomerInfo> customerInfoList)
        {
            throw new NotImplementedException();
        }

        public void Update(ICustomerInfo customerInfo)
        {
            throw new NotImplementedException();
        }
    }
}