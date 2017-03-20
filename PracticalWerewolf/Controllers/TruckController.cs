using PracticalWerewolf.Models.Trucks;
using PracticalWerewolf.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PracticalWerewolf.ViewModels;
using PracticalWerewolf;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using PracticalWerewolf.Controllers.UnitOfWork;

namespace PracticalWerewolf.Controllers
{
    [RequireHttps]
    public class TruckController : Controller
    {
        ITruckService TruckService;
        IContractorService ContractorService;
        IUnitOfWork UnitOfWork;

        public TruckController(ITruckService TruckService, IContractorService ContractorService, IUnitOfWork UnitOfWork)
        {
            this.TruckService = TruckService;
            this.ContractorService = ContractorService;
            this.UnitOfWork = UnitOfWork;
        }

        // GET: Truck
        [Authorize(Roles = "Employee")]
        public ActionResult Index()
        {
            ViewBag.Message = "Trucks, Trucks and even more Trucks!";
            IEnumerable<Truck> trucks = TruckService.GetAllTrucks();
            var model = new TruckIndexViewModel
            {
                Trucks = trucks
            };
            return View(model);
        }

        // GET: Truck/Details/guid
        //[Authorize(Roles = "Contractor, Employee")]
        public ActionResult Details(string id)
        {
            if (!String.IsNullOrEmpty(id))
            {
                var guid = new Guid(id);
                Truck truck = TruckService.GetTruck(guid);
                var model = new TruckDetailsViewModel
                {
                    Guid = id,
                    LicenseNumber = truck.LicenseNumber,
                    AvailableCapacity = truck.AvailableCapacity,
                    MaxCapacity = truck.MaxCapacity,
                    Lat = truck.Location.Latitude,
                    Long = truck.Location.Longitude
                };
                return View(model);
            }
            else
                return HttpNotFound();
        }

        // GET: Truck/Edit/guid
        [Authorize(Roles = "Contractor")]
        public ActionResult Update(string id)
        {
            if (!String.IsNullOrEmpty(id))
            {
                var guid = new Guid(id);
                var truck = TruckService.GetTruck(guid);
                var model = new TruckUpdateViewModel
                {
                    Guid = id,
                    LicenseNumber = truck.LicenseNumber,
                    Volume = truck.MaxCapacity.Volume,
                    Mass = truck.MaxCapacity.Mass
                };

                return View(model);
            }
            else
                return HttpNotFound();
        }

        // POST: Truck/Update/guid
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Contractor")]
        public ActionResult Update(string id, TruckUpdateViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var guid = new Guid(id);
                    var NewModel = new TruckCapacityUnit
                    {
                        TruckCapacityUnitGuid = new Guid(model.Guid),
                        Volume = model.Volume,
                        Mass = model.Mass
                    };
                    TruckService.UpdateTruckMaxCapacity(guid, NewModel);
                    return RedirectToAction("Index");
                }
                else
                    return View(model);
            }
            catch
            {
                return View(model);
            }
        }

        // GET: Truck/Create/
        public ActionResult Create()
        {
            return View();
        }

        // POST: Truck/Create/
        [HttpPost]
        public ActionResult Create(Truck truck)
        {
            if (ModelState.IsValid)
            {
                var capacityUnit = new TruckCapacityUnit
                {
                    Mass = truck.MaxCapacity.Mass,
                    Volume = truck.MaxCapacity.Volume
                };
                var model = new Truck
                {
                    LicenseNumber = truck.LicenseNumber,
                    MaxCapacity = capacityUnit
                };
                TruckService.CreateTruck(model);
                return RedirectToAction("Index");
            }
            else
            {
                return HttpNotFound();
            }
        }

    }
}
