using Microsoft.AspNet.Identity;
using PracticalWerewolf.Controllers.UnitOfWork;
using PracticalWerewolf.Models.UserInfos;
using PracticalWerewolf.Services.Interfaces;
using PracticalWerewolf.Stores.Interfaces;
using PracticalWerewolf.ViewModels.Contractor;
using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Data.Entity.Validation;
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
            Error,
            StatusChangeSuccess
        }

        private ApplicationUserManager UserManager { get; set; }
        private IContractorService ContractorService { get; set; }
        private IUnitOfWork UnitOfWork { get; set; }

        public ContractorController(ApplicationUserManager UserManager, IContractorService ContractorService, IUnitOfWork UnitOfWork)
        {
            this.UnitOfWork = UnitOfWork;
            this.UserManager = UserManager;
            this.ContractorService = ContractorService;
        }

        private void GenerateErrorMessage(ContractorMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ContractorMessageId.RegisterSuccess ? "Registered as a contractor successfully"
                : message == ContractorMessageId.Error ? "Error occured"
                : message == ContractorMessageId.AlreadyRegisteredError ? "You are already registered as a contractor"
                : message == ContractorMessageId.ApprovedSuccess ? "Contractor approved"
                : message == ContractorMessageId.DeniedSuccess ? "Contractor denied"
                : message == ContractorMessageId.StatusChangeSuccess ? "Status successfully changed"
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



        //[HttpPost]
        //[AllowAnonymous]
        //public async Task<ActionResult> ChangeStatus(ContractorMessageId? message)
        //{
        //    GenerateErrorMessage(message);

        //    var userId = User.Identity.GetUserId();
        //    if (userId != null)
        //    {
        //        var user = await UserManager.FindByIdAsync(userId);

        //        var model = new ContractorIndexModel
        //        {
        //            ContractorInfo = user.ContractorInfo,
        //        };

        //        return View(model);
        //    }
        //    else
        //    {
        //        return View(new ContractorIndexModel());
        //    }
        //}
        //[HttpPost]
        //public async Task<ActionResult> ChangeStatus(ContractorMessageId? message)
        //{
        //    GenerateErrorMessage(message);

        //    var userId = User.Identity.GetUserId();
        //    if (userId != null)
        //    {
        //        var user = await UserManager.FindByIdAsync(userId);

        //        var model = new ContractorIndexModel
        //        {
        //            ContractorInfo = user.ContractorInfo,
        //        };

        //        return View(model);
        //    }
        //    else
        //    {
        //        return View(new ContractorIndexModel());
        //    }
        //}

        public ActionResult UpdateStatus(string id)
        {
            if (!String.IsNullOrEmpty(id))
            {
                try
                {

                    var guid = new Guid(id);
                    var contractor = ContractorService.GetContractor(guid);
                    var model = new ContractorStatusModel
                    {
                        ContractorGuid = guid,
                        ContractorStatus = contractor.IsAvailable
                    };

                    return View(model);
                }
                catch
                {
                    return RedirectToAction("Index", "Contractor", new { Message = "Could not update status successfully." });
                }
            }
            return HttpNotFound();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateStatus(ContractorStatusModel returnedModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    ContractorService.SetIsAvailable(returnedModel.ContractorGuid, !returnedModel.ContractorStatus);
                    UnitOfWork.SaveChanges();
                    return RedirectToAction("Index", "Contractor", new { Message = ContractorMessageId.StatusChangeSuccess });
                }
                catch
                {
                    return RedirectToAction("Index", "Contractor", new { Message = "Could not update status successfully." });
                }
            }
            return RedirectToAction("Index", "Contractor", new { Message = "Could not update truck location successfully." });
        }



        //[HttpPost]
        //public async Task<ActionResult> ChangeTheValue(ContractorIndexModel model)
        //{
        //    var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
        //    user.ContractorInfo.IsAvailable = !user.ContractorInfo.IsAvailable;
        //    UserManager.Update(user);

        //    var m = new ContractorIndexModel();
        //    //m.ContractorInfo = model.ContractorInfo;
        //    //m.ContractorInfo.IsAvailable = !model.ContractorInfo.IsAvailable;
        //    return Json(m.ContractorInfo.IsAvailable);
        //}


        //[HttpPost]
        //public ActionResult StatusUpdate(ContractorInfo model)
        //{
        //    ContractorService.SetIsAvailable(model.ContractorInfoGuid, !model.IsAvailable);
        //    UnitOfWork.SaveChanges();

        //    return RedirectToAction("Index", new { Message = ContractorMessageId.AlreadyRegisteredError});

        //}

        //[HttpPost]
        //public async Task<ActionResult> Changestatus(ContractorIndexModel model)
        //{
        //    var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
        //    var userStatus = user.ContractorInfo.IsAvailable;
        //    if (user.ContractorInfo == null)
        //    {
        //        return RedirectToAction("Error", new { Message = ContractorMessageId.Error });
        //    }
        //    model.ContractorInfo.IsAvailable = !model.ContractorInfo.IsAvailable;
        //    try
        //    {
        //        // Your code...
        //        // Could also be before try if you know the exception occurs in SaveChanges
        //        var result = await UserManager.UpdateAsync(user);
        //        if (result.Succeeded)
        //        {
        //            return View(model);
        //            //return RedirectToAction("Index", new { Message = ContractorMessageId.RegisterSuccess });
        //        }
        //        else
        //        {
        //            return RedirectToAction("Error", new { Message = ContractorMessageId.Error });
        //        }
        //    }
        //    catch (DbEntityValidationException e)
        //    {
        //        foreach (var eve in e.EntityValidationErrors)
        //        {
        //            System.Diagnostics.Debug.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
        //                eve.Entry.Entity.GetType().Name, eve.Entry.State);
        //            foreach (var ve in eve.ValidationErrors)
        //            {
        //                System.Diagnostics.Debug.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
        //                    ve.PropertyName, ve.ErrorMessage);
        //            }
        //        }
        //        throw;
        //    }
        //}

        //public async Task<ActionResult> ChangeStatus(ContractorMessageId? message)
        //{
        //    GenerateErrorMessage(message);

        //    var userId = User.Identity.GetUserId();
        //    if (userId != null)
        //    {
        //        var user = await UserManager.FindByIdAsync(userId);

        //        var model = new ContractorIndexModel
        //        {
        //            ContractorInfo = user.ContractorInfo,
        //        };

        //        return View(model);
        //    }
        //    else
        //    {
        //        return View(new ContractorIndexModel());
        //    }
        //}
    }
}
