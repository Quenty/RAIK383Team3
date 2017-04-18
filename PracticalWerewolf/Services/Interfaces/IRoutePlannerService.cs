using PracticalWerewolf.Models.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PracticalWerewolf.Services.Interfaces
{
    public interface IRoutePlannerService
    {
        Task AssignOrders();
        Task AssignOrder(Order order);
    }
}
