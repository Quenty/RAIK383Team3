﻿using PracticalWerewolf.Models.Trucks;
using PracticalWerewolf.Models.UserInfos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PracticalWerewolf.Models.Orders
{
    public class OrderTrackInfo
    {
        [Key]
        public Guid OrderTrackInfoGuid { get; set; } 

        Truck CurrentTruck { get; set; }                    // One-to-one relationship
        ContractorInfo Assignee { get; set; }               // One-to-one relationship, A truck driver can only have one order at a time
    }
}