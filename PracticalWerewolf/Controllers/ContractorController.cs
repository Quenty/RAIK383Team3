using Microsoft.AspNet.Identity;
using PracticalWerewolf.Models.UserInfos;
using PracticalWerewolf.ViewModels.Contractor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace PracticalWerewolf.Controllers
{
    [Authorize]
    public class ContractorController : Controller
    {
        public enum ContractorMessageId
        {
            RegisterSuccess,
            AlreadyRegisteredError,
            Error
        }

        private ApplicationUserManager UserManager {get; set;}

        public ContractorController(ApplicationUserManager UserManager)
        {
            this.UserManager = UserManager;
        }

        public async Task<ActionResult> Index(ContractorMessageId? message)
        {
            ViewBag.StatusMessage = 
                message == ContractorMessageId.RegisterSuccess ? "Registered as a contractor successfully"
                : message == ContractorMessageId.Error ? "Error occured"
                : message == ContractorMessageId.AlreadyRegisteredError ? "You are already registered as a contractor"
                : "";

            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());

            var model = new ContractorIndexModel
            {
                ContractorInfo = user.ContractorInfo,
            };

        
            return View(model);
        }


        public async Task<ActionResult> Register()
        {
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user.ContractorInfo != null)
            {
                return RedirectToAction("Index", new { Message = ContractorMessageId.AlreadyRegisteredError });
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(ContractorRegisterModel model)
        {
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user.ContractorInfo != null)
            {
                return RedirectToAction("Index", new { Message = ContractorMessageId.AlreadyRegisteredError });
            }

            user.ContractorInfo = new ContractorInfo()
            {
                ContractorInfoGuid = Guid.NewGuid(),
                Truck = null,
                IsApproved = false,
                IsAvailable = false
            };

            var result = await UserManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                return RedirectToAction("Index", new { Message = ContractorMessageId.RegisterSuccess });
            }
            else
            {
                return RedirectToAction("Index", new { Message = ContractorMessageId.Error });
            }
            
        }
    }
}