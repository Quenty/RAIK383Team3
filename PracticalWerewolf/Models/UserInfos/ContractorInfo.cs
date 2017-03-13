using PracticalWerewolf.Models.Trucks;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Device.Location;
using System.Linq;
using System.Web;

namespace PracticalWerewolf.Models.UserInfos
{
    public class ContractorInfo
    {
        [Key]
        public Guid ContractorInfoGuid { get; set; }

        [MaxLength(20, ErrorMessage = "License must be shorter than 20 characters")]
        [Display(Name = "Drivers license ID")]
        public String DriversLicenseId;

        [Display(Name = "Home address")]
        public CivicAddressDb HomeAddress;

        // One-to-one relationship, a contractor can only have 1 truck
        public virtual Truck Truck { get; set; }

        [Required]
        public bool IsApproved { get; set; }

        [Required]
        public bool IsAvailable { get; set; }
    }
}