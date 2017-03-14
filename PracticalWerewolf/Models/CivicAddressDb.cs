using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Device.Location;

namespace System.Device.Location
{
    public class CivicAddressDb : CivicAddress
    {
        [Key]
        public Guid CivicAddressGuid { get; set; }
    }
}