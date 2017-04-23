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
        IEnumerable<Order> GetQueuedOrders(Guid contractorInfoGuid);
        IEnumerable<Order> GetInprogressOrders(ContractorInfo contractor);
        IEnumerable<Order> GetDeliveredOrders(ContractorInfo contractor);
        void AssignOrder(Guid orderGuid, ContractorInfo contractor);
        void CancelOrder(Guid orderGuid);
        object GetOrders();
        object GetOrders(CustomerInfo customerInfo);
        void SetOrderAsComplete(Guid guid);
        void AssignOrders();
        void UnqueueOrder(Order order, Guid contractorInfoGuid);
    }
}
