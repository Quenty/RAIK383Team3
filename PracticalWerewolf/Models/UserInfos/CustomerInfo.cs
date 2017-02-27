using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PracticalWerewolf.Models.UserInfos
{
    public class CustomerInfo
    {
        // Note: a customer can be tied to multiple orderRequestInfo (One-to-many relationship)
        // Customers do not own orders, instead orders know who owns them
        [Key]
        public Guid CustomerInfoGuid { get; set; }
                                                            
    }
}