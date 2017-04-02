using PracticalWerewolf.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PracticalWerewolf.Models.Orders;
using PracticalWerewolf.Stores.Interfaces;
using PracticalWerewolf.Models.UserInfos;

namespace PracticalWerewolf.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderStore OrderStore;

        public OrderService(IOrderStore OrderStore)
        {
            this.OrderStore = OrderStore;
        }

        public Order GetOrder(Guid orderGuid)
        {
            Order order = OrderStore.Single(o => o.OrderGuid == orderGuid);
            if (order == null)
            {
                throw new ArgumentException("Order does not exist in database");
            }
            return order;
        }

        public void CancelOrder(Guid orderGuid)
        {
            Order order = GetOrder(orderGuid);

            OrderTrackInfo orderTrackInfo = order.TrackInfo ?? new OrderTrackInfo();
            orderTrackInfo.OrderStatus = OrderStatus.Cancelled;
            OrderStore.Update(order);
        }

        public void AssignOrder(Guid orderGuid, ContractorInfo contractor)
        {
            Order order = GetOrder(orderGuid);
            OrderTrackInfo orderTrackInfo = order.TrackInfo ?? new OrderTrackInfo();
            orderTrackInfo.OrderStatus = OrderStatus.InProgress;
            orderTrackInfo.Assignee = contractor;
            OrderStore.Update(order);
        }

    }
}