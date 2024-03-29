﻿using PracticalWerewolf.Models.Orders;
using PracticalWerewolf.Models.Trucks;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Device.Location;
using System.Linq;
using System.Web;

namespace PracticalWerewolf.Models.UserInfos
{
    public enum ContractorApprovalState
    {
        Pending,
        Approved,
        Denied
    }

    public class ContractorInfo
    {
        [Key]
        public Guid ContractorInfoGuid { get; set; }

        [Required]
        [MaxLength(20, ErrorMessage = "License must be shorter than 20 characters")]
        [Display(Name = "Drivers license ID")]
        public String DriversLicenseId { get; set; }

        [Required]
        [Display(Name = "Home address")]
        public virtual CivicAddressDb HomeAddress { get; set; }

        // One-to-one relationship, a contractor can only have 1 truck
        public virtual Truck Truck { get; set; }

        [Required]
        public ContractorApprovalState ApprovalState { get; set; } = ContractorApprovalState.Pending;

        [Required]
        public bool IsAvailable { get; set; }

        public virtual ICollection<OrderTrackInfo> AssignedOrders { get; set; }
    }
}