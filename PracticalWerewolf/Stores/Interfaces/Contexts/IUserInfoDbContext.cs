using PracticalWerewolf.Models.UserInfos;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PracticalWerewolf.Stores.Interfaces.Contexts
{
    public interface IUserInfoDbContext
    {
        DbSet<CustomerInfo> CustomerInfo { get; set; }
        DbSet<UserInfo> UserInfo { get; set; }
        DbSet<EmployeeInfo> EmployeeInfo { get; set; }
        DbSet<ContractorInfo> ContractorInfo { get; set; }
    }
}
