using Microsoft.AspNet.Identity;
using PracticalWerewolf.Controllers.UnitOfWork;
using PracticalWerewolf.Models.UserInfos;
using PracticalWerewolf.Services;
using PracticalWerewolf.Services.Interfaces;
using PracticalWerewolf.ViewModels;
using PracticalWerewolf.ViewModels.Contractor;
using System;
using System.Device.Location;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Data.Entity.Validation;
using System.Web.Mvc;
using PracticalWerewolf.Models.Routes;
using PracticalWerewolf.Services;
using PracticalWerewolf.Models.Orders;
using Hangfire;
using log4net;
using System.Security.Claims;
using System.Threading;
using System.Diagnostics;
using System.Collections.Generic;

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
            StatusChangeSuccess,
            StatusError,
            NoTruckCreated,
            TruckCreationError,
            ConfirmationError
        }

        private static ILog logger = LogManager.GetLogger(typeof(ContractorController));
        private readonly ApplicationUserManager UserManager;
        private readonly IContractorService ContractorService;
        private readonly IUnitOfWork UnitOfWork;
        private readonly IOrderService OrderService;
        private readonly IRouteStopService RouteStopService;
        private readonly ITruckService TruckService;
        private readonly IRoutePlannerService RoutePlannerService;


        public ContractorController(ApplicationUserManager UserManager, IOrderService OrderService, IContractorService ContractorService, IUnitOfWork UnitOfWork, IRouteStopService RouteStopService, IRoutePlannerService RoutePlannerService, ITruckService TruckService)
        {
            this.UnitOfWork = UnitOfWork;
            this.UserManager = UserManager;
            this.ContractorService = ContractorService;
            this.OrderService = OrderService;
            this.RouteStopService = RouteStopService;
            this.TruckService = TruckService;
            this.RoutePlannerService = RoutePlannerService;
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
                : message == ContractorMessageId.NoTruckCreated ? "You must create a truck to access this page."
                : message == ContractorMessageId.StatusError ? "Could not update status successfully."
                : message == ContractorMessageId.ConfirmationError ? "Internal Error. Could not confirm pick up or drop off"
                : message == ContractorMessageId.TruckCreationError ? "Failed to create truck"
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

                if (User.IsInRole("Employee"))
                {
                    model.UnapprovedContractorCount = ContractorService.GetUnapprovedContractors().Count();
                }

                return View(model);

            }
            else
            {
                return View(new ContractorIndexModel());
            }

        }


        public ActionResult Register()
        {
            if (User.IsInRole("Contractor"))
            {
                return RedirectToAction("Index", new { Message = ContractorMessageId.AlreadyRegisteredError });
            }

            return View();
        }

        [Authorize(Roles = "Employee")]
        public ActionResult Unapproved(ContractorMessageId? message)
        {
            GenerateErrorMessage(message);

            var pending = new List<ContractorApprovalModel>();
            foreach(var x in ContractorService.GetUnapprovedContractors().ToList())
            {
                var innerModel = new ContractorApprovalModel
                {
                    ContractorInfo = x.ContractorInfo
                };


                if (x.Email != null)
                {
                    innerModel.EmailAddress = x.Email;
                }
                
                if (x.PhoneNumber != null)
                {
                    innerModel.PhoneNumber = x.PhoneNumber;
                }
                

                pending.Add(innerModel);
            }
            PendingContractorsModel model = new PendingContractorsModel()
            {
                Pending = pending
            };

            return View(model);
        }

        [Authorize(Roles = "Employee")]
        public ActionResult Approve(Guid guid, bool IsApproved)
        {
            if (guid.Equals(Guid.Empty))
            {
                return RedirectToAction("Unapproved", new { Message = ContractorMessageId.Error });
            }

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
                // http://stackoverflow.com/questions/35446038/asp-net-identity-change-user-role-while-logged-in

                //Get the authentication manager
                var authenticationManager = HttpContext.GetOwinContext().Authentication;

                //Log the user out
                authenticationManager.SignOut();

                //Log the user back in
                var identity = await user.GenerateUserIdentityAsync(UserManager);
                authenticationManager.SignIn(new Microsoft.Owin.Security.AuthenticationProperties() { IsPersistent = true }, identity);

                return RedirectToAction("Index", new { Message = ContractorMessageId.RegisterSuccess });
            }
            else
            {
                return RedirectToAction("Index", new { Message = ContractorMessageId.Error });
            }

        }

        [Authorize(Roles = "Contractor")]
        public async Task<ActionResult> Pending()
        {
            var userId = User.Identity.GetUserId();
            if (userId != null)
            {
                var user = await UserManager.FindByIdAsync(userId);

                var contractor = user.ContractorInfo;
                var model = new PagedOrderListViewModel()
                {
                    DisplayName = "Pending orders",
                    Orders = OrderService.GetInprogressOrdersNoTruck(contractor)
                };

                return PartialView("_PagedOrderListPane", model);
            }
            else
            {
                ViewBag.Message = "No user identity?";
                return PartialView("_StatusMessage");
            }
        }

        //Returns partial view with an order list of deliveries
        public async Task<ActionResult> Route()
        {
            var userId = User.Identity.GetUserId();
            if(userId != null)
            {
                var user = await UserManager.FindByIdAsync(userId);
                var contractor = user.ContractorInfo;

                var model = new OrderRouteViewModel()
                {
                    DistanceToNextStop = RouteStopService.GetDistanceToNextStopInMiles(contractor),
                    DisplayName = "Your Current Route",
                    Route = RouteStopService.GetContractorRoute(contractor).ToList()
                };
                return PartialView("_Route", model);
            }
            return View("index", new { Message = ContractorMessageId.Error });
        }


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
                        IsAvailable = contractor.IsAvailable
                    };

                    return PartialView("_UpdateStatus", model);
                }
                catch
                {
                    return RedirectToAction("Index", "Contractor", new { Message = ContractorMessageId.StatusError });
                }
            }
            return HttpNotFound();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateStatus(ContractorStatusModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    ContractorService.SetIsAvailable(model.ContractorGuid, !model.IsAvailable);
                    //after changing their status
                    if (!model.IsAvailable) { 
                        var pendingOrders = OrderService.GetInprogressOrdersNoTruck(model.ContractorGuid);
                        if (!pendingOrders.Any())
                        {
                            foreach (var order in pendingOrders)
                            {
                                RouteStopService.UnassignOrderFromRouteStop(order);
                                OrderService.UnassignOrder(order);
                            }
                        }
                    }
                    UnitOfWork.SaveChanges();
                    BackgroundJob.Enqueue(() => RoutePlannerService.AssignOrders());

                    return Redirect(Url.Action("Index", "Contractor", new { Message = ContractorMessageId.StatusChangeSuccess }) + "#status");
                }
                catch (Exception e)
                {
                    logger.Warn("Could not change contractor status", e);
                    return Redirect(Url.Action("Index", "Contractor", new { Message = ContractorMessageId.StatusError }) + "#status");
                }
            }
            return Redirect(Url.Action("Index", "Contractor", new { Message = ContractorMessageId.StatusError }) + "#status");
        }

        [Authorize(Roles = "Contractor")]
        public async Task<ActionResult> Current()
        {
            var userId = User.Identity.GetUserId();
            if (userId != null)
            {
                var user = await UserManager.FindByIdAsync(userId);

                var contractor = user.ContractorInfo;
                try { 
                var model = new PagedOrderListViewModel()
                {
                    DisplayName = "Current orders",
                    Orders = OrderService.GetInprogressOrdersInTruck(contractor)
                };
                
                return PartialView("_PagedOrderListPane", model);
                }
                catch (Exception e)
                {
                    //change this
                    return Redirect(Url.Action("Index", "Contractor", new { Message = ContractorMessageId.StatusError }) + "#status");
                }

            }
            else
            {
                return View();
            }
        }

        [Authorize(Roles = "Contractor")]
        public async Task<ActionResult> Delivered()
        {
            var userId = User.Identity.GetUserId();
            if (userId != null)
            {
                var user = await UserManager.FindByIdAsync(userId);

                var contractor = user.ContractorInfo;
                var model = new PagedOrderListViewModel()
                {
                    DisplayName = "Delivered orders",
                    Orders = OrderService.GetDeliveredOrders(contractor)
                };

                return PartialView("_PagedOrderListPane", model);
            }
            else
            {
                return View();
            }
        }

        public async Task<ActionResult> Status()
        {
            var userId = User.Identity.GetUserId();
            if (userId != null)
            {
                var user = await UserManager.FindByIdAsync(userId);

                var truck = user.ContractorInfo.Truck;
                if (truck != null)
                {
                    var model = new TruckDetailsViewModel
                    {
                        Guid = truck.TruckGuid,
                        LicenseNumber = truck.LicenseNumber,
                        MaxCapacity = truck.MaxCapacity,
                        AvailableCapacity = truck.GetAvailableCapacity(),
                        Lat = truck.Location.Latitude,
                        Long = truck.Location.Longitude
                    };
                    return PartialView("_Status", model);
                }
                else
                {
                    GenerateErrorMessage(ContractorMessageId.NoTruckCreated);
                    return PartialView("_StatusMessage");
                }
            }
            else
            {
                GenerateErrorMessage(ContractorMessageId.Error);
                return PartialView("_StatusMessage");
            }
        }

        // GET: Order/Confirmation/guid
        [Authorize(Roles = "Contractor")]
        public ActionResult Confirmation(string id)
        {
            if(id == null)
            {
                return Redirect(Url.Action("Index", "Contractor", new { Message = ContractorMessageId.StatusError }) + "#status");
            }
            var model = new ConfirmationViewModel
            {
                RouteStopGuid = new Guid(id)
            };
            return View(model);
        }


        // POST: Order/Confirmation/guid
        [Authorize(Roles = "Contractor")]
        [HttpPost]
        [ActionName("Confirmation")]
        public async Task<ActionResult> ConfirmationPost(ConfirmationViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                    var contractorInfo = user.ContractorInfo;

                    if (contractorInfo == null)
                    {
                        Debug.Fail("No contractor");
                        return RedirectToAction("Index", "Contractor", new { Message = ContractorMessageId.Error });
                    }

                    RouteStop stop = RouteStopService.GetRouteStop(model.RouteStopGuid);
                    Order order = stop.Order;
                    if (order == null)
                    {
                        Debug.Fail("Null order");
                        logger.Error("Null order");
                        return RedirectToAction("Index", "Contractor", new { Message = ContractorMessageId.Error });
                    }

                    Debug.Assert(order.TrackInfo != null);
                    Debug.Assert(order.TrackInfo.Assignee != null);

                    if (order.TrackInfo.Assignee.ContractorInfoGuid != contractorInfo.ContractorInfoGuid)
                    {
                        Debug.Fail("Assigned to wrong user");
                        logger.Warn("TrackInfo.Assignee !+ ContractorInfo, invalid attempt");
                        return RedirectToAction("Index", "Contractor", new { Message = ContractorMessageId.Error });
                    }

                    Debug.Assert(order.TrackInfo.Assignee.Truck != null);
                    Debug.Assert(order.TrackInfo.Assignee.Truck.TruckGuid != Guid.Empty);
                    
                    //RouteStopService.RemoveRouteStop(model.RouteStopGuid);
                    switch (stop.Type)
                    {
                        case StopType.DropOff:
                            await OrderService.SetOrderAsComplete(order.OrderGuid);
                            TruckService.RemoveItemFromTruck(order.TrackInfo.Assignee.Truck.TruckGuid, order);
                            UnitOfWork.SaveChanges();
                            break;
                        case StopType.PickUp:
                            ContractorInfo contractor = order.TrackInfo.Assignee;
                            OrderService.SetOrderInTruck(order.OrderGuid);
                            TruckService.AddItemToTruck(contractor.Truck.TruckGuid, order);
                            UnitOfWork.SaveChanges();
                            break;
                        default:
                            Debug.Fail("Unknown stop type!");
                            logger.Error("Unknown stop type");
                            return RedirectToAction("Index", "Contractor", new { Message = ContractorMessageId.Error });
                    }

                    return RedirectToAction("Index", "Contractor");
                }
                catch (Exception e)
                {
                    Debug.Fail("Failed to pick up");
                    logger.Error(e);
                    Console.Write(e);
                    return RedirectToAction("Index", "Contractor", new { Message = ContractorMessageId.ConfirmationError });
                }
            }
            else
            {
                return View(model);
            }
        }


    }


}
