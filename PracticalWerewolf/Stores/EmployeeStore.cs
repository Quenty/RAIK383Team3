using PracticalWerewolf.Models;
using PracticalWerewolf.Models.UserInfos;
using PracticalWerewolf.Stores.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PracticalWerewolf.Stores
{
    public class EmployeeStore : EntityStore<EmployeeInfo>, IEmployeeStore
    {

        public EmployeeStore(IDbSetFactory context) : base(context)
        {
            
        }

    }
}