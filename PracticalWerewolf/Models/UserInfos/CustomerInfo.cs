using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PracticalWerewolf.Models.UserInfos
{
    public class CustomerInfo
    {
        [Key]
        public Guid CustomerInfoGuid { get; set; }
                                                            // Note: a customer can be tied to multiple orderRequestInfo (One-to-many relationship)
    }
}