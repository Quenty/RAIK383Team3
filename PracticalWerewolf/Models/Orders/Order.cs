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

        // zero-or-one-to-one relationship, keeps track of who placed the order and where it goes
        [Required]
        public virtual OrderRequestInfo RequestInfo { get; set; }

        // zero-or-one-to-one relationship, keeps track of where the order is
        [Required]
        public virtual OrderTrackInfo TrackInfo { get; set; }
    }
}
