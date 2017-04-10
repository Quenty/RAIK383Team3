using PracticalWerewolf.Models.Orders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PracticalWerewolf.Models.Routes
{
    public enum StopType { PickUp, DropOff }

    public class RouteStop
    {
        [Key]
        public Guid RouteStopGuid { get; set; }

        [Required]
        public StopType Type { get; set; }

        [Required]
        public Order Order { get; set; }

        public TimeSpan TimeToNextStop { get; set; }

        [Required]
        public long StopIndex { get; set; }
    }
}