using PracticalWerewolf.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PracticalWerewolf.Models.Orders;
using PracticalWerewolf.Stores.Interfaces;


namespace PracticalWerewolf.Services
{
    public class OrderRequestService : IOrderRequestService
    {
        private readonly IOrderStore OrderStore;

        public OrderRequestService(IOrderStore OrderStore)
        {
            this.OrderStore = OrderStore;
        }

        public IEnumerable<Order> GetCustomerOrders(Guid customerInfoGuid)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Order> GetCustomerOrders(Guid customerInfoGuid, OrderStatus orderStatus)
        {
            throw new NotImplementedException();
        }

        public void CreateOrderRequestInfo(OrderRequestInfo requestInfo)
        {
            if (requestInfo.OrderRequestInfoGuid == Guid.Empty)
            {
                throw new ArgumentException("Empty GUID in OrderRequestInfo");
            }

            OrderStore.Insert(new Order
            {
                OrderGuid = Guid.NewGuid(),
                RequestInfo = requestInfo,
                TrackInfo = new OrderTrackInfo
                {
                    OrderTrackInfoGuid = Guid.NewGuid(),
                    OrderStatus = OrderStatus.Queued
                }
            });
        }
    }
}