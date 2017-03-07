using System;
using System.ComponentModel.DataAnnotations;
using System.Device.Location;

namespace PracticalWerewolf.Models.Orders
{
    public class Order
    {
        [Key]
        public Guid OrderGuid { get; set; }

        // One-to-one relationship, keeps track of who placed the order and where it goes
        public OrderRequestInfo RequestInfo { get; set; }
        // One-to-one relationship, keeps track of where the order is
        public OrderTrackInfo TrackInfo { get; set; }          
    }
}
