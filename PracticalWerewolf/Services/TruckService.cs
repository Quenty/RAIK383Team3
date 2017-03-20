using PracticalWerewolf.Services.Interfaces;
using System;
using System.Collections.Generic;
using PracticalWerewolf.Models.Trucks;
using System.Device.Location;
using PracticalWerewolf.Stores.Interfaces;
using PracticalWerewolf.Models;
using PracticalWerewolf.Controllers.UnitOfWork;
using System.Data.Entity.Spatial;

namespace PracticalWerewolf.Services
{
    public class TruckService : ITruckService
    {
        private readonly ITruckStore TruckStore;

        public TruckService(ITruckStore TruckStore)
        {
            this.TruckStore = TruckStore;
        }

        

        public void CreateTruck(Truck truck)
        {
            if (truck == null) throw new ArgumentNullException();
            TruckStore.Insert(truck);
        }

        public IEnumerable<Truck> GetAllTrucks()
        {
            return TruckStore.GetAllTrucks();
        }

        public Truck GetTruck(Guid truckGuid)
        {
            return TruckStore.Single(truck => truck.TruckGuid == truckGuid, truck => truck.MaxCapacity);
        }

        public Truck GetTruckByCustomerInfoGuid(Guid truckId)
        {
            throw new NotImplementedException();
        }

        public void UpdateCapacity(Guid truckGuid, TruckCapacityUnit capacity)
        {
            if (capacity == null) throw new ArgumentNullException();
            var truck = GetTruck(truckGuid);
            if (truck != null)
            {
                truck.MaxCapacity = capacity;
                TruckStore.Update(truck);
            }
        }

        public void UpdateLicenseNumber(Guid truckGuid, string licenseNumber)
        {
            var truck = GetTruck(truckGuid);
            truck.LicenseNumber = licenseNumber;
            TruckStore.Update(truck);
        }

        public void UpdateTruckLocation(Guid truckGuid, DbGeography location)
        {
            var truck = GetTruck(truckGuid);
            truck.Location = location;
            TruckStore.Update(truck);
        }
    }
}