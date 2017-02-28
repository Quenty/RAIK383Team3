using PracticalWerewolf.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PracticalWerewolf.Models.Orders;
using PracticalWerewolf.Models.Trucks;
using PracticalWerewolf.Models.UserInfos;

namespace PracticalWerewolf.Services
{
    public class OrderTrackService : IOrderTrackService
    {
        public IEnumerable<Order> GetContractorOrders(Guid contractorInfoGuid)
        {
            throw new NotImplementedException();
        }

        public void RejectOrder(Guid orderGuid)
        {
            throw new NotImplementedException();
        }

        public void UpdateOrderAssignee(Guid orderGuid, CustomerInfo customerInfo)
        {
            throw new NotImplementedException();
        }

        public void UpdateOrderStatus(Guid orderGuid, OrderStatus orderStatus)
        {
            throw new NotImplementedException();
        }

        public void UpdateOrderTruck(Guid orderGuid, Truck currentTruck)
        {
            throw new NotImplementedException();
        }
    }
}