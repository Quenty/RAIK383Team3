using System;
using PracticalWerewolf.Models;
using PracticalWerewolf.Models.UserInfos;
using PracticalWerewolf.Models.Orders;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PracticalWerewolf.Services.Interfaces
{
    interface IContractorService
    {
        //View Current Orders
        IEnumerable<Order> GetCurrentOrders(Guid contractorGuid);
 
        //Change Status
        void ChangeContractorStatus(Boolean status);

        //Update track info
        void UpdateOrderTrackInfo(OrderTrackInfo orderTrackInfo);

        //reject Order
        void RejectOrder(Guid orderGuid);

        //View Customer Info
        CustomerInfo ViewCustomerInfo(Guid customerInfoGuid);

        //View Current Orders
        IEnumerable<Order> GetPreviousOrders(Guid contractorGuid);
    }
}
