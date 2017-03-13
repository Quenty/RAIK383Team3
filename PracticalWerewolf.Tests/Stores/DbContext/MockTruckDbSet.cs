using PracticalWerewolf.Models.Trucks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PracticalWerewolf.Tests.Stores.DbContext
{
    class MockTruckDbSet : MockDbSet<Truck>
    {
        public override Truck Find(params object[] keyValues)
        {
            foreach(var truck in _data)
            {
                if (keyValues.Contains(truck.TruckGuid))
                {
                    return truck;
                }
            }
            return null;
        }
    }
}
