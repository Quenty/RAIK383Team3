using PracticalWerewolf.Models.Orders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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

        private long EstimatedTicksToNextStop { get; set; }

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

        [Required]
        public long StopOrder { get; set; }
    }
}