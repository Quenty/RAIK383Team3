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
using System.Linq;
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
        private readonly ApplicationDbContext _dbContext;
        private readonly IUnitOfWork _unitOfWork;

        public RoutePlannerService(IOrderService orderService, IRouteStopService routeStopService, IContractorService contractorService, ApplicationDbContext dbContext, IUnitOfWork unitOfWork)
        {
            _orderService = orderService;
            _routeStopService = routeStopService;
            _contractorService = contractorService;
            _dbContext = dbContext;
            _unitOfWork = unitOfWork;
        }

        public async Task AssignOrders()
        {
            var orders = _orderService.GetUnassignedOrders().ToList();

            foreach(Order order in orders)
            {
                await AssignOrder(order);
            }

        }

        public async Task AssignOrder(Order order)
        {

            IEnumerable<ContractorInfo> contractors = null;

            try
            {
                contractors = _contractorService.GetAvailableContractorQuery().ToList();
                if (!contractors.Any())
                {
                    logger.Warn("AssignOrder() - No available contractors");
                    return;
                }
            }
            catch (Exception e)
            {
                var inner = e.InnerException;
                logger.Error("ya fuqed up");
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

            if (!options.Any())
            {
                logger.Warn("AssignOrder() - No routes were able to be determined");
                return;
            }

            ContractorRoutePlanner optimalPlan = GetOptimalPlan(options);

            //_routeStopService.Insert(optimalPlan.PickUp);
            //_routeStopService.Insert(optimalPlan.DropOff);

            //List<RouteStop> modifiedStops = optimalPlan.Route;
            //modifiedStops.Remove(optimalPlan.PickUp);
            //modifiedStops.Remove(optimalPlan.DropOff);
            //_routeStopService.Update(modifiedStops);
            _routeStopService.Attach(optimalPlan.Route);
            _routeStopService.Update(optimalPlan.Route);

            order.TrackInfo.Assignee = optimalPlan.Contractor;

            ApplicationUser user = _contractorService.GetUserByContractorInfo(optimalPlan.Contractor);
            await EmailHelper.SendWorkOrderEmail(user, optimalPlan.Order.RequestInfo);
            //_unitOfWork.SaveChanges();
        }

        private ContractorRoutePlanner GetOptimalPlan(IEnumerable<ContractorRoutePlanner> options)
        {
            return options.Aggregate((a, b) => a.DistanceChanged < b.DistanceChanged ? a : b);
        }
        
    }
}