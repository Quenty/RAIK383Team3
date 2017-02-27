using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PracticalWerewolf.Models.UserInfos
{
    public class EmployeeInfo
    {
        // This table will hold employee specific information
        [Key]
        public Guid EmployeeInfoGuid { get; set; }
    }

}