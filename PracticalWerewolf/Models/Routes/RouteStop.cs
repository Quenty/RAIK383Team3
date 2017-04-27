using PracticalWerewolf.Models.Orders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Device.Location;
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
        public virtual Order Order { get; set; }

        public long EstimatedTicksToNextStop { get; set; }

        [NotMapped]
        public TimeSpan EstimatedTimeToNextStop
        {
            get
            {
                return TimeSpan.FromTicks(EstimatedTicksToNextStop);
            }
            set
            {
                EstimatedTicksToNextStop = value.Ticks;
            }
        }

        public int DistanceToNextStop { get; set; }

        [Required]
        public long StopOrder { get; set; }

        public CivicAddressDb Address
        {
            get
            {
                switch (Type)
                {
                    case StopType.PickUp:
                        return Order.RequestInfo.PickUpAddress;
                    case StopType.DropOff:
                        return Order.RequestInfo.DropOffAddress;
                    default:
                        return null;
                }
            }
        }
    }
}