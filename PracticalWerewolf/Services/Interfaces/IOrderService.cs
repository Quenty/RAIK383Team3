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
        IEnumerable<Order> GetUnassignedOrders();
        IEnumerable<Order> GetQueuedOrders(ContractorInfo contractor);
        IEnumerable<Order> GetInprogressOrders(ContractorInfo contractor);
        IEnumerable<Order> GetDeliveredOrders(ContractorInfo contractor);
        IEnumerable<Order> GetInprogressOrdersNoTruck(ContractorInfo contractorinfo);
        IEnumerable<Order> GetInprogressOrdersNoTruck(Guid guid);
        IEnumerable<Order> GetInprogressOrdersInTruck(ContractorInfo contractor);
        void AssignOrder(Guid orderGuid, ContractorInfo contractor);
        void CancelOrder(Guid orderGuid);
        IEnumerable<Order> GetOrders();
        object GetOrders(CustomerInfo customerInfo);
        void SetOrderAsComplete(Guid guid);
        void SetOrderInTruck(Guid orderId);
        void AssignOrders();
        IEnumerable<Order> GetOrderHistory(Guid customerInfoGuid);
        Order GetOrder(Guid orderGuid);
        void SetOrderAsInprogress(Guid orderGuid);
    }
}
