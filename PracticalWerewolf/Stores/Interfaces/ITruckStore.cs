using PracticalWerewolf.Models;
using PracticalWerewolf.Models.Trucks;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PracticalWerewolf.Stores.Interfaces
{
    public interface ITruckStore : IEntityStore<Truck>
    {
        //Get all Truck
        IEnumerable<Truck> GetAllTrucks();

        Truck GetByCustomerInfoGuid(Guid customerInfo);

        //Batch Get Trucks by guid
        IEnumerable<Truck> Get(IEnumerable<Guid> guids);
    }
}
