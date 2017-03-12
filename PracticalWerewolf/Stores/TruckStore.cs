using PracticalWerewolf.Models;
using PracticalWerewolf.Models.Trucks;
using PracticalWerewolf.Stores.Interfaces;
using PracticalWerewolf.Stores.Interfaces.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PracticalWerewolf.Stores
{
    public class TruckStore : ITruckStore
    {
        private ITruckDbContext context;

        public TruckStore(ITruckDbContext truckDbContext)
        {
            context = truckDbContext;
        }

        public void Add(IEnumerable<Truck> truckList)
        {
            throw new NotImplementedException();
        }

        public void Add(Truck truck)
        {
            throw new NotImplementedException();
        }

        public void Delete(IEnumerable<Truck> truckList)
        {
            throw new NotImplementedException();
        }

        public void Delete(Truck Truck)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Truck> GetAllTrucks()
        {
            var trucks = context.Truck.ToList();
            return trucks;
        }

        public IEnumerable<Truck> Get(IEnumerable<Guid> guids)
        {
            throw new NotImplementedException();
        }

        public Truck Get(Guid guid)
        {
            var truck = context.Truck.Find(guid);
            return truck;
        }

        public Truck GetByCustomerInfoGuid(Guid customerInfo)
        {
            throw new NotImplementedException();
        }

        public void Update(IEnumerable<Truck> truckList)
        {
            throw new NotImplementedException();
        }

        public void Update(Truck truck)
        {
            var oldTruck = context.Truck.Find(truck.TruckGuid);
            
            if(oldTruck != null)
            {
                oldTruck.MaxCapacity = truck.MaxCapacity;
                oldTruck.Location = truck.Location;
                oldTruck.CurrentCapacity = truck.CurrentCapacity;
            }

        }
    }
}
