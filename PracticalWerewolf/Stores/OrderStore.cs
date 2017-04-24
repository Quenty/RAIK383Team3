using PracticalWerewolf.Models;
using PracticalWerewolf.Models.Orders;
using PracticalWerewolf.Models.Trucks;
using PracticalWerewolf.Stores.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PracticalWerewolf.Stores
{
    public class OrderStore : EntityStore<Order>, IOrderStore
    {
        public OrderStore(IDbSetFactory context) : base(context)
        {
            
        }
    }
}
