using PracticalWerewolf.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PracticalWerewolf.Models.Trucks;
using System.Device.Location;
using PracticalWerewolf.Stores.Interfaces;
using PracticalWerewolf.Stores.Interfaces.Contexts;
using PracticalWerewolf.Models;

namespace PracticalWerewolf.Services
{
    public class TruckService : ITruckService
    {
        private ITruckStore TruckStore;
        private ApplicationDbContext context;

        public TruckService(ITruckStore TruckStore, ApplicationDbContext DbContext)
        {
            this.context = DbContext;
            this.TruckStore = TruckStore;
        }

        public void CreateTruck(Truck truck)
        {
            //TODO possibly return an IdentityResult
            if (truck == null) throw new ArgumentNullException();
            try
            {
                TruckStore.Create(truck);
            }
            catch
            {
                throw new Exception();
            }
        }

        public IEnumerable<Truck> GetAllTrucks()
        {
            return TruckStore.GetAllTrucks();
        }

        public Truck GetTruck(Guid truckId)
        {
            return TruckStore.Get(truckId);
        }

        public Truck GetTruckByCustomerInfoGuid(Guid truckId)
        {
            throw new NotImplementedException();
        }

        public void UpdateTruckCurrentCapacity(Guid truckGuid, TruckCapacityUnit capacity)
        {
            throw new NotImplementedException();
        }

        public void UpdateTruckLocation(Guid truckGuid, GeoCoordinate location)
        {
            throw new NotImplementedException();
        }

        public void UpdateTruckMaxCapacity(Guid truckGuid, TruckCapacityUnit capacity)
        {
            if (capacity == null) throw new ArgumentNullException();
            //TODO: Maybe return an IdentityResult for this?
            var oldTruck = GetTruck(truckGuid);
            if(oldTruck != null)
            {
                var truck = new Truck
                {
                    Location = oldTruck.Location,
                    TruckGuid = oldTruck.TruckGuid,
                    CurrentCapacity = oldTruck.CurrentCapacity,
                    MaxCapacity = capacity
                };

                TruckStore.Update(truck);
                context.SaveChanges();
            }
        }
    }
}