using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Web;

namespace PracticalWerewolf.Models
{
    public class Truck : ITruck
    {
        public ITruckCapacityUnit AvailableCapacity { get; set; }
        public ContractorInfo Contractor { get; set; }
        public ITruckCapacityUnit CurrentCapacity { get; set; }
        public GeoCoordinate Location { get; set; }
        public ITruckCapacityUnit MaxCapacity { get; set; }
    }
}