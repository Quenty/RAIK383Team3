using System.Device.Location;
using PracticalWerewolf.Models.Trucks;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Data.Entity.Spatial;
using PracticalWerewolf.Models.Orders;

namespace PracticalWerewolf.Services.Interfaces
{
    public interface ITruckService
    {
        // ITruckStore.
        IEnumerable<Truck> GetAllTrucks();

        // Depends upon ITruckStore.Create
        void CreateTruck(Truck truck);

        // Depends upon ITruckStore.Update
        void UpdateTruckLocation(Guid truckGuid, DbGeography location);

        // Depends upon ITruckStore.Get
        Truck GetTruck(Guid truckGuid);

        // Depends upon ITruckStore.GetByCustomerInfoGuid
        Truck GetTruckByCustomerInfoGuid(Guid truckGuid);

        void UpdateCapacity(Guid truckGuid, TruckCapacityUnit capacity);

        void UpdateLicenseNumber(Guid truckGuid, string licenseNumber);

        void AddItemToTruck(Guid truckGuid, Order order);

        void RemoveItemFromTruck(Guid truckGuid, Order order);

    }
}
