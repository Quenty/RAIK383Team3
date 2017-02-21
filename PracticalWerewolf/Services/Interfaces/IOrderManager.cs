using System;
using PracticalWerewolf.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PracticalWerewolf.Services.Interfaces
{
    interface IOrderManager
    {
        void CreateOrder(IOrder order);

        IOrder GetOrder(Guid orderGuid);

        void UpdateOrder(IOrder order);

        void DeleteOrder(Guid orderGuid);
    }
}
