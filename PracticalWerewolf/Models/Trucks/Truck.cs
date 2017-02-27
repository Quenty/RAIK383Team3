using PracticalWerewolf.Models.UserInfos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Device.Location;
using System.Linq;
using System.Web;

namespace PracticalWerewolf.Models.Trucks
{

    public class TruckCapacityUnit
    {
        [Key]
        public Guid TruckCapacityUnitGuid { get; set; }             // One-to-one relationship with truck
    }

    public class Truck
    {
        [Key]
        public Guid TruckGuid { get; set; }                         // One-to-one relationship with contractor


        public GeoCoordinate Location { get; set; }                 // One-to-one relationship 

        public TruckCapacityUnit AvailableCapacity { get; }         // One-to-one relationship
        public TruckCapacityUnit CurrentCapacity { get; set; }      // One-to-one relationship
        public TruckCapacityUnit MaxCapacity { get; set; }          // One-to-one relationship
    }
}