﻿using Microsoft.AspNet.Identity;
using PracticalWerewolf.Controllers.UnitOfWork;
using PracticalWerewolf.Models.Orders;
using PracticalWerewolf.Models.UserInfos;
using PracticalWerewolf.Services.Interfaces;
using PracticalWerewolf.ViewModels.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PracticalWerewolf.Controllers
{
    [RequireHttps]
    public class OrderController : Controller
    {
        private readonly IOrderRequestService OrderRequestService;
        private readonly IOrderTrackService OrderTrackService;
        private readonly IUserInfoService UserInfoService;
        private readonly IUnitOfWork UnitOfWork;
        private readonly ApplicationUserManager UserManager;

        public OrderController(IOrderRequestService OrderRequestService, IOrderTrackService OrderTrackService, 
            IUserInfoService UserInfoService, IUnitOfWork UnitOfWork, ApplicationUserManager UserManager)
        {
            this.OrderRequestService = OrderRequestService;
            this.OrderTrackService = OrderTrackService;
            this.UserInfoService = UserInfoService;
            this.UnitOfWork = UnitOfWork;
            this.UserManager = UserManager;
        }

        // GET: Order/Index
        [Authorize (Roles= "Employees")]
        public ActionResult Index()
        {
            // Users will see a list of all orders

            // Depends upon IOrderRequestService.GetCustomerOrders
            return View();
        }

        // GET: Order/Index/guid
        [Authorize(Roles = ("Customer"))]
        public ActionResult Index(string guid)
        {
            // Customer will see a list of past and present orders associated to them
            // Depends upon IOrderRequestService.GetCustomerOrders
            return View();
        }

        // GET: Order/ContractedIndex/guid
        [Authorize(Roles = ("Contractors"))]
        public ActionResult ContractedIndex(string guid)
        {
            // Contractor will see a list of past and present orders associated to them
            // Depends upon IOrderTrackService.GetContractorOrders
            return View();
        }

        // GET: Order/Details/guid
        [Authorize (Roles = "Employees, Contractors, Customers")]
        public ActionResult Details(string guid)
        {
            // Will get detailed information on a specific order
            // Depends upon IOrderService.GetByUserGuids
            return View();
        }

        // GET: Order/Create
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

            OrderRequestService.CreateOrderRequestInfo(new OrderRequestInfo
            {
                OrderRequestInfoGuid = Guid.NewGuid(),
                DropOffAddress = model.DropOffAddress,
                PickUpAddress = model.PickUpAddress,
                Size = model.Size,
                RequestDate = DateTime.Now,
                Requester = Requester
            });

            UnitOfWork.SaveChanges();

            return RedirectToAction("Index");
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

        // GET: Order/Cancel/guid
        [Authorize(Roles = "Customer, Employees")]
        public ActionResult Cancel(string guid)
        {
            // Gives customer and employee the option to cancel an order
            // Depends upon IOrderRequestService.GetOrdersByCustomerInfo
            return View();
        }

        // POST: Order/Cancel/guid
        [HttpPost]
        [Authorize(Roles = "Customer, Employees")]
        public ActionResult Cancel(string guid, FormCollection collection)
        {
            // Updates the database by removing the specific order
            // Depends upon IOrderTrackService.UpdateOrderStatus
            try
            {
                // TODO: Add cancel logic here
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
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
        public ActionResult Confirmation(string guid)
        {
            // Page for the contractor to collect signature once product is delivered
            // Depends upon IOrderTrackService.UpdateOrderStatus, ITruckService.UpdateTruckCurrentCapacity
            return View();
        }

        // POST: Order/Confirmation/guid
        [Authorize(Roles = "Contractor")]
        public ActionResult Confirmation(string guid, FormCollection collection)
        {
            // Updates the database by marking the order as completed
            // Depends upon IOrderTrackService.UpdateOrderStatus, ITruckService.UpdateTruckCurrentCapacity
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
