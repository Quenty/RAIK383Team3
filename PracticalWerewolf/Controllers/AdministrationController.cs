﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace PracticalWerewolf.Controllers
{
    [RequireHttps]
    [Authorize(Roles = "Employee")]
    public class AdministrationController : Controller
    {
        private readonly ApplicationUserManager UserManager;

        public AdministrationController(ApplicationUserManager UserManager)
        {
            this.UserManager = UserManager;
        }

        public async Task<ActionResult> BanUser(string UserId)
        {
            if (UserId == null)
            {
                throw new ArgumentNullException();
            }

            await UserManager.SetLockoutEnabledAsync(UserId, true);
            await UserManager.SetLockoutEndDateAsync(UserId, DateTime.Today.AddYears(10));

            return Redirect(Request.UrlReferrer.ToString());
        }

        public async Task<ActionResult> UnbanUser(string UserId)
        {
            if (UserId == null)
            {
                throw new ArgumentNullException();
            }

            await UserManager.SetLockoutEnabledAsync(UserId, false);

            return Redirect(Request.UrlReferrer.ToString());
        }

        public ActionResult RemoveEmployee(string UserId)
        {
            if (UserId == null)
            {
                throw new ArgumentNullException();
            }

            var User = UserManager.FindByIdAsync(UserId).Result;
            var ForceLoad = User.EmployeeInfo;

            User.EmployeeInfo = null;

            var result = UserManager.UpdateAsync(User).Result;

            return Redirect(Request.UrlReferrer.ToString());
        }

        public ActionResult SetEmployee(string UserId)
        {
            if (UserId == null)
            {
                throw new ArgumentNullException();
            }

            var User = UserManager.FindByIdAsync(UserId).Result;

            if (User.EmployeeInfo != null)
            {
                throw new ArgumentException("User already is employee");
            }

            User.EmployeeInfo = new Models.UserInfos.EmployeeInfo
            {
                EmployeeInfoGuid = Guid.NewGuid()
            };

            var result = UserManager.UpdateAsync(User).Result;

            return Redirect(Request.UrlReferrer.ToString());
        }
    }
}