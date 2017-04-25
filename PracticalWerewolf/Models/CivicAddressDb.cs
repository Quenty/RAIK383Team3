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
        public CivicAddressDb()
        {

        }

        public CivicAddressDb(CivicAddressDb copy)
        {
            this.Route = copy.Route;
            this.StreetNumber = copy.StreetNumber;
            this.City = copy.City;
            this.State = copy.State;
            this.ZipCode = copy.ZipCode;
            this.Country = copy.Country;
            this.RawInputAddress = copy.RawInputAddress;
        }

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

        public override string ToString()
        {
            string cityState = string.Join(", ", City, State);
            string zipCountry = string.Join(", ", ZipCode, Country);
            string value = string.Join(" ", StreetNumber, Route, cityState, zipCountry);

            return value;
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is CivicAddressDb)
            {
                return (obj as CivicAddressDb) == this;
            }
            return base.Equals(obj);
        }

        public static bool operator ==(CivicAddressDb address1, CivicAddressDb address2)
        {
            return address1.GetHashCode() == address2.GetHashCode();
        }

        public static bool operator !=(CivicAddressDb address1, CivicAddressDb address2)
        {
            return !(address1 == address2);
        }
    }
}