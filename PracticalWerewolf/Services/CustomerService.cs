using PracticalWerewolf.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PracticalWerewolf.Models.UserInfos;

namespace PracticalWerewolf.Services
{
    public class CustomerService : ICustomerService
    {
        public CustomerInfo GetUserCustomerInfo(Guid userId)
        {
            throw new NotImplementedException();
        }

        public void RegisterCustomerInfo(Guid userId, CustomerInfo newInfo)
        {
            throw new NotImplementedException();
        }
    }
}