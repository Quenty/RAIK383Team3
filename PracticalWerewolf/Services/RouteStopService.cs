using log4net;
using PracticalWerewolf.Models.Orders;
using PracticalWerewolf.Models.Routes;
using PracticalWerewolf.Models.UserInfos;
using PracticalWerewolf.Services.Interfaces;
using PracticalWerewolf.Stores.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PracticalWerewolf.Services
{
    public class RouteStopService : IRouteStopService
    {
        private static ILog logger = LogManager.GetLogger(typeof(RouteStopService));
        private readonly IRouteStopStore _routeStopStore;

        public RouteStopService(IRouteStopStore store)
        {
            _routeStopStore = store;
        }

        public IEnumerable<RouteStop> GetContractorRoute(ContractorInfo contractor)
        {
            if (contractor == null)
            {
                logger.Error("GetContractorRoute() - Null contractor");
                throw new ArgumentNullException();
            }

            return _routeStopStore.AsQueryable()
                .Where(x => x.Order.TrackInfo.OrderStatus == OrderStatus.Queued || x.Order.TrackInfo.OrderStatus == OrderStatus.InProgress)
                .Where(x => x.Order.TrackInfo.Assignee.ContractorInfoGuid == contractor.ContractorInfoGuid)
                .OrderBy(x => x.StopOrder);
        }

        public RouteStop GetContractorCurrentAssignment(ContractorInfo contractor)
        {
            if (contractor == null)
            {
                logger.Error("GetContractorRoute() - Null contractor");
                throw new ArgumentNullException();
            }

            return _routeStopStore.AsQueryable().Where(x => x.Order.TrackInfo.Assignee.ContractorInfoGuid == contractor.ContractorInfoGuid).FirstOrDefault(x => x.Order.TrackInfo.OrderStatus == OrderStatus.InProgress);
        }
    }
}