using System.Web.Mvc;

namespace PracticalWerewolf.Controllers
{
    public class OrderController : Controller
    {
        // GET: Order/Index
        [Authorize (Roles= "Employees")]
        public ActionResult Index()
        {
            //Users will see a list of all orders
            return View();
        }

        // GET: Order/Index/5
        [Authorize(Roles = "Contractors, Customers")]
        public ActionResult Index(int id)
        {
            //customer or contractor will see a list of past and present orders associated to them
            return View();
        }

        // GET: Order/Details/5
        [Authorize (Roles = "Employees, Contractors, Customers")]
        public ActionResult Details(int id)
        {
            //Will get detailed information on a specific order
            return View();
        }

        // GET: Order/Create
        [Authorize (Roles = "Customer, Employees")]
        public ActionResult Create()
        {
            //takes user to a form to create a new order
            return View();
        }

        // POST: Order/Create
        [HttpPost]
        [Authorize(Roles = "Customer, Employees")]
        public ActionResult Create(FormCollection collection)
        {
            //Takes the info from customer or employee and updates the database
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
            //Allow for the information to be updated
            return View();
        }

        // POST: Order/Edit/5
        [HttpPost]
        [Authorize(Roles = "Customer, Employees")]
        public ActionResult Edit(int id, FormCollection collection)
        {
            //Save the updated information to the database
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
            //Gives customer and employee the option to delete an order
            return View();
        }

        // POST: Order/Delete/5
        [HttpPost]
        [Authorize(Roles = "Customer, Employees")]
        public ActionResult Delete(int id, FormCollection collection)
        {
            //updates the database by removing the specific order
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
            //contractor has rejected offer and now we must find a new persona
            return View();
        }

        // GET: Order/Confirmation/5
        [Authorize(Roles = "Contractor")]
        public ActionResult Confirmation(int id)
        {
            //Page for the contractor to collect signature once product is delivered
            return View();
        }

        //POST: Order/Confirmation/5
        [Authorize(Roles = "Contractor")]
        public ActionResult Confirmation(int id, FormCollection collection)
        {
            //updates the database by marking the order as completed
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
