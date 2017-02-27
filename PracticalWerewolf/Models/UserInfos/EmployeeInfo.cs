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
        public Guid EmployeeInfoGuid { get; set; }              // Primary key for EmployeeInfo table, One-to-One
    }

}