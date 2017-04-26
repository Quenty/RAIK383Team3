using GoogleMapsApi.Entities.Directions.Response;
using log4net;
using PracticalWerewolf.Helpers;
using PracticalWerewolf.Models.Orders;
using PracticalWerewolf.Models.Routes;
using PracticalWerewolf.Models.UserInfos;
using PracticalWerewolf.Services.Interfaces;
using PracticalWerewolf.Stores.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace PracticalWerewolf.Services
{
    public class RouteStopService : IRouteStopService
    {
        private static ILog logger = LogManager.GetLogger(typeof(RouteStopService));
        private readonly IRouteStopStore RouteStopStore;

        public RouteStopService(IRouteStopStore store)
        {
            RouteStopStore = store;
        }

        public IEnumerable<RouteStop> GetContractorRouteAsNoTracking(ContractorInfo contractor)
        {
            if (contractor == null)
            {
                logger.Error("GetContractorRoute() - Null contractor");
                throw new ArgumentNullException();
            }

            return RouteStopStore.AsNoTracking()
                .Include(x => x.Order)
                .Where(x => x.Order.TrackInfo.OrderStatus == OrderStatus.Queued || x.Order.TrackInfo.OrderStatus == OrderStatus.InProgress)
                .Where(x => x.Order.TrackInfo.Assignee.ContractorInfoGuid == contractor.ContractorInfoGuid)
                .OrderBy(x => x.StopOrder);
        }

        public IEnumerable<RouteStop> GetContractorRoute(ContractorInfo contractor)
        {
            if (contractor == null)
            {
                logger.Error("GetContractorRoute() - Null contractor");
                throw new ArgumentNullException();
            }

            return RouteStopStore.AsQueryable()
                .Include(x => x.Order)
                .Where(x => x.Order.TrackInfo.OrderStatus == OrderStatus.InProgress)
                .Where(x => x.Order.TrackInfo.Assignee.ContractorInfoGuid == contractor.ContractorInfoGuid)
                .Where(x => x.Order.TrackInfo.CurrentTruck == null || x.Order.TrackInfo.CurrentTruck.TruckGuid != contractor.Truck.TruckGuid || x.Type == StopType.DropOff) // hasn't been picked up yet or is in truck
                .OrderBy(x => x.StopOrder);
        }

        public RouteStop GetContractorCurrentAssignment(ContractorInfo contractor)
        {
            if (contractor == null)
            {
                logger.Error("GetContractorRoute() - Null contractor");
                throw new ArgumentNullException();
            }

            return RouteStopStore.AsQueryable().Where(x => x.Order.TrackInfo.Assignee.ContractorInfoGuid == contractor.ContractorInfoGuid).FirstOrDefault(x => x.Order.TrackInfo.OrderStatus == OrderStatus.InProgress);
        }

        public string GetDistanceToNextStopInMiles(ContractorInfo contractor)
        {
            if (contractor == null)
            {
                logger.Error("GetDistanceToNextStopInMiles() - Null contractor");
                throw new ArgumentNullException();
            }

            DirectionsResponse directions;

            var nextStop = GetContractorCurrentAssignment(contractor);
            if(nextStop == null)
            {
                return null;
            }

            var address = nextStop.Type == StopType.PickUp ? nextStop.Order.RequestInfo.PickUpAddress : nextStop.Order.RequestInfo.DropOffAddress;

            if (contractor.Truck.Location != null)
            {
                directions = LocationHelper.GetRouteBetweenLocations(contractor.Truck.Location, address);
            }
            else
            {
                directions = LocationHelper.GetRouteBetweenLocations(contractor.HomeAddress, address);
            }

            if (directions.Status == DirectionsStatusCodes.OK)
            {
                return directions.Routes.First().Legs.First().Distance.Text;
            }
            else
            {
                logger.Error($"GetDistanceToNextStopInMiles() - Failed to load distance to next stop : {directions.ErrorMessage}");
                return null;
            }
        }

        public void UnassignOrderFromRouteStop(Order order)
        {
            if(order == null)
            {
                logger.Warn("Unassign null order");
                return;
            }

            var routeStops = RouteStopStore.AsQueryable()
                .Where(x => x.Order.OrderGuid == order.OrderGuid).ToList();

            foreach(var stop in routeStops)
            {
                RouteStopStore.Delete(stop);
            }
        }

        public void Update(RouteStop entity)
        {
            RouteStopStore.Update(entity);
        }

        public void Update(IEnumerable<RouteStop> entities)
        {
            foreach(RouteStop entity in entities)
            {
                RouteStopStore.Update(entity);
            }
        }

        public void Insert(RouteStop entity)
        {
            RouteStopStore.Insert(entity);
        }

        public void Attach(RouteStop entity)
        {
            RouteStopStore.Attach(entity);
        }

        public void Attach(IEnumerable<RouteStop> entities)
        {
            foreach(var entity in entities)
            {
                RouteStopStore.Attach(entity);
            }
        }

        public RouteStop GetRouteStop(Guid guid)
        {
            return RouteStopStore.AsNoTracking().Where(x => x.RouteStopGuid == guid).FirstOrDefault();
        }
    }
}