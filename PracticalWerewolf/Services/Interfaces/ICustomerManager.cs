using System;
using PracticalWerewolf.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PracticalWerewolf.Services.Interfaces
{
    interface ICustomerManager
    {

        //Create Order
        void CreateOrderRequest(IOrderRequestInfo orderRequestInfo);

        //See current orders
        IEnumerable<IOrder> GetCurrentOrders();

        //See order history
        IEnumerable<IOrder> GetPreviousOrders();

        //Change billing info
        void UpdateBillingInfo(IBillingInfo billingInfo);

        //Cancel order
        //In Order?
        //Maybe boolean to show pass/fail (if already in transit)
        void DeleteOrder();

        //See billing info
        IBillingInfo GetBillingInfo();

        //Change order
        //same as cancel?
        void UpdateOrder(IOrder order);
    }
}
