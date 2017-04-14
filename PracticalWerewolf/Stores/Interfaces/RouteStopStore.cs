using PracticalWerewolf.Models.Routes;

namespace PracticalWerewolf.Stores.Interfaces
{
    public class RouteStopStore : EntityStore<RouteStop>, IRouteStopStore
    {
        public RouteStopStore(IDbSetFactory context) : base(context) { }

    }
}