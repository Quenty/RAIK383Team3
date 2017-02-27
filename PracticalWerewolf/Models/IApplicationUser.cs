using PracticalWerewolf.Models.UserInfos;
using System;

namespace PracticalWerewolf.Models
{
    public interface IApplicationUser
    {
        ContractorInfo ContractorInfo { get; set; }                 // One-to-one relationship, nullable field in an application user table
        CustomerInfo CustomerInfo { get; set; }                     // One-to-one relationship, nullable field in an application user table
        EmployeeInfo EmployeeInfo { get; set; }                     // One-to-one relationship, nullable field in an application user table
        UserInfo UserInfo { get; set; }                             // One-to-one relationship, nullable field in an application user table
    }
}
