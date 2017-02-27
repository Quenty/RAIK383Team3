using PracticalWerewolf.Models.Orders;
using PracticalWerewolf.Models.Trucks;
using PracticalWerewolf.Models.UserInfos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PracticalWerewolf.Services.Interfaces
{
    interface IOrderTrackService
    {
        // Depends upon IOrderStore
        IEnumerable<Order> GetCustomerOrders(Guid customerInfoGuid);

        // Depends upon IOrderStore.UpdateOrderStatus
        void RejectOrder(Guid orderGuid);

        // Depends upon IOrderStore
        void UpdateOrderStatus(Guid orderGuid, OrderStatus orderStatus);

        // Depends upon IOrderStore, ITruckService
        void UpdateOrderTruck(Guid orderGuid, Truck currentTruck);

        // Depends upon IOrderStore, ICustomerService
        void UpdateOrderAssignee(Guid orderGuid, CustomerInfo customerInfo);
    }
}
