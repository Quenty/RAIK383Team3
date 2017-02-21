using PracticalWerewolf.Models.UserInfos;
using System;

namespace PracticalWerewolf.Models
{
    public interface IApplicationUser
    {
        ContractorInfo ContractorInfo { get; set; } 
        CustomerInfo CustomerInfo { get; set; }
        EmployeeInfo EmployeeInfo { get; set; }
        UserInfo UserInfo { get; set; }
    }
}
