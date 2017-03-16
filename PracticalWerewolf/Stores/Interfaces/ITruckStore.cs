using PracticalWerewolf.Models;
using PracticalWerewolf.Models.Trucks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PracticalWerewolf.Stores.Interfaces
{
    public interface ITruckStore
    {
        //Get all Truck
        IEnumerable<Truck> GetAllTrucks();

        //Get Truck by guid
        Truck Get(Guid guid);

        Truck GetByCustomerInfoGuid(Guid customerInfo);

        //Batch Get Trucks by guid
        IEnumerable<Truck> Get(IEnumerable<Guid> guids);

        //Add Truck
        void Create(Truck truck);

        //Update Truck
        void Update(Truck truck);

    }
}
