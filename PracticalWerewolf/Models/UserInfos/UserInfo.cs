using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PracticalWerewolf.Models.UserInfos
{
    public class UserInfo
    {
        [Key]
        public Guid UserInfoGuid { get; set; }

        [MaxLength(50, ErrorMessage = "FirstName must be 50 characters or less")]
        public string FirstName { get; set; }

        [MaxLength(50, ErrorMessage = "LastName must be 50 characters or less")]
        public string LastName { get; set; }
    }
}