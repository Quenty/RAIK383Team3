using System;
using PracticalWerewolf.Models;
using PracticalWerewolf.Models.Orders;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PracticalWerewolf.Services.Interfaces
{
    interface ICustomerService
    {

        //Create Order
        void CreateOrderRequest(OrderRequestInfo orderRequestInfo);

        //See current orders
        IEnumerable<Order> ViewCurrentOrders(Guid customerInfoId);

        //See order history
        IEnumerable<Order> ViewPreviousOrders(Guid customerInfoId);

        //Change billing info
        //void UpdateBillingInfo(BillingInfo billingInfo);
        //TODO?

        //Cancel order
        void DeleteOrder(Guid orderGuid);

        //See billing info
        //IBillingInfo GetBillingInfo();

        //Change order
        void ChangeOrder(Order order);

        //Cancel order
        void CancelOrder(Guid orderGuid);
    }
}
