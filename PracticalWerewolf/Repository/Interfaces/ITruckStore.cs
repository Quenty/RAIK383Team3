using PracticalWerewolf.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PracticalWerewolf.Repository.Interfaces
{
    public interface ITruckStore
    {
        //Get all Truck
        List<ITruck> Get();

        //Get Truck by guid
        ITruck Get(Guid guid);

        //Batch Get Trucks by guid
        IEnumerable<ITruck> Get(IEnumerable<Guid> guids);

        //Add Truck
        void Add(ITruck truck);

        //Batch Add Truck
        void Add(IEnumerable<ITruck> truckList);

        //Update Truck
        void Update(ITruck truck);

        //Batch Update Truck
        void Update(IEnumerable<ITruck> truckList);

        //Delete Truck
        void Delete(ITruck Truck);

        //Batch DeleteContactorInfo
        void Delete(IEnumerable<ITruck> truckList);
    }
}
