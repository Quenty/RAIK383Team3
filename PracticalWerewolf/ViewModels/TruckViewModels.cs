using PracticalWerewolf.Models.Trucks;
using System;
using System.Collections.Generic;
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
        public double Speed { get; set; }
        public String GeoCordinate { get; set; }
    }



}