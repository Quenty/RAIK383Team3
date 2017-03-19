using Microsoft.AspNet.Identity.EntityFramework;
using PracticalWerewolf.Controllers.UnitOfWork;
using PracticalWerewolf.Models;
using PracticalWerewolf.Models.Orders;
using PracticalWerewolf.Models.Trucks;
using PracticalWerewolf.Models.UserInfos;
using PracticalWerewolf.Stores.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Device.Location;
using System.Linq;
using System.Web;

namespace PracticalWerewolf.Application
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public DbSet<CustomerInfo> CustomerInfo { get; set; }
        public DbSet<UserInfo> UserInfo { get; set; }
        public DbSet<EmployeeInfo> EmployeeInfo { get; set; }
        public DbSet<ContractorInfo> ContractorInfo { get; set; }

        public DbSet<Truck> Truck { get; set; }
        public DbSet<TruckCapacityUnit> TruckCapacityUnit { get; set; }
        public DbSet<CivicAddressDb> CivicAddressDb { get; set; }

        public DbSet<Order> Order { get; set; }
        public DbSet<OrderRequestInfo> OrderRequestInfo { get; set; }
        public DbSet<OrderTrackInfo> OrderTrackInfo { get; set; }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

    }

    // https://github.com/icotting/SETwitter/blob/9bf60ffe26cacee4b44b4a9eae48156bf2145949/ASP.NET/Twitter/Application/TwitterContext.cs
    public class ApplicationContextAdapter : IDbSetFactory, IUnitOfWork
    {
        private readonly DbContext _context;

        public ApplicationContextAdapter(DbContext context)
        {
            _context = context;
        }


        #region IUnitOfWork Members

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        #endregion

        #region IObjectSetFactory Members

        public void Dispose()
        {
            _context.Dispose();
        }

        public DbSet<T> CreateDbSet<T>() where T : class
        {
            return _context.Set<T>();
        }

        public void ChangeObjectState(object entity, EntityState state)
        {
            _context.Entry(entity).State = state;
            
        }

        public void RefreshEntity<T>(ref T entity) where T : class
        {
            _context.Entry<T>(entity).Reload();
        }

        #endregion
    }
}