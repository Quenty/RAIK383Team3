using Hangfire;
using Microsoft.AspNet.Identity;
using PracticalWerewolf.Controllers.UnitOfWork;
using PracticalWerewolf.Models;
using PracticalWerewolf.Models.Orders;
using PracticalWerewolf.Models.UserInfos;
using PracticalWerewolf.Services.Interfaces;
using PracticalWerewolf.ViewModels.Contractor;
using PracticalWerewolf.ViewModels.Orders;
using System;
using System.Linq;
using System.Threading.Tasks;
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
        private readonly IRoutePlannerService RoutePlannerService;
        private readonly ApplicationUserManager UserManager;
        private readonly EmailService EmailService;

        public enum OrderMessageId
        {
            OrderCreatedSuccess,
            OrderCreatedError,
            CancelOrderError,
            CouldNotUpdateStatus,
            CouldNotFindOrderError,
            Error
        }

        public OrderController(IOrderRequestService OrderRequestService, IOrderTrackService OrderTrackService, 
            IUserInfoService UserInfoService, IUnitOfWork UnitOfWork, ApplicationUserManager UserManager, IOrderService OrderService, IRoutePlannerService RoutePlannerService, EmailService EmailService)
        {
            this.OrderRequestService = OrderRequestService;
            this.OrderTrackService = OrderTrackService;
            this.UserInfoService = UserInfoService;
            this.UnitOfWork = UnitOfWork;
            this.UserManager = UserManager;
            this.OrderService = OrderService;
            this.RoutePlannerService = RoutePlannerService;
            this.EmailService = EmailService;
        }

        private PagedOrderListViewModel GetOrderHistoryPage()
        {
            ApplicationUser user = UserManager.FindById(User.Identity.GetUserId());
            var CustomerInfoGuid = user.CustomerInfo.CustomerInfoGuid;

            var model = new PagedOrderListViewModel
            {
                DisplayName = "Order history",
                Orders = OrderService.GetOrderHistory(CustomerInfoGuid)
            };

            return model;
        }

        public ActionResult Index(OrderMessageId? message)
        {
            ViewBag.StatusMessage = message == OrderMessageId.OrderCreatedSuccess ? "Order placed successfully."
                : message == OrderMessageId.OrderCreatedError ? "Internal error. Try placing your order again."
                : message == OrderMessageId.Error ? "Something went wrong, please try again!"
                : message == OrderMessageId.CancelOrderError ? "Internal error. Please try to cancel order again."
                : message == OrderMessageId.CouldNotUpdateStatus ? "Internal error. Could not update order status, try again."
                : message == OrderMessageId.CouldNotFindOrderError ? "System error. Could not find the order you were looking for."
                : "";

            var model = new PracticalWerewolf.ViewModels.Orders.OrderIndex();
            if (User.IsInRole("Customer"))
            {
                model.PagedOrderListViewModel = GetOrderHistoryPage();
            }

            return View(model);
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
        public async Task<ActionResult> Create(CreateOrderRequestViewModel model)
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

            var request = new OrderRequestInfo
            {
                OrderRequestInfoGuid = Guid.NewGuid(),
                DropOffAddress = model.DropOffAddress,
                PickUpAddress = model.PickUpAddress,
                Size = model.Size,
                RequestDate = DateTime.Now,
                Requester = Requester
            };

            OrderRequestService.CreateOrderRequestInfo(request);

            UnitOfWork.SaveChanges();
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            await EmailService.SendOrderConfirmEmail(request, user);

            BackgroundJob.Enqueue(() =>  RoutePlannerService.AssignOrders()  );
            

            return RedirectToAction("Index", new { message = OrderMessageId.OrderCreatedSuccess });
        }

        [Authorize(Roles = "Customer")]
        public ActionResult History()
        {


            return View(GetOrderHistoryPage());
        }

        // GET: Order/Edit/guid
        [Authorize(Roles = "Customer, Employees")]
        public ActionResult Edit(string id)
        {
            // Allow for the information to be updated
            // Depends upon IOrderService.Create
            return View();
        }

        //GET: /Order/Order/id
        public ActionResult Order(string id)
        {
            if (id != null)
            {
                Guid guid = new Guid(id);
                Order order = OrderService.GetOrder(guid);
                if (order == null)
                {
                    return RedirectToAction("Index", new { Message = OrderMessageId.CouldNotFindOrderError });
                }
                ApplicationUser customer = UserManager.Users.Single(u => u.CustomerInfo.CustomerInfoGuid == order.RequestInfo.Requester.CustomerInfoGuid);
                ApplicationUser driver = null;
                if (order.TrackInfo.Assignee != null)
                {
                    driver = UserManager.Users.Single(u => u.ContractorInfo.ContractorInfoGuid == order.TrackInfo.Assignee.ContractorInfoGuid);
                }

                var model = new OrderDetailsViewModel
                {
                    DropOffAddress = order.RequestInfo.DropOffAddress,
                    PickUpAddress = order.RequestInfo.PickUpAddress,
                    Size = order.RequestInfo.Size,
                    RequestDate = order.RequestInfo.RequestDate,
                    Customer = customer,
                    Contractor = driver
                };

                return View(model);
            }
            else
            {
                return RedirectToAction("Index", new { Message = OrderMessageId.Error });
            }
        }

        // POST: Order/Edit/guid
        [HttpPost]
        [Authorize(Roles = "Customer, Employees")]
        public ActionResult Edit(string id, FormCollection collection)
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

        public ActionResult SetInTruck(Order order)
        {

            OrderStatusViewModel status = new OrderStatusViewModel
            {
                orderId = order.OrderGuid,
                orderStatus = order.TrackInfo.OrderStatus,
                inTruck = (order.TrackInfo.CurrentTruck == null)
            };
            return PartialView("_SetOrderInTruck", status);
        }

        [Authorize(Roles = "Contractor")]
        [HttpPost]
        public ActionResult SetInTruck(Guid orderId)
        {
            OrderService.SetOrderInTruck(orderId);
            UnitOfWork.SaveChanges();
            return View("_UpdateMessage");
        }

        // POST: Order/Confirmation/guid
        [Authorize(Roles = "Contractor")]
        [HttpPost]
        [ActionName("Confirmation")]
        public ActionResult ConfirmationPost(ConfirmationViewModel model)
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
