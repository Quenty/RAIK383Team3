using Microsoft.AspNet.Identity;
using PracticalWerewolf.Controllers.UnitOfWork;
using PracticalWerewolf.Models.UserInfos;
using PracticalWerewolf.Services.Interfaces;
using PracticalWerewolf.ViewModels.Contractor;
using System;
using System.Device.Location;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace PracticalWerewolf.Controllers
{
    [RequireHttps]
    [Authorize]
    public class ContractorController : Controller
    {
        public enum ContractorMessageId
        {
            ApprovedSuccess,
            DeniedSuccess,
            RegisterSuccess,
            AlreadyRegisteredError,
            Error
        }

        private ApplicationUserManager UserManager { get; set; }
        private IContractorService ContractorService { get; set; }
        private IOrderService OrderService { get; set; }
        private IUnitOfWork UnitOfWork { get; set; }

        public ContractorController(ApplicationUserManager UserManager, IOrderService OrderService, IContractorService ContractorService, IUnitOfWork UnitOfWork)
        {
            this.UnitOfWork = UnitOfWork;
            this.UserManager = UserManager;
            this.ContractorService = ContractorService;
            this.OrderService = OrderService;
        }

        private void GenerateErrorMessage(ContractorMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ContractorMessageId.RegisterSuccess ? "Registered as a contractor successfully"
                : message == ContractorMessageId.Error ? "Error occured"
                : message == ContractorMessageId.AlreadyRegisteredError ? "You are already registered as a contractor"
                : message == ContractorMessageId.ApprovedSuccess ? "Contractor approved"
                : message == ContractorMessageId.DeniedSuccess ? "Contractor denied"
                : "";
        }

        [AllowAnonymous]
        public async Task<ActionResult> Index(ContractorMessageId? message)
        {
            GenerateErrorMessage(message);

            var userId = User.Identity.GetUserId();
            if (userId != null)
            {
                var user = await UserManager.FindByIdAsync(userId);

                var model = new ContractorIndexModel
                {
                    ContractorInfo = user.ContractorInfo,
                };

                return View(model);
            }
            else
            {
                return View(new ContractorIndexModel());
            }
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
        public ActionResult Unapproved(ContractorMessageId? message)
        {
            GenerateErrorMessage(message);

            PendingContractorsModel model = new PendingContractorsModel()
            {
                Pending = ContractorService.GetUnapprovedContractors().Select(m => new ContractorApprovalModel
                {
                    ContractorInfo = m,
                }).ToList(),
            };

            return View(model);
        }

        [Authorize(Roles="Employee")]
        public ActionResult Approve(Guid guid, bool IsApproved)
        {
            if (guid.Equals(Guid.Empty))
            {
                return RedirectToAction("Unapproved", new { Message = ContractorMessageId.Error });
            }
                //Is this another instance where we want an IdentityResult?
                ContractorService.SetApproval(guid, IsApproved ? ContractorApprovalState.Approved : ContractorApprovalState.Denied);
            UnitOfWork.SaveChanges();

            return RedirectToAction("Unapproved", new { Message = IsApproved ? ContractorMessageId.ApprovedSuccess : ContractorMessageId.DeniedSuccess });
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


            CivicAddressDb Address = model.Address;
            Address.CivicAddressGuid = Guid.NewGuid();

            user.ContractorInfo = new ContractorInfo()
            {
                ContractorInfoGuid = Guid.NewGuid(),
                Truck = null,
                ApprovalState = ContractorApprovalState.Pending,
                IsAvailable = false,
                HomeAddress = Address,
                DriversLicenseId = model.DriversLicenseId
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

        public async Task<ActionResult> Pending()
        {
            var userId = User.Identity.GetUserId();
            if (userId != null)
            {
                var user = await UserManager.FindByIdAsync(userId);

                var contractor = user.ContractorInfo;
                var model = new PendingOrderViewModel()
                {
                    Orders = OrderService.GetQueuedOrders(contractor)
                };

                return PartialView(model);
            }
            else
            {
                return View();
            }
        }

        public async Task<ActionResult> Current()
        {
            var userId = User.Identity.GetUserId();
            if (userId != null)
            {
                var user = await UserManager.FindByIdAsync(userId);

                var contractor = user.ContractorInfo;
                var model = new CurrentOrderViewModel()
                {
                    Orders = OrderService.GetInprogressOrders(contractor)
                };

                return PartialView(model);
            }
            else
            {
                return View();
            }
        }



    }
}