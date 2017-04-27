using System;
using PracticalWerewolf.Models.Orders;
using System.Collections.Generic;
using PracticalWerewolf.Models.UserInfos;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace PracticalWerewolf.Services.Interfaces
{
    public interface IOrderService
    {
        IEnumerable<Order> GetUnassignedOrders();
        IEnumerable<Order> GetQueuedOrders(ContractorInfo contractor);
        IEnumerable<Order> GetQueuedOrders(Guid contractorInfoGuid);
        IEnumerable<Order> GetInprogressOrders(ContractorInfo contractor);
        IEnumerable<Order> GetInprogressOrdersNoTruck(ContractorInfo contractorinfo);
        IEnumerable<Order> GetInprogressOrdersNoTruck(Guid guid);
        IEnumerable<Order> GetInprogressOrdersInTruck(ContractorInfo contractor);
        IEnumerable<Order> GetDeliveredOrders(ContractorInfo contractor);
        void AssignOrder(Guid orderGuid, ContractorInfo contractor);
        void CancelOrder(Guid orderGuid);
        IEnumerable<Order> GetOrders();
        object GetOrders(CustomerInfo customerInfo);
        Task SetOrderAsComplete(Guid guid);
        void SetOrderInTruck(Guid orderId);
        void AssignOrders();
        void UnassignOrder(Order order);
        IEnumerable<Order> GetOrderHistory(Guid customerInfoGuid);
        Order GetOrder(Guid orderGuid);
        int QueryCount(Expression<Func<Order, bool>> where);
        void CreateOrder(Order order);
    }
}
