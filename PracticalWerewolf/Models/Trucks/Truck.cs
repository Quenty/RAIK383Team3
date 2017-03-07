﻿using PracticalWerewolf.Models.UserInfos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Spatial;
using System.Device.Location;
using System.Linq;
using System.Web;

namespace PracticalWerewolf.Models.Trucks
{

    public class TruckCapacityUnit
    {
        [Key]
        public Guid TruckCapacityUnitGuid { get; set; }
    }

    public class Truck
    {
        // A truck has a One-to-one relationship with contractor
        [Key]
        public Guid TruckGuid { get; set; }
                                                                
        // Gets broken down into different props but stays in the truck table
        public DbGeography Location { get; set; } 

        public TruckCapacityUnit AvailableCapacity { get; }
        // One-to-one, each truck will have one Current and Max capacity
        public TruckCapacityUnit CurrentCapacity { get; set; }
        public TruckCapacityUnit MaxCapacity { get; set; }
    }
}