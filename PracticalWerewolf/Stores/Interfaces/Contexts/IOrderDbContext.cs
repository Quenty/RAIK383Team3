using PracticalWerewolf.Models.Orders;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PracticalWerewolf.Stores.Interfaces.Contexts
{
    public interface IOrderDbContext
    {
        DbSet<Order> Order { get; set; }
        DbSet<OrderRequestInfo> OrderRequestInfo { get; set; }
        DbSet<OrderTrackInfo> OrderTrackInfo { get; set; }
    }
}
