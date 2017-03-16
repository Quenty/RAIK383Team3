using PracticalWerewolf.Models.Trucks;
using PracticalWerewolf.Models.UserInfos;
using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using System.Linq;
using System.Web;

namespace PracticalWerewolf.ViewModels
{
   public class TruckIndexViewModel
    {
        public IEnumerable<Truck> Trucks { get; set; }
    }

    public class TruckDetailsViewModel
    {
        public String Guid { get; set; }
        public String LicenseNumber { get; set; }
        public double? Lat { get; set; }
        public double? Long { get; set; }
        public String State { get; set; }
        public TruckCapacityUnit MaxCapacity { get; set; }
        public TruckCapacityUnit AvailableCapacity { get; set; }
    }

    public class TruckUpdateViewModel
    {
        public String Guid { get; set; }
        public String LicenseNumber { get; set; }
        public double Mass { get; set; }
        public double Volume { get; set; }
    }



}