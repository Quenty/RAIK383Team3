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
        CustomerInfo Requester { get; set; } 
        // One-to-one relationship                       
        TruckCapacityUnit Size { get; set; }
        [Required]
        DateTime RequestDate { get; set; }
        [Required]
        CivicAddress PickUpAddress { get; set; }
        [Required]
        CivicAddress DropOffAddress { get; set; }

    }
}