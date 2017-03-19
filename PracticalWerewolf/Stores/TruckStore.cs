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
        public TruckStore(IDbSetFactory context) : base(context)
        {
        }

        public void Create(Truck truck)
        {
            base.Insert(truck);
        }

        public IEnumerable<Truck> GetAllTrucks()
        {
            return base.GetAll().ToList();
        }

        public IEnumerable<Truck> Get(IEnumerable<Guid> guids)
        {
            throw new NotImplementedException();
        }

        public Truck Get(Guid guid)
        {
            return base.Find(guid);
        }

        public Truck GetByCustomerInfoGuid(Guid customerInfo)
        {
            throw new NotImplementedException();
        }

        public void Update(Truck truck)
        {
            base.Update(truck);
        }
    }
}
