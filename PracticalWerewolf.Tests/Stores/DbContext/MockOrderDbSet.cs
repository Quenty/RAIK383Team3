using PracticalWerewolf.Models.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PracticalWerewolf.Tests.Stores.DbContext
{
    public class MockOrderDbSet : MockDbSet<Order>
    {
        public override Order Find(params object[] keyValues)
        {
            foreach (var order in _data)
            {
                if (keyValues.Contains(order.OrderGuid))
                {
                    return order;
                }
            }
            return null;
        }
    }
}
