using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PracticalWerewolf.Models.Home
{
    public class DonutChart<T>
    {
        public List<T> Data { get; set; }
        public List<string> Labels { get; set; }


        private string _MiddleLabel = null;
        public string MiddleLabel { get
            {
                return _MiddleLabel ?? ((Data is List<int>) ? (Data as List<int>).Sum().ToString() : "");
            }
            set
            {
                _MiddleLabel = value;
            }
        }
    }

    public class Statistics
    {
        public DonutChart<int> PackageStateChart { get; set; }
        public DonutChart<int> UsersChart { get; set; }
    }
}