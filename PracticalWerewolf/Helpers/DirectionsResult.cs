using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PracticalWerewolf.Helpers
{
    public class DirectionsResult
    {
        public string Origin { get; set; }

        public string Destination { get; set; }

        public long Distance { get; set; }

        public TimeSpan Duration { get; set; }
    }
}