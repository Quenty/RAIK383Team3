<<<<<<< HEAD
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PracticalWerewolf.Controllers
{
    public class TruckController : Controller
    {
        // GET: Truck
        public ActionResult Index()
        {
=======
﻿using System.Web.Mvc;

namespace PracticalWerewolf.Controllers
{

    public class TruckController : Controller
    {
        // GET: Truck
        [Authorize(Roles = "Employees")]
        public ActionResult Index()
        {
            //shows list of all truck options
>>>>>>> integration
            return View();
        }

        // GET: Truck/Details/5
<<<<<<< HEAD
        public ActionResult Details(int id)
        {
=======
        [Authorize(Roles = "Contractor, Employees")]
        public ActionResult Details(int id)
        {
            //Gets details on a specific truck
>>>>>>> integration
            return View();
        }

        // GET: Truck/Create
<<<<<<< HEAD
        public ActionResult Create()
        {
=======
        [Authorize(Roles = "Contractor")]
        public ActionResult Create()
        {
            //Takes contractors to a form to add a truck to their account
>>>>>>> integration
            return View();
        }

        // POST: Truck/Create
        [HttpPost]
<<<<<<< HEAD
        public ActionResult Create(FormCollection collection)
        {
=======
        [Authorize(Roles = "Contractor")]
        public ActionResult Create(FormCollection collection)
        {
            //Updates the database with the new truck
>>>>>>> integration
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

        // GET: Truck/Edit/5
<<<<<<< HEAD
        public ActionResult Edit(int id)
        {
=======
        [Authorize(Roles = "Contractor")]
        public ActionResult Edit(int id)
        {
            //User is taken to a page where they can change information
>>>>>>> integration
            return View();
        }

        // POST: Truck/Edit/5
        [HttpPost]
<<<<<<< HEAD
        public ActionResult Edit(int id, FormCollection collection)
        {
=======
        [Authorize(Roles = "Contractor")]
        public ActionResult Edit(int id, FormCollection collection)
        {
            //The database is updated with the new user information
>>>>>>> integration
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
<<<<<<< HEAD

        // GET: Truck/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Truck/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
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
=======
>>>>>>> integration
    }
}
