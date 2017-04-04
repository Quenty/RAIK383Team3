using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Device.Location;

namespace PracticalWerewolf.Models.Orders
{
    public class Order
    {
        [Key]
        public Guid OrderGuid { get; set; }
        public virtual OrderRequestInfo RequestInfo { get; set; }
        public virtual OrderTrackInfo TrackInfo { get; set; }
    }
}
