using System;
using PracticalWerewolf.Models.Orders;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PracticalWerewolf.Models.UserInfos;

namespace PracticalWerewolf.Services.Interfaces
{
    public interface IOrderService
    {
        // Depends upon IOrderStore.Create
        void CreateOrder(OrderRequestInfo order);

        // Depends upon IOrderStore.Get
        Order GetOrder(Guid orderGuid);

        // Depends upon IOrderStore.Get(OrderStatus orderStatus)
        IEnumerable<Order> GetOrders(OrderStatus orderStatus);

        // Depends upon IOrderStore.GetOrdersByCustomerInfoGuid
        IEnumerable<Order> GetOrdersByCustomerInfo(Guid customerInfoGuid);

        // Depends upon IOrderStore.GetByUserGuids
        IEnumerable<Order> GetByUserGuids(Guid userId);

        // Depends upon this.GetOrders
        IEnumerable<Order> GetQueuedOrders(ContractorInfo contractor);

        // Depends upon IOrderStore.Get
        IEnumerable<Order> GetOrders();

        // Depends upon IOrderRequestService.UpdateOrderStatus
        void CancelOrder(Guid orderGuid);
        IEnumerable<Order> GetInprogressOrders(ContractorInfo contractor);
    }
}
