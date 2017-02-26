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
        void CreateOrder(Order order);

        Order GetOrder(Guid orderGuid);

        void UpdateOrder(Order order);

        void DeleteOrder(Guid orderGuid);
    }
}
