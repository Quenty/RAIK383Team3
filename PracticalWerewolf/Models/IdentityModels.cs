﻿using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using PracticalWerewolf.Models;

namespace PracticalWerewolf.Models
{
    public class ApplicationUser : IdentityUser
    {

        public IContractorInfo ContractorInfo { get; set; }
        public ICustomerInfo CustomerInfo { get; set; }
        public IEmployeeInfo EmployeeInfo { get; set; }
        public IUserInfo UserInfo { get; set; }
        public IPermission Permissions { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<IContractorInfo> ContractorInfo { get; set; }
        public DbSet<ICustomerInfo> CustomerInfo { get; set; }
        public DbSet<IEmployeeInfo> EmployeeInfo { get; set; }
        public DbSet<IUserInfo> UserInfo { get; set; }
        public DbSet<IPermission> Permissions { get; set; }

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