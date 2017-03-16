using PracticalWerewolf.Models;
using PracticalWerewolf.Models.UserInfos;
using PracticalWerewolf.Stores.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PracticalWerewolf.Stores
{
    public class CustomerStore : ICustomerStore
    {
        private ApplicationDbContext context;

        public CustomerStore(ApplicationDbContext context)
        {
            this.context = context;
        }

        public void Add(IEnumerable<CustomerInfo> customerInfoList)
        {
            throw new NotImplementedException();
        }

        public void Add(CustomerInfo customerInfo)
        {
            throw new NotImplementedException();
        }

        public void Delete(IEnumerable<CustomerInfo> customerInfoList)
        {
            throw new NotImplementedException();
        }

        public void Delete(CustomerInfo customerInfo)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<CustomerInfo> Get()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<CustomerInfo> Get(IEnumerable<Guid> guids)
        {
            throw new NotImplementedException();
        }

        public CustomerInfo Get(Guid guid)
        {
            throw new NotImplementedException();
        }

        public IApplicationUser GetUser(Guid guid)
        {
            throw new NotImplementedException();
        }

        public void Update(IEnumerable<CustomerInfo> customerInfoList)
        {
            throw new NotImplementedException();
        }

        public void Update(CustomerInfo customerInfo)
        {
            throw new NotImplementedException();
        }
    }
}