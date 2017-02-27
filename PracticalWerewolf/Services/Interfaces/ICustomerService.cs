using System;
using PracticalWerewolf.Models;
using PracticalWerewolf.Models.Orders;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PracticalWerewolf.Models.UserInfos;

namespace PracticalWerewolf.Services.Interfaces
{
    interface ICustomerService
    {
        // Depends upon ICustomerStore.GetUser
        CustomerInfo GetUserCustomerInfo(Guid userId);

        // Depends upon ICustomerStore.Create
        void RegisterCustomerInfo(Guid userId, CustomerInfo newInfo);
    }
}
