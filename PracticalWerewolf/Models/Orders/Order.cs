using System;
using System.ComponentModel.DataAnnotations;
using System.Device.Location;

namespace PracticalWerewolf.Models.Orders
{
    public class Order
    {
        [Key]
        public Guid OrderGuid { get; set; }             // An order has a one-to-many relationship to customers (a customer can have multiple orders)

        OrderRequestInfo RequestInfo { get; set; }
        OrderTrackInfo TrackInfo { get; set; }
    }
}
