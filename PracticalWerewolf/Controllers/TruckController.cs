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
using log4net;
using PracticalWerewolf.Helpers;

namespace PracticalWerewolf.Controllers
{
    [Authorize(Roles = "Employee, Contractor")]
    [RequireHttps]
    public class TruckController : Controller
    {
        public enum TruckMessageId
        {
            Error,
            TruckNotFoundError,
            TruckUpdateSuccess,
            TruckUpdateError,
            TruckCreationError,
            TruckLocationUpdateError,
            TruckLocationUpdatedSuccess
        }

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

        private void GenerateErrorMessage(TruckMessageId? message)
        {
            ViewBag.StatusMessage =
                message == TruckMessageId.Error ? "Internal error. Please try again."
                : message == TruckMessageId.TruckNotFoundError ? "Internal error, could not find truck."
                : message == TruckMessageId.TruckUpdateSuccess ? "Your truck was updated successfully."
                : message == TruckMessageId.TruckUpdateError ? "Could not update truck, please try again."
                : message == TruckMessageId.TruckCreationError ? "Could not create truck successfully."
                : message == TruckMessageId.TruckLocationUpdateError ? "Could not update truck location successfully"
                : message == TruckMessageId.TruckLocationUpdatedSuccess ? "Truck location updated successfully"
                : "";
        }



        [OverrideAuthorization]
        [Authorize(Roles = "Employee")]
        public ActionResult Index()
        {
             IEnumerable<Truck> trucks = TruckService.GetAllTrucks();
            List<TruckDetailsViewModel> truckModels = new List<TruckDetailsViewModel>();

            foreach (Truck item in trucks)
            {
                ContractorInfo contractor = ContractorService.GetContractorByTruckGuid(item.TruckGuid);
                if( contractor != null )
                {
                    var owner = UserManager.Users.FirstOrDefault(u => u.ContractorInfo.ContractorInfoGuid == contractor.ContractorInfoGuid);
                    var toAdd = new TruckDetailsViewModel
                    {
                        Guid = item.TruckGuid,
                        LicenseNumber = item.LicenseNumber,
                        Lat = item.Location.Latitude,
                        Long = item.Location.Longitude,
                        MaxCapacity = item.MaxCapacity,
                        AvailableCapacity = item.GetAvailableCapacity(),
                        Owner = owner
                    };
                    truckModels.Add(toAdd);
                }
                
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
                    ApplicationUser owner = contractor == null ? null : UserManager.Users.FirstOrDefault(u => u.ContractorInfo.ContractorInfoGuid == contractor.ContractorInfoGuid);
                    var model = new TruckDetailsViewModel
                    {
                        Guid = new Guid(id),
                        LicenseNumber = truck.LicenseNumber,
                        AvailableCapacity = truck.GetAvailableCapacity(),
                        MaxCapacity = truck.MaxCapacity,
                        Lat = truck.Location.Latitude,
                        Long = truck.Location.Longitude,
                        Owner = owner
                    };
                    return View(model);
                }

                return View(new { Message = TruckMessageId.TruckNotFoundError }); 
            }
            else
            {
                return View(new { Message = TruckMessageId.Error }); 
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
                    if (truck == null)
                    {
                        throw new Exception("Truck is null");
                    }
                    if (!User.IsInRole("Employee"))
                    {
                        var UserInfo = UserManager.FindById(User.Identity.GetUserId());
                        if (UserInfo.ContractorInfo.Truck != truck)
                        {
                            throw new Exception("Cannot edit other user's truck");
                        }
                    }

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
            return View(new { Message = TruckMessageId.TruckNotFoundError });
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

                    return RedirectToAction("Index", "Contractor", new { Message = TruckMessageId.TruckUpdateSuccess });

                }
                catch
                {
                    logger.Error("Edit(id, ViewModel) - Error getting Truck, creating new TruckCapacityUnit, in TruckService.UpdateCapacity(), or TruckService.UpdateLicenseNumber()");
                }
            }
            else
            {
                return View(model);
            }

            return View("index", new { Message = TruckMessageId.TruckUpdateError } );
        }

        // GET: Truck/Create/
        public ActionResult Create()
        {
            return View();
        }

        // POST: Truck/Create/
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Contractor")]
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
                        Location = LocationHelper.CreatePoint(returnedModel.Lat, returnedModel.Long),
                        UsedCapacity = new TruckCapacityUnit(TruckCapacityUnit.Zero) {
                            TruckCapacityUnitGuid = Guid.NewGuid(),
                        }
                    };
                    TruckService.CreateTruck(model);
                    ContractorService.UpdateContractorTruck(model, user);
                    UnitOfWork.SaveChanges();
                    return RedirectToAction("Index", "Contractor");

                }
                catch (Exception e)
                {
                    logger.Error("Create(ViewModel) - Error getting user, creating TruckCapacityUnit, creating Truck, or ContractorService.UpdateContractorTruck()", e);
                }
            }
            else
            {
                return View(returnedModel);
            }

            return RedirectToAction("Index", "Contractor",new { Message = TruckMessageId.TruckCreationError });
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
            return View("Index", new { Message = TruckMessageId.Error } );
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
                    return Redirect(Url.Action("Index", "Contractor", new { Message = TruckMessageId.TruckLocationUpdatedSuccess }) + "#truck");
                }
                catch
                {
                    logger.Error("UpdateLocation - Error in UpdateTruckLocation");
                    return Redirect(Url.Action("Index", "Contractor", new { Message = TruckMessageId.TruckLocationUpdateError }) + "#truck");
                }
            }
            else
            {
                return View("Index", new { Message = TruckMessageId.Error });
            }
        }

    }
}
