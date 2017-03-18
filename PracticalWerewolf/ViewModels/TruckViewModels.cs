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
        public IEnumerable<ContractorInfo> Contractor { get; set; }
        public IEnumerable<Truck> Trucks { get; set; }
    }

    public class TruckDetailsViewModel
    {
        public String Guid { get; set; }
        [Display(Name = "License Plate Number")]
        public String LicenseNumber { get; set; }
        [Display(Name = "Latitude")]
        public double? Lat { get; set; }
        [Display(Name = "Longitude")]
        public double? Long { get; set; }
        public String State { get; set; }
        [Display(Name = "Maximum Capacity: ")]
        public TruckCapacityUnit MaxCapacity { get; set; }
        [Display(Name = "Available Capacity: ")]
        public TruckCapacityUnit AvailableCapacity { get; set; }
    }

    public class TruckUpdateViewModel
    {
        public String Guid { get; set; }
        [Display(Name = "License Plate Number")]
        public String LicenseNumber { get; set; }
        [Display(Name = "Weight (lb)")]
        public double Mass { get; set; }
        [Display(Name = "Volume (cubic ft)")]
        public double Volume { get; set; }
    }
    public class TruckCreateViewModel
    {
        public String Guid { get; set; }
        [Display(Name = "License Plate Number")]
        public String LicenseNumber { get; set; }
        [Display(Name = "Weight (lb)")]
        public double Mass { get; set; }
        [Display(Name = "Volume (cubic ft)")]
        public double Volume { get; set; }
        [Display(Name = "Longitude")]
        public double Long { get; set; }
        [Display(Name = "Latitude")]
        public double Lat { get; set; }
    }



}