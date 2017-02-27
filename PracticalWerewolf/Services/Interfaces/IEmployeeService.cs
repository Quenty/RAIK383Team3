using System;
using PracticalWerewolf.Models.Orders;
using PracticalWerewolf.Models.UserInfos;
using PracticalWerewolf.Models.Trucks;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PracticalWerewolf.Services.Interfaces
{
    interface IEmployeeService
    {
        // Depends upon IEmployeeStore.GetUser
        void GetUserEmployeeInfo(Guid userId);

        // Depend upon IEmployeeStore.Create
        void RegisterEmployeeInfo(Guid userId, EmployeeInfo newContractorInfo);
    }
}
