using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PracticalWerewolf.Models.UserInfos
{
    public class EmployeeInfo
    {
        [Key]
        public Guid CustomerInfoGuid { get; set; }
    }

}