using PracticalWerewolf.Controllers.Validation;
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
        [Display(Name = "Drivers license", Prompt = "Drivers license")]
        [MaxLength(14, ErrorMessage = "DriversLicenseId must be less than 14 characters")]
        public String DriversLicenseId { get; set; }

        public String RawAddressString { get; set; }

        [Required]
        [Display(Name = "Street address")]
        public CivicAddressDb Address { get; set; }

        [Required]
        [Display(Name = "I have read and agree to the contractor terms and conditions")]
        [MustBeTrue(ErrorMessage = "You must agree to the terms and conditions to register")]
        public Boolean TermsOfService { get; set; }
    }

    public class ContractorApprovalModel
    {
        [Required]
        public ContractorInfo ContractorInfo { get; set; }

        [Required]
        public ContractorApprovalState NewState { get; set; }
    }

    public class PendingContractorsModel
    {
        public IEnumerable<ContractorApprovalModel> Pending { get; set; }
    }

    public class ContractorIndexModel
    {
        public ContractorInfo ContractorInfo { get; set; }
    }
}