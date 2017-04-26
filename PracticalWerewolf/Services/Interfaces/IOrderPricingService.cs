using PracticalWerewolf.Models.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PracticalWerewolf.Services.Interfaces
{
    public interface IOrderPricingService
    {
        decimal CalculateOrderCost(Guid orderGuid);
        decimal CalculateOrderCost(Order order);
    }
}
