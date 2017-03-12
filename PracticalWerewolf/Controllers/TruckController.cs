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
        [Authorize (Roles = "Employee")]
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
            var guid = new Guid(id);
            var truck = TruckService.GetTruck(guid);
            var model = new TruckUpdateViewModel
            {
                Guid = id,
                Volume = truck.MaxCapacity.Volume,
                Mass = truck.MaxCapacity.Mass
            };

            return View(model);
        }

        // POST: Truck/Update/guid
        [HttpPost]
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
    }
}
