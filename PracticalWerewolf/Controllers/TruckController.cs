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

namespace PracticalWerewolf.Controllers
{
    public class TruckController : Controller
    {
        ITruckService TruckService;
        IContractorService ContractorService;

        public TruckController(ITruckService TruckService, IContractorService ContractorService)
        {
            this.TruckService = TruckService;
            this.ContractorService = ContractorService;
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
                if(truck != null)
                {
                    var model = new TruckDetailsViewModel
                    {
                        Guid = id,
                        LicenseNumber = truck.LicenseNumber,
                        AvailableCapacity = truck.GetAvailableCapacity(),
                        MaxCapacity = truck.MaxCapacity,
                        Lat = truck.Location.Latitude,
                        Long = truck.Location.Longitude
                    };
                    return View(model);
                }
            }
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
                if (truck != null)
                {
                    var model = new TruckUpdateViewModel
                    {
                        Guid = id,
                        LicenseNumber = truck.LicenseNumber,
                        Volume = truck.MaxCapacity.Volume,
                        Mass = truck.MaxCapacity.Mass
                    };

                    return View(model);
                }
            }

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
                    var newCapacity = new TruckCapacityUnit
                    {
                        TruckCapacityUnitGuid = new Guid(model.Guid),
                        Volume = model.Volume,
                        Mass = model.Mass
                    };
                    //Here's where the IdentityResultWouldBeNice
                    TruckService.UpdateTruckMaxCapacity(guid, newCapacity);
                    return RedirectToAction("Index");
                }
                else
                {
                    return View(model);
                }
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
                try
                {
                    var capacityUnit = new TruckCapacityUnit
                    {
                        //Guid?
                        Mass = truck.MaxCapacity.Mass,
                        Volume = truck.MaxCapacity.Volume
                    };
                    //what's wrong with using the truck they gave us?
                    var model = new Truck
                    {
                        //Guid?
                        //Will there be a problem if a truck has the same license number?
                        //Do we need to set the current capacity or location?
                        //Are we going to require they have a license number?
                        LicenseNumber = truck.LicenseNumber,
                        MaxCapacity = capacityUnit
                    };
                    TruckService.CreateTruck(model);
                    return RedirectToAction("Index");

                }
                catch
                {
                    //TODO: LOG IT
                }
            }
            return HttpNotFound();
        }

    }
}
