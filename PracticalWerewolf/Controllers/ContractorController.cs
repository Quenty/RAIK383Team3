using Microsoft.AspNet.Identity;
using PracticalWerewolf.Models.UserInfos;
using PracticalWerewolf.Services.Interfaces;
using PracticalWerewolf.Stores.Interfaces;
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

        private ApplicationUserManager UserManager { get; set; }
        private IContractorService ContractorService { get; set; }

        public ContractorController(ApplicationUserManager UserManager, IContractorService ContractorService)
        {
            this.UserManager = UserManager;
            this.ContractorService = ContractorService;
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

        [Authorize(Roles = "Employee")]
        public ActionResult Approve()
        {
            PendingContractorsModel model = new PendingContractorsModel()
            {
                Pending = ContractorService.GetUnapprovedContractors(),
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Approve(ContractorApprovalModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            return RedirectToAction("Approve", new { Message = ContractorMessageId.Error });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(ContractorRegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user.ContractorInfo != null)
            {
                return RedirectToAction("Index", new { Message = ContractorMessageId.AlreadyRegisteredError });
            }

            user.ContractorInfo = new ContractorInfo()
            {
                ContractorInfoGuid = Guid.NewGuid(),
                Truck = null,
                ApprovalState = ContractorApprovalState.Pending,
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