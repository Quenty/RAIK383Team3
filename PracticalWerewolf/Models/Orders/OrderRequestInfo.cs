using PracticalWerewolf.Models.Trucks;
using PracticalWerewolf.Models.UserInfos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Device.Location;
using System.Linq;
using System.Web;

namespace PracticalWerewolf.Models.Orders
{
    public class OrderRequestInfo
    {
        [Key]
        public Guid OrderRequestInfoGuid { get; set; }

        // One-to-many relationship, a customer can own multiple orders
        [Required]
        public virtual CustomerInfo Requester { get; set; } 

        // One-to-one relationship
        [Required]
        public virtual TruckCapacityUnit Size { get; set; }

        [Required]
        [Display(Name = "Date Requested")]
        public DateTime RequestDate { get; set; }

        [Required]
        [Display(Name = "Pick Up Address")]
        public virtual CivicAddressDb PickUpAddress { get; set; }

        [Required]
        [Display(Name = "Drop Off Address")]
        public virtual CivicAddressDb DropOffAddress { get; set; }
    }
}