using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Web;

namespace PracticalWerewolf.Helpers
{
    public class DirectionsResult
    {

        public CivicAddressDb Origin { get; set; }

        public CivicAddressDb Destination { get; set; }

        public int Distance { get; set; }

        public TimeSpan Duration { get; set; }
    }
}