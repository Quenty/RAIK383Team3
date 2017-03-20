using PracticalWerewolf.Models.Trucks;
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

namespace PracticalWerewolf.Controllers
{
    [Authorize(Roles = "Employee, Contractor")]
    [RequireHttps]
    public class TruckController : Controller
    {
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
        public ActionResult Index()
        {
            String userName = System.Web.HttpContext.Current.User.Identity.Name;
            ApplicationUser user = UserManager.FindByName(userName);
            ApplicationUser fullUser = UserManager.Users.Where(u => u.Id == user.Id).FirstOrDefault();

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
                    //AvailableCapacity = item.AvailableCapacity, //TODO: uncomment once we have actual data
                    Owner = owner
                };
                truckModels.Add(toAdd);
            }
            var model = new TruckIndexViewModel
            {
                Trucks = truckModels,
                HasTruck = fullUser.ContractorInfo.Truck != null ? true : false
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
                        // AvailableCapacity = truck.AvailableCapacity, // TODO: uncomment once there's data for this
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
                    //TODO log it
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

                    return RedirectToAction("Index");

                }
                catch
                {
                    //TODO log it
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
                var userName = System.Web.HttpContext.Current.User.Identity.Name;
                var user = UserManager.FindByName(userName);
                Guid TruckGuid = Guid.NewGuid();
                var capacityUnit = new TruckCapacityUnit
                {
                    TruckCapacityUnitGuid = Guid.NewGuid(),
                    Mass = returnedModel.Mass,
                    Volume = returnedModel.Volume
                };
                var model = new Truck
                {
                    TruckGuid = TruckGuid,
                    LicenseNumber = returnedModel.LicenseNumber,
                    MaxCapacity = capacityUnit,
                    Location = LocationHelper.CreatePoint(returnedModel.Lat, returnedModel.Long)
                };
                TruckService.CreateTruck(model);
                ContractorService.UpdateContractorTruck(model, user);
                UnitOfWork.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Index", new { Message = "Could not create truck successfully." });
            }
        }
    }
}