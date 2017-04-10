using PracticalWerewolf.Models.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PracticalWerewolf.Services.Interfaces
{
    public interface IOrderRequestService
    {
        // Depends upon IOrderStore.GetOrdersByCustomerInfoGuid
        IEnumerable<Order> GetCustomerOrders(Guid customerInfoGuid);

        // Depends upon IOrderStore.GetOrdersByCustomerInfoGuid
        IEnumerable<Order> GetCustomerOrders(Guid customerInfoGuid, OrderStatus orderStatus);

        void CreateOrderRequestInfo(OrderRequestInfo requestInfo);
    }
}
