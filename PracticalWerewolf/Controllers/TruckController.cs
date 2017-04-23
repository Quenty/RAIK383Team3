﻿using PracticalWerewolf.Models.Trucks;
using PracticalWerewolf.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using PracticalWerewolf.ViewModels;
using Microsoft.AspNet.Identity;
using PracticalWerewolf.Models;
using PracticalWerewolf.Models.UserInfos;
using PracticalWerewolf.Controllers.UnitOfWork;
using System.Activities;
using System.Data.Entity.Spatial;
using PracticalWerewolf.Application;
using log4net;
using PracticalWerewolf.Services;

namespace PracticalWerewolf.Controllers
{
    [Authorize(Roles = "Employee, Contractor")]
    [RequireHttps]
    public class TruckController : Controller
    {

        private static readonly ILog logger = LogManager.GetLogger(typeof(TruckController));
        ITruckService TruckService;
        IContractorService ContractorService;
        IUnitOfWork UnitOfWork;
        ApplicationUserManager UserManager;

        public TruckController(ITruckService TruckService, IContractorService ContractorService, IUnitOfWork UnitOfWork, ApplicationUserManager userManager)
        {
            this.UnitOfWork = UnitOfWork;
            this.TruckService = TruckService;
            this.ContractorService = ContractorService;
            this.UserManager = userManager;
        }

        // TODO: Only show active trucks to employees
        [OverrideAuthorization]
        [Authorize(Roles = "Employee")]
        public ActionResult Index()
        {
            String userName = User.Identity.Name;
            ApplicationUser user = UserManager.FindByName(userName);

            IEnumerable<Truck> trucks = TruckService.GetAllTrucks();
            List<TruckDetailsViewModel> truckModels = new List<TruckDetailsViewModel>();

            foreach (Truck item in trucks)
            {
                ContractorInfo contractor = ContractorService.GetContractorByTruckGuid(item.TruckGuid);
                var owner = contractor == null ? null : UserManager.Users.Single(u => u.ContractorInfo.ContractorInfoGuid == contractor.ContractorInfoGuid);
                var toAdd = new TruckDetailsViewModel
                {
                    Guid = item.TruckGuid,
                    LicenseNumber = item.LicenseNumber,
                    Lat = item.Location.Latitude,
                    Long = item.Location.Longitude,
                    MaxCapacity = item.MaxCapacity,
                    AvailableCapacity = item.AvailableCapacity,
                    Owner = owner
                };
                truckModels.Add(toAdd);
            }

            var model = new TruckIndexViewModel
            {
                Trucks = truckModels,
            };
            return View(model);
        }

        // GET: Truck/Details/guid
        public ActionResult Details(string id)
        {
            if (!String.IsNullOrEmpty(id))
            {
                var guid = new Guid(id);
                Truck truck = TruckService.GetTruck(guid);
                if (truck != null)
                {
                    ContractorInfo contractor = ContractorService.GetContractorByTruckGuid(guid);
                    ApplicationUser owner = contractor == null ? null : UserManager.Users.Single(u => u.ContractorInfo.ContractorInfoGuid == contractor.ContractorInfoGuid);
                    var model = new TruckDetailsViewModel
                    {
                        Guid = new Guid(id),
                        LicenseNumber = truck.LicenseNumber,
                        AvailableCapacity = truck.AvailableCapacity, // TODO: uncomment once there's data for this
                        MaxCapacity = truck.MaxCapacity,
                        Lat = truck.Location.Latitude,
                        Long = truck.Location.Longitude,
                        Owner = owner
                    };
                    return View(model);
                }

                return HttpNotFound(); // TODO: Use StatusMessage template and an Error enum
            }
            else
            {
                return HttpNotFound(); // TODO: Use StatusMessage template and an Error enum
            }
        }

