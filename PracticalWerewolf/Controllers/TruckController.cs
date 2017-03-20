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
using PracticalWerewolf.Models;
using System.Activities;
using System.Data.Entity.Spatial;
using PracticalWerewolf.Application;

namespace PracticalWerewolf.Controllers
{
    [RequireHttps]
    public class TruckController : Controller
    {
        ITruckService TruckService;
        IContractorService ContractorService;
        ApplicationDbContext context;

        public TruckController(ITruckService TruckService, IContractorService ContractorService, ApplicationDbContext context)
        {
            this.context = context;
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
        [Authorize(Roles = "Contractor, Employee")]
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
                        AvailableCapacity = truck.AvailableCapacity,
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
        public ActionResult Edit(string id)
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
        public ActionResult Edit(String id, TruckUpdateViewModel model)
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
                    var newModel = new Truck
                    {
                        TruckGuid = new Guid(model.Guid),
                        LicenseNumber = model.LicenseNumber,
                        MaxCapacity = newCapacity
                    };
                    //Here's where the IdentityResultWouldBeNice
                    TruckService.UpdateTruck(newModel);
                    context.SaveChanges();
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
        [Authorize(Roles = "Contractor")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Truck/Create/
        [Authorize(Roles = "Contractor")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TruckCreateViewModel returnedModel)
        {
            if (ModelState.IsValid && returnedModel.LicenseNumber != null && returnedModel.Mass >= 0 && returnedModel.Volume >= 0)
            {
                try
                {
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
                        Location = CreatePoint(returnedModel.Lat, returnedModel.Long)
                    };
                    TruckService.CreateTruck(model);
                    context.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch
                {
                    //Log it
                }
            }
            return HttpNotFound();

        }

        private DbGeography CreatePoint(double lat, double lon, int srid = 4326)
        {
            string wkt = String.Format("POINT({0} {1})", lon, lat);

            return DbGeography.PointFromText(wkt, srid);
        }

    }
}
