using PracticalWerewolf.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PracticalWerewolf.Models.Trucks;
using System.Device.Location;
using PracticalWerewolf.Stores.Interfaces;

namespace PracticalWerewolf.Services
{
    public class TruckService : ITruckService
    {
        private ITruckStore TruckStore;
        
        public TruckService(ITruckStore TruckStore)
        {
            this.TruckStore = TruckStore;
        }

        public void CreateTruck(Truck truck)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Truck> GetAllTrucks()
        {
            return TruckStore.GetAllTrucks();
        }

        public Truck GetTruck(Guid truckId)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }
    }
}