        // GET: Truck/Edit/guid
        public ActionResult Edit(string id)
        {
            if (!String.IsNullOrEmpty(id))
            {
                try
                {

                    var guid = new Guid(id);
                    var truck = TruckService.GetTruck(guid);
                    var model = new TruckUpdateViewModel
                    {
                        Guid = guid,
                        LicenseNumber = truck.LicenseNumber,
                        Volume = truck.MaxCapacity.Volume,
                        Mass = truck.MaxCapacity.Mass
                    };

                    return View(model);
                }
                catch
                {
                    logger.Error("Edit() - error getting truck or creating ViewModel");
                }
            }
            return HttpNotFound();
        }

        // POST: Truck/Update/guid
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(String id, TruckUpdateViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var oldTruck = TruckService.GetTruck(model.Guid);
                    var NewCapacityModel = new TruckCapacityUnit
                    {
                        TruckCapacityUnitGuid = Guid.NewGuid(),
                        Volume = model.Volume,
                        Mass = model.Mass
                    };

                    TruckService.UpdateCapacity(model.Guid, NewCapacityModel);
                    TruckService.UpdateLicenseNumber(model.Guid, model.LicenseNumber);

                    UnitOfWork.SaveChanges();

                    return RedirectToAction("Index", "Contractor");

                }
                catch
                {
                    logger.Error("Edit(id, ViewModel) - Error getting Truck, creating new TruckCapacityUnit, in TruckService.UpdateCapacity(), or TruckService.UpdateLicenseNumber()");
                }
            }
            return View(model);
        }

        // GET: Truck/Create/
        public ActionResult Create()
        {
            return View();
        }

        // POST: Truck/Create/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TruckCreateViewModel returnedModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var userName = User.Identity.Name;
                    var user = UserManager.FindByName(userName);
                    var capacityUnit = new TruckCapacityUnit
                    {
                        TruckCapacityUnitGuid = Guid.NewGuid(),
                        Mass = returnedModel.Mass,
                        Volume = returnedModel.Volume
                    };
                    var model = new Truck
                    {
                        TruckGuid = Guid.NewGuid(),
                        LicenseNumber = returnedModel.LicenseNumber,
                        MaxCapacity = capacityUnit,
                        Location = LocationHelper.CreatePoint(returnedModel.Lat, returnedModel.Long)
                    };
                    TruckService.CreateTruck(model);
                    ContractorService.UpdateContractorTruck(model, user);
                    UnitOfWork.SaveChanges();
                    return RedirectToAction("Index", "Contractor");

                }
                catch
                {
                    logger.Error("Create(ViewModel) - Error getting user, creating TruckCapacityUnit, creating Truck, or ContractorService.UpdateContractorTruck()");
                }
            }
            return RedirectToAction("Index", "Contractor",new { Message = ContractorController.ContractorMessageId.TruckCreationError });
        }

        public ActionResult Location(string id)
        {
            if (!String.IsNullOrEmpty(id))
            {
                try
                {

                    var guid = new Guid(id);
                    var truck = TruckService.GetTruck(guid);
                    var model = new TruckUpdateLocation
                    {
                        Guid = guid,
                        Lat = truck.Location.Latitude,
                        Long = truck.Location.Longitude

                    };

                    return View(model);
                }
                catch
                {
                    logger.Error("Edit() - error getting truck or creating ViewModel");
                }
            }
            return HttpNotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Location(TruckUpdateLocation returnedModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var location = LocationHelper.CreatePoint(returnedModel.Lat, returnedModel.Long);

                    TruckService.UpdateTruckLocation(returnedModel.Guid, location);
                    UnitOfWork.SaveChanges();
                    return Redirect(Url.Action("Index", "Contractor", new { Message = ContractorController.ContractorMessageId.TruckLocationUpdatedSuccess }) + "#truck");
                }
                catch
                {
                    logger.Error("UpdateLocation - Error in UpdateTruckLocation");
                    return Redirect(Url.Action("Index", "Contractor", new { Message = ContractorController.ContractorMessageId.TruckLocationUpdateError }) + "#truck");
                }
            }
            else
            {
                return View(returnedModel);
            }
        }

    }
}