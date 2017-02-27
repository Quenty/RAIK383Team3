using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PracticalWerewolf.Controllers
{
    public class OrderController : Controller
    {
        // GET: Order/Index
        [Authorize (Roles= "Employees")]
        public ActionResult Index()
        {
            // Users will see a list of all orders

            // Depends upon IOrderRequestService.GetCustomerOrders
            return View();
        }

        // GET: Order/Index/guid
        [Authorize(Roles = ("Customers"))]
        public ActionResult Index(string guid)
        {
            // Customer or contractor will see a list of past and present orders associated to them
            // Depends upon IOrderRequestService.GetCustomerOrders
            return View();
        }

        // GET: Order/ContractedIndex/guid
        [Authorize(Roles = ("Contractors"))]
        public ActionResult ContractedIndex(string guid)
        {
            // Customer or contractor will see a list of past and present orders associated to them
            // Depends upon IOrderTrackService.GetContractorOrders
            return View();
        }

        // GET: Order/Details/guid
        [Authorize (Roles = "Employees, Contractors, Customers")]
        public ActionResult Details(string guid)
        {
            // Will get detailed information on a specific order
            // Depends upon IOrderService.GetOrdersByCustomerInfo, IOrderService
            return View();
        }

        // GET: Order/Create
        [Authorize (Roles = "Customer, Employees")]
        public ActionResult Create()
        {
            // Takes user to a form to create a new order
            return View();
        }

        // POST: Order/Create
        [HttpPost]
        [Authorize(Roles = "Customer, Employees")]
        public ActionResult Create(FormCollection collection)
        {
            // Takes the info from customer or employee and updates the database
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

        // GET: Order/Edit/5
        [Authorize(Roles = "Customer, Employees")]
        public ActionResult Edit(int id)
        {
            // Allow for the information to be updated
            return View();
        }

        // POST: Order/Edit/5
        [HttpPost]
        [Authorize(Roles = "Customer, Employees")]
        public ActionResult Edit(int id, FormCollection collection)
        {
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

        // GET: Order/Delete/5
        [Authorize(Roles = "Customer, Employees")]
        public ActionResult Delete(int id)
        {
            // Gives customer and employee the option to delete an order
            return View();
        }

        // POST: Order/Delete/5
        [HttpPost]
        [Authorize(Roles = "Customer, Employees")]
        public ActionResult Delete(int id, FormCollection collection)
        {
            // Updates the database by removing the specific order
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

        // POST: Order/Reject/5
        [HttpPost]
        [Authorize(Roles = "Contractor")]
        public ActionResult Reject(int id)
        {
            // Contractor has rejected offer and now we must find a new person
            return View();
        }

        // GET: Order/Confirmation/5
        [Authorize(Roles = "Contractor")]
        public ActionResult Confirmation(int id)
        {
            // Page for the contractor to collect signature once product is delivered
            return View();
        }

        // POST: Order/Confirmation/5
        [Authorize(Roles = "Contractor")]
        public ActionResult Confirmation(int id, FormCollection collection)
        {
            // Updates the database by marking the order as completed
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
