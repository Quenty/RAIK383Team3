using PracticalWerewolf.Models;
using PracticalWerewolf.Models.Orders;
using PracticalWerewolf.Models.Trucks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PracticalWerewolf.Stores.Interfaces
{
    public interface IOrderStore : IEntityStore<Order>
    {
    }
}
