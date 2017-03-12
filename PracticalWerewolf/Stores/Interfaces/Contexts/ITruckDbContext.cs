using PracticalWerewolf.Models.Trucks;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PracticalWerewolf.Stores.Interfaces.Contexts
{
    public interface ITruckDbContext
    {
        DbSet<Truck> Truck { get; set; }
        DbSet<TruckCapacityUnit> TruckCapacityUnit { get; set; }
    }
}
