using PracticalWerewolf.Models.UserInfos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Spatial;
using System.Device.Location;
using System.Linq;
using System.Web;

namespace PracticalWerewolf.ViewModels.Contractor
{
    public class ContractorRegisterModel
    {
        [Required]
        [Display(Name = "Street address")]
        public String StreetAddress { get; set; }

        [Required]
        public String City { get; set; }

        [Required]
        public String State { get; set; }

        [Required]
        public String ZipCode { get; set; }

        [Required]
        public String Country { get; set; }

        [Required]
        [Display(Name = "Drivers license ID")]
        public String DriversLicenseId { get; set; }
    }

    public class ContractorApprovalModel
    {
        public Guid ApprovedContractor { get; set; }

        [Required]
        public Boolean IsApproved { get; set; }
    }

    public class PendingContractorsModel
    {
        public IEnumerable<ContractorInfo> Pending { get; set; }
    }

    public class ContractorIndexModel
    {
        public ContractorInfo ContractorInfo { get; set; }
    }
}