using PracticalWerewolf.Models;
using PracticalWerewolf.Models.UserInfos;
using PracticalWerewolf.Stores.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PracticalWerewolf.Stores
{
    public class CustomerStore : EntityStore<CustomerInfo>, ICustomerStore
    {
        public CustomerStore(IDbSetFactory context) : base(context)
        {
            
        }

        
    }
}