using PracticalWerewolf.Models.Orders;
using PracticalWerewolf.Stores.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PracticalWerewolf.Stores
{
    public class OrderTrackInfoStore : EntityStore<OrderTrackInfo>, IOrderTrackInfoStore
    {
        public OrderTrackInfoStore(IDbSetFactory context) : base(context)
        {

        }
    }
}