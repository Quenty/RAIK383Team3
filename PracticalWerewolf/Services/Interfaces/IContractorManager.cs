using System;
using PracticalWerewolf.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PracticalWerewolf.Services.Interfaces
{
    interface IContractorManager
    {
        //View Current Orders
        IEnumerable<IOrder> GetCurrentOrderInfo();

        //Change Status
        void UpdateStatus(Boolean status);

        //Update track info
        void UpdateOrderTrackInfo(IOrderTrackInfo orderTrackInfo);

        //reject Order
        void RejectOrder();

        //View Customer Info
        //Should this be in Orders instead?
        ICustomerInfo GetICustomerInfo();

        //View Current Orders
        IEnumerable<IOrder> GetPreviousOrderInfo();
    }
}
