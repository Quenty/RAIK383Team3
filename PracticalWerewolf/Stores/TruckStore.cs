using PracticalWerewolf.Models;
using PracticalWerewolf.Models.Trucks;
using PracticalWerewolf.Stores.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PracticalWerewolf.Stores
{
    public class TruckStore : EntityStore<Truck>, ITruckStore
    {
        private readonly ApplicationDbContext context;

        public TruckStore(ApplicationDbContext context) : base(context.Truck)
        {
            this.context = context;
        }

        public void Create(Truck truck)
        {
            if (truck == null) throw new ArgumentNullException();
            context.Truck.Add(truck);
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

        public void Update(Truck truck)
        {
            if(truck == null) throw new ArgumentNullException();

            //http://stackoverflow.com/a/15339512
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
