﻿using System;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using PracticalWerewolf.Models;
using System.ComponentModel.DataAnnotations;
using PracticalWerewolf.Models.UserInfos;
using PracticalWerewolf.Models.Trucks;
using PracticalWerewolf.Models.Orders;
using PracticalWerewolf.Stores.Interfaces;
using PracticalWerewolf.Stores.Interfaces.Contexts;

namespace PracticalWerewolf.Models
{
    public class ApplicationUser : IdentityUser, IApplicationUser
    {
        public virtual ContractorInfo ContractorInfo { get; set; }          // One-to-one relationship, nullable field in an application user table
        public virtual CustomerInfo CustomerInfo { get; set; }              // One-to-one relationship, nullable field in an application user table
        public virtual EmployeeInfo EmployeeInfo { get; set; }              // One-to-one relationship, nullable field in an application user table
        public virtual UserInfo UserInfo { get; set; }                      // One-to-one relationship, nullable field in an application user table

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here

            return userIdentity;
        }
    }


    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>, IOrderDbContext, IUserInfoContext, ITruckDbContext
    {
        public DbSet<CustomerInfo> CustomerInfo { get; set; }
        public DbSet<UserInfo> UserInfo { get; set; }
        public DbSet<EmployeeInfo> EmployeeInfo { get; set; }
        public DbSet<ContractorInfo> ContractorInfo { get; set; }

        public DbSet<Truck> Truck { get; set; }
        public DbSet<TruckCapacityUnit> TruckCapacityUnit { get; set; }

        public DbSet<Order> Order { get; set; }
        public DbSet<OrderRequestInfo> OrderRequestInfo { get; set; }
        public DbSet<OrderTrackInfo> OrderTrackInfo { get; set; }

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