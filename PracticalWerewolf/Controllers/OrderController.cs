using Microsoft.AspNet.Identity;
using PracticalWerewolf.Controllers.UnitOfWork;
using PracticalWerewolf.Models.Orders;
using PracticalWerewolf.Models.UserInfos;
using PracticalWerewolf.Services.Interfaces;
using PracticalWerewolf.ViewModels.Orders;
using System;
using System.Web.Mvc;

namespace PracticalWerewolf.Controllers
{
    [RequireHttps]
    public class OrderController : Controller
    {
        private readonly IOrderRequestService OrderRequestService;
        private readonly IOrderTrackService OrderTrackService;
        private readonly IOrderService OrderService;
        private readonly IUserInfoService UserInfoService;
        private readonly IUnitOfWork UnitOfWork;
        private readonly ApplicationUserManager UserManager;

        public enum OrderMessageId
        {
            OrderCreatedSuccess,
            OrderCreatedError,
            CancelOrderError,
            CouldNotUpdateStatus,
            Error
        }

        public OrderController(IOrderRequestService OrderRequestService, IOrderTrackService OrderTrackService,
            IUserInfoService UserInfoService, IUnitOfWork UnitOfWork, ApplicationUserManager UserManager, IOrderService OrderService)
        {
            this.OrderRequestService = OrderRequestService;
            this.OrderTrackService = OrderTrackService;
            this.UserInfoService = UserInfoService;
            this.UnitOfWork = UnitOfWork;
            this.UserManager = UserManager;
            this.OrderService = OrderService;
        }

        public ActionResult Index(OrderMessageId? message)
        {
            ViewBag.StatusMessage = message == OrderMessageId.OrderCreatedSuccess ? "Order placed successfully."
                : message == OrderMessageId.OrderCreatedError ? "Internal error. Try placing your order again."
                : message == OrderMessageId.Error ? "Something went wrong, please try again!"
                : message == OrderMessageId.CancelOrderError ? "Internal error. Please try to cancel order again."
                : message == OrderMessageId.CouldNotUpdateStatus ? "Internal error. Could not update order status, try again."
                : "";

            return View();
        }


        // GET: Order/Details/guid
        [Authorize(Roles = "Employees, Contractors, Customers")]
        public ActionResult Details(string guid)
        {
            // Will get detailed information on a specific order
            // Depends upon IOrderService.GetByUserGuids
            return View();
        }

        // GET: Order/Create
        [Authorize(Roles = ("Customer"))]
        public ActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = ("Customer"))]
        public ActionResult Create(CreateOrderRequestViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            CustomerInfo Requester = UserInfoService.GetUserCustomerInfo(User.Identity.GetUserId());
            if (Requester == null)
            {
                return RedirectToAction("Index", new { message = OrderMessageId.OrderCreatedError });
            }

            model.DropOffAddress.CivicAddressGuid = Guid.NewGuid();
            model.PickUpAddress.CivicAddressGuid = Guid.NewGuid();
            model.Size.TruckCapacityUnitGuid = Guid.NewGuid();

            OrderRequestService.CreateOrderRequestInfo(new OrderRequestInfo
            {
                OrderRequestInfoGuid = Guid.NewGuid(),
                DropOffAddress = model.DropOffAddress,
                PickUpAddress = model.PickUpAddress,
                Size = model.Size,
                RequestDate = DateTime.Now,
                Requester = Requester
            });

            OrderService.AssignOrders();
            UnitOfWork.SaveChanges();

            return RedirectToAction("Index", new { message = OrderMessageId.OrderCreatedSuccess });
        }

        // GET: Order/Edit/guid
        [Authorize(Roles = "Customer, Employees")]
        public ActionResult Edit(string guid)
        {
            // Allow for the information to be updated
            // Depends upon IOrderService.Create
            return View();
        }

        // POST: Order/Edit/guid
        [HttpPost]
        [Authorize(Roles = "Customer, Employees")]
        public ActionResult Edit(string guid, FormCollection collection)
        {
            // Depends upon IOrderRequestService.UpdateRequest
            // Save the updated information to the database
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // POST: Order/Cancel/guid
        [HttpPost]
        [Authorize(Roles = "Customer, Employees")]
        public ActionResult Cancel(string id)
        {
            if (!String.IsNullOrEmpty(id))
            {
                try
                {
                    var Guid = new Guid(id);
                    OrderService.CancelOrder(Guid);
                    UnitOfWork.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch
                {
                    return RedirectToAction("Index", new { message = OrderMessageId.CancelOrderError });
                }
            }
            return RedirectToAction("Index", new { message = OrderMessageId.CancelOrderError });
        }

        // POST: Order/Reject/guid
        [HttpPost]
        [Authorize(Roles = "Contractor")]
        public ActionResult Reject(string guid)
        {
            // Contractor has rejected offer and now we must find a new person
            // Depends upon IOrderTrackService.UpdateOrderStatus, IOrderTrackService.UpdateOrderAssignee, IContractorService.UpdateContractorIsAvailable
            return View();
        }

        // GET: Order/Confirmation/guid
        [Authorize(Roles = "Contractor")]
        public ActionResult Confirmation(string id)
        {
            var model = new ConfirmationViewModel
            {
                Guid = new Guid(id)
            };
            return View(model);
        }

        
        // POST: Order/Confirmation/guid
        [Authorize(Roles = "Contractor")]
        [HttpPost]
        [ActionName("Confirmation")]
        public ActionResult ConfirmationPost(ConfirmationViewModel model )
        {
            if (ModelState.IsValid)
            {
                try
                {
                    OrderService.SetOrderAsComplete(model.Guid);
                    UnitOfWork.SaveChanges();

                    return RedirectToAction("Index", "Contractor");
                }
                catch
                {
                    return RedirectToAction("Index", "Contractor", new { Message = OrderMessageId.CouldNotUpdateStatus });
                }
            }
            else
                return RedirectToAction("Index", "Contractor", new { Message = OrderMessageId.Error });
        }

        //[Authorize (Roles = "Employee")]
        public ActionResult AllOrders()
        {
            var orders = OrderService.GetOrders();
            return View("Order", orders);
        }

        public ActionResult Orders()
        {
            var customerInfo = UserInfoService.GetUserCustomerInfo(User.Identity.GetUserId());
            try
            {
                var orders = OrderService.GetOrders(customerInfo);
                return View("Order", orders);
            } 
            catch
            {
                return RedirectToAction("Index", new { message = OrderMessageId.Error });
            }
        }

    }
}
