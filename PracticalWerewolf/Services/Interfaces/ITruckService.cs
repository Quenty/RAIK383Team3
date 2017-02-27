using System.Device.Location;
using PracticalWerewolf.Models.Trucks;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;

namespace PracticalWerewolf.Services.Interfaces
{
    interface ITruckService
    {
        // Depends upon ITruckStore.Create
        void CreateTruck(Truck truck);

        // Depends upon ITruckStore.Update
        void UpdateTruckLocation(Guid truckGuid, GeoCoordinate location);

        // Depends upon ITruckStore.Update
        void UpdateTruckMaxCapacity(Guid truckGuid, TruckCapacityUnit capacity);

        // Depends upon ITruckStore.Update
        void UpdateTruckCurrentCapacity(Guid truckGuid, TruckCapacityUnit capacity);

        // Depends upon ITruckStore.Get
        Truck GetTruck(Guid truckId);

        // Depends upon ITruckStore.GetByCustomerInfoGuid
        Truck GetTruckByCustomerInfoGuid(Guid truckId);
    }
}
