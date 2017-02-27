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
        List<Truck> Get();

        //Get Truck by guid
        Truck Get(Guid guid);

        Truck GetByCustomerInfoGuid(Guid customerInfo);

        //Batch Get Trucks by guid
        IEnumerable<Truck> Get(IEnumerable<Guid> guids);

        //Add Truck
        void Add(Truck truck);

        //Batch Add Truck
        void Add(IEnumerable<Truck> truckList);

        //Update Truck
        void Update(Truck truck);

        //Batch Update Truck
        void Update(IEnumerable<Truck> truckList);

        //Delete Truck
        void Delete(Truck Truck);

        //Batch DeleteTruck
        void Delete(IEnumerable<Truck> truckList);
    }
}
