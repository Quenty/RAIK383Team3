using PracticalWerewolf.Services.Interfaces;
using System;
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
        private readonly IContractorService ContractorService;

        public AdministrationController(ApplicationUserManager UserManager, IContractorService ContractorService)
        {
            this.UserManager = UserManager;
            this.ContractorService = ContractorService;
        }

        public async Task<ActionResult> BanUser(string UserId)
        {
            if (UserId == null)
            {
                throw new ArgumentNullException();
            }

            var lockoutRequest = await UserManager.SetLockoutEnabledAsync(UserId, true);
            var enddateRequest = await UserManager.SetLockoutEndDateAsync(UserId, DateTime.Today.AddYears(10));

           
            var ContractorInfo = ContractorService.GetContractorInfo(UserId);
            if (ContractorInfo != null)
            {
                ContractorService.SetIsAvailable(ContractorInfo.ContractorInfoGuid, false);
            }

            if (lockoutRequest.Succeeded && enddateRequest.Succeeded)
            {
                await UserManager.UpdateSecurityStampAsync(UserId);
            }

            return Redirect(Request.UrlReferrer.ToString());
        }

        public async Task<ActionResult> UnbanUser(string UserId)
        {
            if (UserId == null)
            {
                throw new ArgumentNullException();
            }

            var lockoutRequest = await UserManager.SetLockoutEnabledAsync(UserId, false);
            if (lockoutRequest.Succeeded)
            {
                await UserManager.UpdateSecurityStampAsync(UserId);
            }
            
            
            return Redirect(Request.UrlReferrer.ToString());
        }

        public async Task<ActionResult> RemoveEmployee(string UserId)
        {
            if (UserId == null)
            {
                throw new ArgumentNullException();
            }

            var user = UserManager.FindByIdAsync(UserId).Result;
            var ForceLoad = user.EmployeeInfo;

            user.EmployeeInfo = null;

            var result = UserManager.UpdateAsync(user).Result;

            if (result.Succeeded)
            {
                await UserManager.UpdateSecurityStampAsync(user.Id);
            }

            return Redirect(Request.UrlReferrer.ToString());
        }

        public async Task<ActionResult> SetEmployee(string UserId)
        {
            if (UserId == null)
            {
                throw new ArgumentNullException();
            }

            var user = UserManager.FindByIdAsync(UserId).Result;

            if (user.EmployeeInfo != null)
            {
                throw new ArgumentException("User already is employee");
            }

            user.EmployeeInfo = new Models.UserInfos.EmployeeInfo
            {
                EmployeeInfoGuid = Guid.NewGuid()
            };

            var result = UserManager.UpdateAsync(user).Result;

            if (result.Succeeded)
            {
                await UserManager.UpdateSecurityStampAsync(user.Id);
            }

            return Redirect(Request.UrlReferrer.ToString());
        }
    }
}