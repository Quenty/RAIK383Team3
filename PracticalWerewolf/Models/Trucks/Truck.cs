﻿using PracticalWerewolf.Models.Orders;
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
        public static readonly TruckCapacityUnit Zero = new TruckCapacityUnit {
            Volume = 0,
            Mass = 0
        };

        public TruckCapacityUnit()
        {
        }

        public TruckCapacityUnit(TruckCapacityUnit copy)
        {
            TruckCapacityUnitGuid = copy.TruckCapacityUnitGuid;
            Volume = copy.Volume;
            Mass = copy.Mass;
        }

        [Key]
        public Guid TruckCapacityUnitGuid { get; set; }

        [Required]
        [Display(Name = "Weight (lb)")]
        public double Mass { get; set; }

        [Required]
        [Display(Name = "Volume (cubic ft)")]
        public double Volume { get; set; }

        public decimal CostMultiplier
        {
            get
            {
                if (Mass >= 1000 || Volume >= 1000)
                {
                    return 10m;
                }
                else if (Mass >= 500 || Volume >= 500)
                {
                    return 5.0m;
                }
                else if (Mass >= 50 || Volume >= 50)
                {
                    return 1.0m;
                }
                else
                {
                    return 0.50m;
                }
            }
        }


        public static TruckCapacityUnit operator +(TruckCapacityUnit capacity1, TruckCapacityUnit capacity2)
        {
            return new TruckCapacityUnit
            {
                Mass = capacity1.Mass + capacity2.Mass,
                Volume = capacity1.Volume + capacity2.Volume
            };
        }

        public override bool Equals(object obj)
        {
            if (obj is TruckCapacityUnit)
            {
                TruckCapacityUnit capacity2 = obj as TruckCapacityUnit;
                return Mass == capacity2.Mass && Volume == capacity2.Volume;
            }

            return base.Equals(obj);
        }

        public static TruckCapacityUnit operator -(TruckCapacityUnit capacity1, TruckCapacityUnit capacity2)
        {
            return new TruckCapacityUnit
            {
                Mass = capacity1.Mass - capacity2.Mass,
                Volume = capacity1.Volume - capacity2.Volume
            };
        }

        public static bool operator ==(TruckCapacityUnit capacity1, TruckCapacityUnit capacity2)
        {
            if (object.ReferenceEquals(capacity1, null))
            {
                return object.ReferenceEquals(capacity2, null);
            }
            
            return capacity1.Equals(capacity2);
        }

        public static bool operator !=(TruckCapacityUnit capacity1, TruckCapacityUnit capacity2)
        {
            if (object.ReferenceEquals(capacity1, null))
            {
                return !object.ReferenceEquals(capacity2, null);
            }

            return !capacity1.Equals(capacity2);
        }


        public bool FitsIn(TruckCapacityUnit max)
        {
            return (Mass <= max.Mass) && (Volume <= max.Volume);
        }
    }

    

    public class Truck
    {
        // A truck has a One-to-one relationship with contractor
        [Key]
        public Guid TruckGuid { get; set; }


        [Required]
        [Display(Name = "License Plate Number")]
        public virtual String LicenseNumber { get; set; }


        // Gets broken down into different props but stays in the truck table
        public DbGeography Location { get; set; }

        public TruckCapacityUnit GetAvailableCapacity()
        {
            if (UsedCapacity == null)
            {
                return MaxCapacity;
            }

            return new TruckCapacityUnit
            {
                TruckCapacityUnitGuid = Guid.NewGuid(),
                Mass = MaxCapacity.Mass - UsedCapacity.Mass,
                Volume = MaxCapacity.Volume - UsedCapacity.Volume
            };
        }

        [Display(Name = "Used Capacity")]
        public virtual TruckCapacityUnit UsedCapacity { get; set; }

        [Required]
        [Display(Name = "Maximum Capacity")]
        public virtual TruckCapacityUnit MaxCapacity { get; set; }

        public virtual ICollection<OrderTrackInfo> CurrentOrders { get; set; }
    }

}