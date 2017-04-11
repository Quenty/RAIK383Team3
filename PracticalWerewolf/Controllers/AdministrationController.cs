using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace PracticalWerewolf.Controllers
{
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
                // TODO: error
            }

            await UserManager.SetLockoutEnabledAsync(UserId, true);
            await UserManager.SetLockoutEndDateAsync(UserId, DateTime.Today.AddYears(10));

            return Redirect(Request.UrlReferrer.ToString());
        }

        
        public ActionResult SetEmployee(string UserId)
        {
            var User = UserManager.FindByIdAsync(UserId).Result;

            if (User.EmployeeInfo != null)
            {
                // TODO: Error
            }

            User.EmployeeInfo = new Models.UserInfos.EmployeeInfo
            {
                EmployeeInfoGuid = Guid.NewGuid()
            };

            var result = UserManager.UpdateAsync(User).Result;

            // TODO: Check result

            return Redirect(Request.UrlReferrer.ToString());
        }
    }
}