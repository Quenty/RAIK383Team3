using PracticalWerewolf.Models;
using PracticalWerewolf.Models.Trucks;
using PracticalWerewolf.Models.Orders;
using PracticalWerewolf.ViewModels.Contractor;
using PracticalWerewolf.ViewModels.Paged;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Device.Location;
using System.Linq;
using System.Web;

namespace PracticalWerewolf.ViewModels.Orders
{
    public class OrderIndex
    {
        public PagedOrderListViewModel PagedOrderListViewModel { get; set; }
    }

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
    public class ConfirmationViewModel
    {
        public Guid Guid { get; set; }
    }

    public class OrderDetailsViewModel
    {
        [Display(Name = "Pick up address", Description = "Where to pick up your package")]
        public CivicAddressDb PickUpAddress { get; set; }

        [Display(Name = "Package size", Description = "Approximately how big is your package")]
        public TruckCapacityUnit Size { get; set; }

        [Display(Name = "Drop off address", Description = "Where your package going")]
        public CivicAddressDb DropOffAddress { get; set; }

        [Display(Name = "Date Requested")]
        public DateTime RequestDate { get; set; }

        [Display(Name = "Customer "  )]
        public ApplicationUser Customer { get; set; }

        [Display(Name = "Contractor ")]
        public ApplicationUser Contractor { get; set; }

    }

    public class OrderStatusViewModel
    {
        public Guid orderId { get; set; }
        public OrderStatus orderStatus { get; set; }
        public Boolean inTruck { get; set; }
        public string message { get; set; }
    }

}