using System;
using System.ComponentModel.DataAnnotations;
using System.Device.Location;

namespace PracticalWerewolf.Models.Orders
{
    public class Order
    {
        [Key]
        public Guid OrderGuid { get; set; }             // Primary key for Order table

        OrderRequestInfo RequestInfo { get; set; }      // One-to-one relationship
        OrderTrackInfo TrackInfo { get; set; }          // One-to-one relationship
    }
}
