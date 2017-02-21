using PracticalWerewolf.Repository.Interfaces;
using PracticalWerewolf.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PracticalWerewolf.Models;

namespace PracticalWerewolf.Services
{
    public class TruckManager : ITruckManager
    {
        private ITruckStore TruckStore;

        public TruckManager(ITruckStore store)
        {
            TruckStore = store;
        }

        public void CreateTruck(ITruck truck)
        {
            throw new NotImplementedException();
        }

        public void DeleteTruck(Guid orderGuid)
        {
            throw new NotImplementedException();
        }

        public ITruck GetTruck(Guid orderGuid)
        {
            throw new NotImplementedException();
        }

        public void UpdateTruck(ITruck truck)
        {
            throw new NotImplementedException();
        }
    }
}