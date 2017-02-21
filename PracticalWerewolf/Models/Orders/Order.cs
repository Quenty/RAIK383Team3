using System;
using System.ComponentModel.DataAnnotations;
using System.Device.Location;

namespace PracticalWerewolf.Models.Orders
{
    public class Order
    {
        [Key]
        public Guid OrderGuid { get; set; }

        OrderRequestInfo RequestInfo { get; set; }
        OrderTrackInfo TrackInfo { get; set; }
    }
}
