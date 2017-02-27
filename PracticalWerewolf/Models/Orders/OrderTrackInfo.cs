using PracticalWerewolf.Models.Trucks;
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
        // One-to-one relationship
        Truck CurrentTruck { get; set; }
        // Many-to-one relationship, A truck driver can only have many orders at a time
        ContractorInfo Assignee { get; set; }               
    }
}
