using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Device.Location;

namespace System.Device.Location
{
   
    public class CivicAddressDb
    {
        [Required]
        public String Route { get; set; }

        [Required]
        [Display(Name = "Street number")]
        public String StreetNumber { get; set; }

        [Required]
        public String City { get; set; }

        [Required]
        public String State { get; set; }

        [Required]
        [Display(Name = "Zipcode")]
        [MaxLength(14, ErrorMessage = "Zipcode must be less than 14 characters")]
        public String ZipCode { get; set; }

        [Required]
        public String Country { get; set; }

        [Key]
        public Guid CivicAddressGuid { get; set; }

        [Display(Prompt = "Street address", Name = "Street Address")]
        public String RawInputAddress { get; set; }
    }
}