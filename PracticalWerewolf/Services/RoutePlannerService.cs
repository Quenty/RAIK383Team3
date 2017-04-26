using log4net;
using PracticalWerewolf.Application;
using PracticalWerewolf.Controllers.UnitOfWork;
using PracticalWerewolf.Helpers;
using PracticalWerewolf.Models;
using PracticalWerewolf.Models.Orders;
using PracticalWerewolf.Models.Routes;
using PracticalWerewolf.Models.UserInfos;
using PracticalWerewolf.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace PracticalWerewolf.Services
{
    public class RoutePlannerService : IRoutePlannerService
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(RoutePlannerService));

        private readonly IOrderService _orderService;
        private readonly IRouteStopService _routeStopService;
        private readonly IContractorService _contractorService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly EmailService _emailService;

        public RoutePlannerService(IOrderService orderService, IRouteStopService routeStopService, IContractorService contractorService, IUnitOfWork unitOfWork, EmailService emailService)
        {
            _orderService = orderService;
            _routeStopService = routeStopService;
            _contractorService = contractorService;
            _unitOfWork = unitOfWork;
            _emailService = emailService;
        }

        public void AssignOrders()
        {
            var orders = _orderService.GetUnassignedOrders().ToList();

            foreach(Order order in orders)
            {
                AssignOrder(order);
            }
        }

        public void AssignOrder(Order order)
        {

            IEnumerable<ContractorInfo> contractors = null;

            contractors = _contractorService.GetAvailableContractorQuery().ToList();

            if (!contractors.Any())
            {
                logger.Warn("AssignOrder() - No available contractors");
                return;
            }

            List<ContractorRoutePlanner> options = new List<ContractorRoutePlanner>();

            foreach (ContractorInfo contractor in contractors)
            {
                try
                {
                    List<RouteStop> route = _routeStopService.GetContractorRouteAsNoTracking(contractor).ToList();
                    var planner = new ContractorRoutePlanner(contractor, order, route);
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

            foreach(ContractorRoutePlanner option in options)
            {
                Debug.Assert(option.Contractor != null);
                Debug.Assert(option.WillWork);
                Debug.Assert(option.DistanceChanged > 0);
                Debug.Assert(option.PickUp != null);
                Debug.Assert(option.DropOff != null);
                Debug.Assert(option.Order != null);
            }

            if (!options.Any())
            {
                logger.Warn("AssignOrder() - No routes were able to be determined");
                return;
            }

            ContractorRoutePlanner optimalPlan = GetOptimalPlan(options);

            List<RouteStop> trackedRoute = _routeStopService.GetContractorRoute(optimalPlan.Contractor).ToList();

            _routeStopService.Insert(optimalPlan.PickUp);
            _routeStopService.Insert(optimalPlan.DropOff);

            int pickUpIndex = optimalPlan.Route.IndexOf(optimalPlan.PickUp);
            int dropOffIndex = optimalPlan.Route.IndexOf(optimalPlan.DropOff);

            optimalPlan.PickUp.Order = order;
            optimalPlan.DropOff.Order = order;

            trackedRoute.Insert(pickUpIndex, optimalPlan.PickUp);
            trackedRoute.Insert(dropOffIndex, optimalPlan.DropOff);

            _orderService.AssignOrder(order.OrderGuid, optimalPlan.Contractor);

            ApplicationUser user = _contractorService.GetUserByContractorInfo(optimalPlan.Contractor);
            _emailService.SendWorkOrderEmail(user, optimalPlan.Order);

            _unitOfWork.SaveChanges();
        }

        private ContractorRoutePlanner GetOptimalPlan(IEnumerable<ContractorRoutePlanner> options)
        {
            var optimalRoute = options.First();

            foreach(var option in options)
            {
                //This assumes that the addition of a new order will not cause the route total distance to decrease
                if(option.DistanceChanged < optimalRoute.DistanceChanged && option.DistanceChanged >= 0)
                {
                    optimalRoute = option;
                }
            }

            return optimalRoute;
        }
        
    }
}