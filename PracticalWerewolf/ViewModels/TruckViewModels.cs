using PracticalWerewolf.Models;
using PracticalWerewolf.Models.Trucks;
using PracticalWerewolf.Models.UserInfos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Spatial;
using System.Linq;
using System.Web;

namespace PracticalWerewolf.ViewModels
{
   public class TruckIndexViewModel
    {
        public IEnumerable<TruckDetailsViewModel> Trucks { get; set; }
    }

    public class TruckDetailsViewModel
    {
        public Guid Guid { get; set; }
        [Display(Name = "License Plate Number")]
        public String LicenseNumber { get; set; }
        [Display(Name = "Latitude")]
        public double? Lat { get; set; }
        [Display(Name = "Longitude")]
        public double? Long { get; set; }

        [Display(Name = "Maximum Capacity: ")]
        public TruckCapacityUnit MaxCapacity { get; set; }
        [Display(Name = "Available Capacity: ")]
        public TruckCapacityUnit AvailableCapacity { get; set; }

        public ApplicationUser Owner { get; set; } // TODO: Refactor to UserInfo, store instead of loading ApplicationUser
    }

    public class TruckUpdateViewModel
    {
        public Guid Guid { get; set; }
        [Display(Name = "License Plate Number")]
        public String LicenseNumber { get; set; }
        [Display(Name = "Weight (lb)")]
        [Range(0d, (double)decimal.MaxValue, ErrorMessage = "Amount must be greater than zero.")]
        public double Mass { get; set; }
        [Range(0d, (double)decimal.MaxValue, ErrorMessage = "Amount must be greater than zero.")]
        [Display(Name = "Volume (cubic ft)")]
        public double Volume { get; set; }
    }
    public class TruckCreateViewModel
    {
        public Guid Guid { get; set; }
        [Display(Name = "License Plate Number")]
        public String LicenseNumber { get; set; }
        [Display(Name = "Weight (lb)")]
        [Range(0d, (double)decimal.MaxValue, ErrorMessage = "Amount must be greater than zero.")]
        public double Mass { get; set; }
        [Display(Name = "Volume (cubic ft)")]
        [Range(0d, (double)decimal.MaxValue, ErrorMessage = "Amount must be greater than zero.")]
        public double Volume { get; set; }
        [Display(Name = "Longitude")]
        [Range(-180d, 180d, ErrorMessage = "Longitude must be between -180 and 180.")]
        public double Long { get; set; }
        [Display(Name = "Latitude")]
        [Range(-90d, 90d, ErrorMessage = "Latitude must be between -90 and 90.")]
        public double Lat { get; set; }
        public ApplicationUser Owner { get; set; } // TODO: Refactor to UserInfo, store instead of loading ApplicationUser
    }

    public class TruckUpdateLocation
    {
        public Guid Guid { get; set; }
        [Display(Name = "Longitude")]
        [Range(-180d, 180d, ErrorMessage = "Longitude must be between -180 and 180.")]
        public double? Long { get; set; }
        [Display(Name = "Latitude")]
        [Range(-90d, 90d, ErrorMessage = "Latitude must be between -90 and 90.")]
        public double? Lat { get; set; }
    }



}