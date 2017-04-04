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
        [Display(Name = "Pick up address", Description = "Where to pick up your package")]
        public CivicAddressDb PickUpAddress { get; set; }

        [Required]
        [Display(Name = "Package size", Description = "Approximately how big is your package")]
        public TruckCapacityUnit Size { get; set; }

        [Required]
        [Display(Name = "Drop off address", Description = "Where your package going")]
        public CivicAddressDb DropOffAddress { get; set; }
    }
}