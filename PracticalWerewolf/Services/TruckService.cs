using PracticalWerewolf.Stores.Interfaces;
using PracticalWerewolf.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PracticalWerewolf.Models;
using PracticalWerewolf.Models.Trucks;

namespace PracticalWerewolf.Services
{
    public class TruckService : ITruckService
    {
        private ITruckStore TruckStore;

        public TruckService(ITruckStore store)
        {
            TruckStore = store;
        }

        public void CreateTruck(Truck truck)
        {
            throw new NotImplementedException();
        }

        public void DeleteTruck(Guid truckId)
        {
            throw new NotImplementedException();
        }

        public void UpdateTruck(Truck truck)
        {
            throw new NotImplementedException();
        }
    }
}
