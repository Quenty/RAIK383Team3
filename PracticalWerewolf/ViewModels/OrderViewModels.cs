using PracticalWerewolf.Models.Trucks;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Device.Location;
using System.Linq;
using System.Web;

namespace PracticalWerewolf.ViewModels.Orders
{
    public class CreateOrderRequestViewModel
    {
        [Required]
        public Guid CustomomerInfoGuid { get; set; }

        [Required]
        public CivicAddressDb PickUpAddress { get; set; }

        [Required]
        public TruckCapacityUnit Size { get; set; }

        [Required]
        public CivicAddressDb DropOffAddress { get; set; }
    }
}