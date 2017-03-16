using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

// http://jasonwatmore.com/post/2013/10/16/aspnet-mvc-required-checkbox-with-data-annotations

namespace PracticalWerewolf.Controllers.Validation
{
    public class MustBeTrueAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            return value is bool && (bool)value;
        }
    }
}