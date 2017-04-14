using PracticalWerewolf.Application;
using PracticalWerewolf.Models.Orders;
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
        private readonly IRouteStopService _routeStopService;
        private readonly IContractorService _contractorService;
        private readonly ApplicationDbContext _dbContext;

        public RoutePlannerService(IRouteStopService routeStopService, IContractorService contractorService, ApplicationDbContext dbContext)
        {
            _routeStopService = routeStopService;
            _contractorService = contractorService;
            _dbContext = dbContext;
        }

        public void AssignOrder(Order order)
        {
            IEnumerable<ContractorInfo> contractors = _contractorService.getAvailableContractorQuery();


        }
    }
}