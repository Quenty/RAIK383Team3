using PracticalWerewolf.Models.Trucks;
using PracticalWerewolf.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PracticalWerewolf.ViewModels;

namespace PracticalWerewolf.Controllers
{

    public class TruckController : Controller
    {
        ITruckService TruckService;

        public TruckController(ITruckService TruckService)
        {
            this.TruckService = TruckService;
        }

        // GET: Truck
        [Authorize(Roles = "Employees")]
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
        [Authorize(Roles = "Contractor, Employees")]
        public ActionResult Details(string guid)
        {

            // Gets details on a specific truck
            // Depends upon TruckService.Get
            return View();
        }

        // GET: Truck/Create
        [Authorize(Roles = "Contractor")]
        public ActionResult Create()
        {
            // Takes contractors to a form to add a truck to their account
            // Shouldn't depend upon nothing
            return View();
        }

        // POST: Truck/Register
        [HttpPost]
        [Authorize(Roles = "Contractor")]
        public ActionResult Register(FormCollection collection)
        {
            // Updates the database with the new truck
            // Depends upon TruckService.Create
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Truck/Edit/guid
        [Authorize(Roles = "Contractor")]
        public ActionResult Edit(int id)
        {
            // User is taken to a page where they can change information
            // Depends upon TruckService.GetTruck

            return View();
        }

        // POST: Truck/Edit/guid
        [HttpPost]
        [Authorize(Roles = "Contractor")]
        public ActionResult Edit(int id, FormCollection collection)
        {
            // The database is updated with the new user information
            // Depends upon TruckService.UpdateTruckLocation, TruckService.UpdateTruckCurrentCapacity, TruckService.UpdateTruckMaxCapacity
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
    }
}
