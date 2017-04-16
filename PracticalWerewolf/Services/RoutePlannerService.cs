using log4net;
using PracticalWerewolf.Application;
using PracticalWerewolf.Helpers;
using PracticalWerewolf.Models;
using PracticalWerewolf.Models.Orders;
using PracticalWerewolf.Models.Routes;
using PracticalWerewolf.Models.UserInfos;
using PracticalWerewolf.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PracticalWerewolf.Services
{
    public class RoutePlannerService
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(RoutePlannerService));
        private readonly IRouteStopService _routeStopService;
        private readonly IContractorService _contractorService;
        private readonly ApplicationDbContext _dbContext;

        public RoutePlannerService(IRouteStopService routeStopService, IContractorService contractorService, ApplicationDbContext dbContext)
        {
            _routeStopService = routeStopService;
            _contractorService = contractorService;
            _dbContext = dbContext;
        }

        public async void AssignOrder(Order order)
        {
            IEnumerable<ContractorInfo> contractors = _contractorService.GetAvailableContractorQuery();
            if (!contractors.Any())
            {
                logger.Warn("AssignOrder() - No available contractors");
                return;
            }

            List<RoutePlannerDelegate> options = new List<RoutePlannerDelegate>();

            foreach (ContractorInfo contractor in contractors)
            {
                try
                {
                    List<RouteStop> route = _routeStopService.GetContractorRouteAsNoTracking(contractor).ToList();
                    var planner = new RoutePlannerDelegate(contractor, order, route);
                    planner.CalculateOptimalRoute();
                    if (planner.WillWork)
                    {
                        options.Add(planner);
                    }
                }
                catch (Exception e)
                {
                    logger.Error($"AssignOrder() - Error with RoutePlannerDelegate with ContractorInfoGuid {contractor.ContractorInfoGuid.ToString()} - {e.ToString()}");
                }
            }

            if (!options.Any())
            {
                logger.Warn("AssignOrder() - No routes were able to be determined");
                return;
            }

            RoutePlannerDelegate optimalPlan = GetOptimalPlan(options);

            _routeStopService.Insert(optimalPlan.PickUp);
            _routeStopService.Insert(optimalPlan.DropOff);

            List<RouteStop> modifiedStops = optimalPlan.Route;
            modifiedStops.Remove(optimalPlan.PickUp);
            modifiedStops.Remove(optimalPlan.DropOff);
            _routeStopService.Update(modifiedStops);

            order.TrackInfo.Assignee = optimalPlan.Contractor;

            ApplicationUser user = _contractorService.GetUserByContractorInfo(optimalPlan.Contractor);
            await EmailHelper.SendWorkOrderEmail(user, optimalPlan.Order.RequestInfo);
        }

        private RoutePlannerDelegate GetOptimalPlan(IEnumerable<RoutePlannerDelegate> options)
        {
            return options.Aggregate((a, b) => a.DistanceChanged < b.DistanceChanged ? a : b);
        }
        
    }
}