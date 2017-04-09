using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PracticalWerewolf.EmailTemplates
{
    public enum OrderUpdateType
    {
        [Display(Description ="has Shipped")]
        Shipping
    }

    public class OrderUpdateModel
    {
        public string UserName { get; set; }
        public OrderUpdateType UpdateType { get; set; }
        public string UpdateDescription { get; set; }
        public Guid OrderId { get; set; }
        public string ArrivalDate { get; set; }
        public string Destination { get; set; }
        public string Cost { get; set; }
        public string LogoId { get; set; }
    }
}