using System;
using PracticalWerewolf.Models.Orders;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PracticalWerewolf.Services.Interfaces
{
    interface IOrderService
    {
        // Depends upon IOrderRequestService.UpdateOrderStatus
        void CancelOrder(Guid orderGuid);
    }
}
