using System;
using PracticalWerewolf.Models.Trucks;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PracticalWerewolf.Services.Interfaces
{
    interface ITruckService
    {
        void CreateTruck(Truck truck);

        void UpdateTruck(Truck truck);

        void DeleteTruck(Guid orderGuid);
    }
}
