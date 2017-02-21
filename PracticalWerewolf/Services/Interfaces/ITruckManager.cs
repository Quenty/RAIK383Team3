using System;
using PracticalWerewolf.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PracticalWerewolf.Services.Interfaces
{
    interface ITruckManager
    {
        void CreateTruck(ITruck truck);

        ITruck GetTruck(Guid orderGuid);

        void UpdateTruck(ITruck truck);

        void DeleteTruck(Guid orderGuid);
    }
}
