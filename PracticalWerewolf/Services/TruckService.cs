using PracticalWerewolf.Services.Interfaces;
using System;
using System.Collections.Generic;
using PracticalWerewolf.Models.Trucks;
using System.Device.Location;
using PracticalWerewolf.Stores.Interfaces;
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
        public void Update(Truck newTruck)
        {
            TruckStore.Update(newTruck);
        }
    }
}