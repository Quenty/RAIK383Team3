﻿using PracticalWerewolf.Models.Trucks;
using PracticalWerewolf.Models.UserInfos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PracticalWerewolf.Models.Orders
{
    public enum OrderStatus
    {
        Complete,
        Cancelled,
        [Display(Name = "In Progress")]
        InProgress,
        Queued
    }

    public class OrderTrackInfo
    {
        [Key]
        public Guid OrderTrackInfoGuid { get; set; }

        [Required]
        [Display(Name = "Status")]
        public OrderStatus OrderStatus { get; set; } = OrderStatus.Queued;

        // One-to-zero-or-one relationship
        public virtual Truck CurrentTruck { get; set; }

        // Many-to-one relationship, A truck driver can have many orders at a time
        public virtual ContractorInfo Assignee { get; set; }
    }
}
