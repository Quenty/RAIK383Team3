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
        TruckCapacityUnit Size { get; set; }

        
        DateTime RequestDate { get; set; }

        
        CivicAddress PickUpAddress { get; set; }

        
        CivicAddress DropOffAddress { get; set; }
    }
}