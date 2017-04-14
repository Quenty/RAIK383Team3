using PracticalWerewolf.Models.Routes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PracticalWerewolf.Tests.Stores.DbContext
{
    public class MockRouteStopDbSet : MockDbSet<RouteStop>
    {
        public override RouteStop Find(params object[] keyValues)
        {
            foreach (var routeStop in _data)
            {
                if (keyValues.Contains(routeStop.RouteStopGuid))
                {
                    return routeStop;
                }
            }
            return null;
        }
    }
}
