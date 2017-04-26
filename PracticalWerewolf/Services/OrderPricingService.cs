using log4net;
using PracticalWerewolf.Helpers;
using PracticalWerewolf.Models.Orders;
using PracticalWerewolf.Services.Interfaces;
using PracticalWerewolf.Stores.Interfaces;
using System;

namespace PracticalWerewolf.Services
{
    public class OrderPricingService : IOrderPricingService
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(OrderPricingService));

        private readonly IOrderStore OrderStore;

        public decimal CalculateOrderCost(Guid orderGuid)
        {
            Order order = OrderStore.Find(orderGuid);

            return CalculateOrderCost(order);
        }

        public decimal CalculateOrderCost(Order order)
        {
            if
        }

        private decimal GetFirstTierPricing(Order order)
        {

        }

        private decimal GetSecondTierPricing(Order order)
        {

        }

        private decimal GetThirdTierPricing(Order order)
        {

        }
    }
}