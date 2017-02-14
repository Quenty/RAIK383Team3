using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Device.Location;

namespace PracticalWerewolf.Models
{
    public interface ITruckCapacityUnit
    {
    }

    public interface ITruck
    {
        GeoCoordinate Location { get; set; }
        IContractorInfo Contractor { get; set; }
        ITruckCapacityUnit CurrentCapacity { get; set; }
        ITruckCapacityUnit MaxCapacity { get; set;}
        ITruckCapacityUnit AvailableCapacity { get; } // TODO implement
    }
}
