using System;
using System.ComponentModel.DataAnnotations;
using System.Device.Location;

namespace PracticalWerewolf.Models.Orders
{
    public class Order
    {
        // An order has a one-to-many relationship to customers (a customer can have multiple orders)
        [Key]
        public Guid OrderGuid { get; set; }

        // One-to-one relationship, keeps track of who is driving and where it is going
        OrderRequestInfo RequestInfo { get; set; }
        // One-to-one relationship, keeps track of where the order is
        OrderTrackInfo TrackInfo { get; set; }          
    }
}
