using PracticalWerewolf.Models.Trucks;
using PracticalWerewolf.Models.UserInfos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PracticalWerewolf.Models.Orders
{
    public enum OrderStatus { Complete, Cancelled, InProgress, Queued }

    public class OrderTrackInfo
    {
        [Key]
        public Guid OrderTrackInfoGuid { get; set; }

        public OrderStatus OrderStatus { get; set; }

        public virtual Truck CurrentTruck { get; set; }
        public virtual ContractorInfo Assignee { get; set; }
    }
}