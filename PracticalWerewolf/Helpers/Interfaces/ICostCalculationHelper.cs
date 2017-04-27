using PracticalWerewolf.Models.Orders;
using PracticalWerewolf.Models.Routes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PracticalWerewolf.Helpers.Interfaces
{
    public interface ICostCalculationHelper
    {
        decimal CalculateOrderCost(OrderRequestInfo requestInfo);
        decimal CalculateContractorPayment(IEnumerable<RouteStop> allStops);
    }
}
