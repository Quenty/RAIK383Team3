using System.Web.Mvc;

namespace PracticalWerewolf.Controllers
{

    public class TruckController : Controller
    {
        // GET: Truck
        [Authorize(Roles = "Employees")]
        public ActionResult Index()
        {
            //shows list of all truck options
            return View();
        }

        // GET: Truck/Details/5
        [Authorize(Roles = "Contractor, Employees")]
        public ActionResult Details(int id)
        {
            //Gets details on a specific truck
            return View();
        }

        // GET: Truck/Create
        [Authorize(Roles = "Contractor")]
        public ActionResult Create()
        {
            //Takes contractors to a form to add a truck to their account
            return View();
        }

        // POST: Truck/Create
        [HttpPost]
        [Authorize(Roles = "Contractor")]
        public ActionResult Create(FormCollection collection)
        {
            //Updates the database with the new truck
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
        [Authorize(Roles = "Contractor")]
        public ActionResult Edit(int id)
        {
            //User is taken to a page where they can change information
            return View();
        }

        // POST: Truck/Edit/5
        [HttpPost]
        [Authorize(Roles = "Contractor")]
        public ActionResult Edit(int id, FormCollection collection)
        {
            //The database is updated with the new user information
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
