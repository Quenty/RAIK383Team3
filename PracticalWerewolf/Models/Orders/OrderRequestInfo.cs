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
        public CustomerInfo Requester { get; set; } 
        // One-to-one relationship                       
        public TruckCapacityUnit Size { get; set; }

        [Required]
        public DateTime RequestDate { get; set; }

        [Required]
        public CivicAddressDb PickUpAddress { get; set; }

        [Required]
        public CivicAddressDb DropOffAddress { get; set; }

    }
}