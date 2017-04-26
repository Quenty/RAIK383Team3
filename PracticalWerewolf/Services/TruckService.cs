using PracticalWerewolf.Services.Interfaces;
using System;
using System.Collections.Generic;
using PracticalWerewolf.Models.Trucks;
using System.Device.Location;
using PracticalWerewolf.Stores.Interfaces;
using PracticalWerewolf.Models;
using PracticalWerewolf.Controllers.UnitOfWork;
using System.Data.Entity.Spatial;
using log4net;
using PracticalWerewolf.Models.Orders;

namespace PracticalWerewolf.Services
{
    public class TruckService : ITruckService
    {
        private readonly ILog logger = LogManager.GetLogger(typeof(TruckService));
        private readonly ITruckStore TruckStore;
        private readonly IOrderService OrderService;

        public TruckService(ITruckStore TruckStore, IOrderService OrderService)
        {
            this.TruckStore = TruckStore;
            this.OrderService = OrderService;
        }

        public void AddItemToTruck(Truck truck, Order order)
        {
            if (truck == null || order == null)
            {
                logger.Error("Could not add item to truck becasue item or truck was null");
                throw new ArgumentNullException();
            }

            truck.CurrentOrders.Add(order.TrackInfo);
            TruckCapacityUnit capacity;
            if (truck.UsedCapacity != null)
            {
                capacity = new TruckCapacityUnit
                {
                    TruckCapacityUnitGuid = Guid.NewGuid(),
                    Mass = (truck.UsedCapacity.Mass + order.RequestInfo.Size.Mass),
                    Volume = (truck.UsedCapacity.Volume + order.RequestInfo.Size.Volume)
                };
            }
            else
            {
                capacity = new TruckCapacityUnit
                {
                    TruckCapacityUnitGuid = Guid.NewGuid(),
                    Mass = order.RequestInfo.Size.Mass,
                    Volume = order.RequestInfo.Size.Volume
                };
            }
            truck.MaxCapacity = truck.MaxCapacity;
            truck.UsedCapacity = capacity;
            TruckStore.Update(truck);
        }

        public void RemoveItemFromTruck(Truck truck, Order order)
        {
            if (truck == null || order == null)
            {
                logger.Error("Could not add item to truck becasue item or truck guid was null");
                throw new ArgumentNullException();
            }
            truck.CurrentOrders.Remove(order.TrackInfo);

            var capacity = new TruckCapacityUnit
            {
                TruckCapacityUnitGuid = truck.UsedCapacity.TruckCapacityUnitGuid,
                Mass = (truck.UsedCapacity.Mass - order.RequestInfo.Size.Mass),
                Volume = (truck.UsedCapacity.Volume - order.RequestInfo.Size.Volume)
            };
            truck.UsedCapacity = capacity;
            TruckStore.Update(truck);
        }




        public void CreateTruck(Truck truck)
        {
            if (truck == null)
            {
                logger.Error("CreateTruck() - null Truck passed in");
                throw new ArgumentNullException();
            }
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
            if (capacity == null)
            {
                logger.Error("UpdateCapacity() - null TruckCapacityUnit passed in");
                throw new ArgumentNullException("TruckCapacityUnit is null");
            }
            var truck = GetTruck(truckGuid);
            if (truck != null)
            {
                truck.MaxCapacity = capacity;
                TruckStore.Update(truck);
            }
            else
            {
                throw new Exception("Truck does not exist in database!");
            }
        }

        public void UpdateUsedCapacity(Guid truckGuid, TruckCapacityUnit capacity)
        {
            if (capacity == null)
            {
                logger.Error("UpdateUsedCapacity() - null TruckCapacityUnit passed in");
                throw new ArgumentNullException("TruckCapacityUnit is null");
            }
            var truck = GetTruck(truckGuid);
            if (truck != null)
            {
                truck.UsedCapacity = capacity;
                TruckStore.Update(truck);
            }
            else
            {
                throw new Exception("Truck does not exist in database!");
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