using log4net;
using PracticalWerewolf.Helpers.Interfaces;
using PracticalWerewolf.Models.Orders;
using PracticalWerewolf.Models.Routes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PracticalWerewolf.Helpers
{
    public class CostCalculationHelper
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(CostCalculationHelper));
        private static readonly decimal METERS_PER_MILE = 1609.344m;
        private static readonly decimal FIXED_MILES = 50;

        private readonly ILocationHelper LocationHelper;

        public CostCalculationHelper(ILocationHelper locationHelper)
        {
            LocationHelper = locationHelper;
        }

        public decimal CalculateOrderCost(OrderRequestInfo requestInfo)
        {
            var directions = LocationHelper.GetDirections(requestInfo.PickUpAddress, requestInfo.DropOffAddress);
            decimal miles = directions.Distance / METERS_PER_MILE;

            return requestInfo.Size.CostMultiplier * (FIXED_MILES + miles);
        }

        public decimal CalculateContractorPayment(IEnumerable<RouteStop> allStops)
        {
            //We are paying them $1 per mile
            decimal totalMeters = allStops.Sum(x => x.DistanceToNextStop);

            decimal miles = totalMeters / METERS_PER_MILE;

            return miles;
        }
    }
}