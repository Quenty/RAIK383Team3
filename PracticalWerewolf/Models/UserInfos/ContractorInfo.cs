using PracticalWerewolf.Models.Trucks;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PracticalWerewolf.Models.UserInfos
{
    public class ContractorInfo
    {
        [Key]
        public Guid ContractorInfoGuid { get; set; }
        // One-to-one relationship, a contractor can only have 1 truck
        public virtual Truck Truck { get; set; }

        [Required]
        public bool IsAvailable { get; set; }
    }
}