using System;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using PracticalWerewolf.Models;

namespace PracticalWerewolf.Models
{
    public class ApplicationUser : IdentityUser
    {
        public virtual ContractorInfo ContractorInfo { get; set; }
        public virtual CustomerInfo CustomerInfo { get; set; }
        public virtual EmployeeInfo EmployeeInfo { get; set; }
        public virtual UserInfo UserInfo { get; set; } // 1-1

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here

            return userIdentity;
        }
    }

    public class UserInfo : IUserInfo {
        public System.Guid UserInfoGuid { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    public class CustomerInfo : ICustomerInfo
    {
        public System.Guid CustomerInfoGuid { get; set; }
        public virtual IBillingInfo BillingInfo { get; set; }
    }

    public class EmployeeInfo : IEmployeeInfo
    {
        public System.Guid CustomerInfoGuid { get; set; }
    }

    public class ContractorInfo : IContractorInfo
    {
        public System.Guid ContractorInfoGuid { get; set; }
        public virtual ITruck Truck { get; set; }
        public bool IsAvailable { get; set; }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        DbSet<CustomerInfo> CustomerInfo { get; set; }
        DbSet<UserInfo> UserInfo { get; set; }
        DbSet<EmployeeInfo> EmployeeInfo { get; set; }
        DbSet<ContractorInfo> ContractorInfo { get; set; }

        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}