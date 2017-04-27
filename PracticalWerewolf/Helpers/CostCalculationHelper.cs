using log4net;
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

        public static decimal CalculateOrderCost(Order order)
        {
            var directions = LocationHelper.GetRouteBetweenLocations(order.RequestInfo.PickUpAddress, order.RequestInfo.DropOffAddress);
            decimal miles = directions.Routes.First().Legs.First().Distance.Value / METERS_PER_MILE;

            return order.RequestInfo.Size.CostMultiplier * miles;
        }

        public static decimal CalculateContractorPayment(IEnumerable<RouteStop> allStops)
        {
            //We are paying them $1 per mile
            decimal totalMeters = allStops.Sum(x => x.DistanceToNextStop);

            decimal miles = totalMeters / METERS_PER_MILE;

            return miles;
        }
    }
}