using PracticalWerewolf.Models.UserInfos;
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
        [Required]
        public double Mass { get; set; }
        [Required]
        public double Volume { get; set; }
    }

    public class Truck
    {
        // A truck has a One-to-one relationship with contractor
        [Key]
        public Guid TruckGuid { get; set; }
        [Required]
        public String LicenseNumber { get; set; }
                                                                
        // Gets broken down into different props but stays in the truck table
        public DbGeography Location { get; set; } 

        public TruckCapacityUnit AvailableCapacity {
            get
            {
                return new TruckCapacityUnit
                {
                    TruckCapacityUnitGuid = Guid.NewGuid(),
                    Mass = MaxCapacity.Mass - UsedCapacity.Mass,
                    Volume = MaxCapacity.Volume - UsedCapacity.Volume
                };
            }
        }

        //TODO Calculate from orders associated with truck
        public TruckCapacityUnit UsedCapacity { get; set; }
        [Required]
        public TruckCapacityUnit MaxCapacity { get; set; }
    }
}