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
        public Guid OrderRequestInfoGuid { get; set; }              // One-to-one relationship with order

        CustomerInfo Requester { get; set; }                        // One-to-many relationship, a customer can own multiple orders
        TruckCapacityUnit Size { get; set; }                        // One-to-one relationship

        [Required]
        DateTime RequestDate { get; set; }                          // One-to-one relationship

        [Required]
        CivicAddress PickUpAddress { get; set; }                    // One-to-one relationship

        [Required]
        CivicAddress DropOffAddress { get; set; }                   // One-to-one relationship
    }
}