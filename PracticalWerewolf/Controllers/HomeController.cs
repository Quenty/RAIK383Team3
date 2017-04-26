using PracticalWerewolf.Models.Home;
using PracticalWerewolf.Models.Orders;
using PracticalWerewolf.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace PracticalWerewolf.Controllers
{ 
    public class HomeController : Controller
    {
        private readonly IOrderService OrderService;
        private readonly IUserInfoService UserInfoService;

        public HomeController(IOrderService OrderService, IUserInfoService UserInfoService)
        {
            this.OrderService = OrderService;
            this.UserInfoService = UserInfoService;
        }

        public ActionResult Index()
        {
            int delivered = OrderService.QueryCount(x => x.TrackInfo.OrderStatus == OrderStatus.Complete);
            int inprogress = OrderService.QueryCount(x => x.TrackInfo.OrderStatus == OrderStatus.InProgress);
            int unassigned = OrderService.QueryCount(x => x.TrackInfo.OrderStatus == OrderStatus.Queued);

            int customers = UserInfoService.QueryCount(x => x.CustomerInfo != null);
            int contractors = UserInfoService.QueryCount(x => x.ContractorInfo != null);
            int employees = UserInfoService.QueryCount(x => x.EmployeeInfo != null);

            var model = new Statistics
            {
                PackageStateChart = new DonutChart<int>
                {
                   Labels = new List<String> { "Delivered", "In progress", "Unassigned"},
                   Data = new List<int> { delivered, inprogress, unassigned }
                },
                UsersChart = new DonutChart<int>
                {
                    Labels = new List<String> { "Customer", "Employee", "Contractor"},
                    Data = new List<int> { customers, employees, contractors },
                    MiddleLabel = UserInfoService.QueryCount(x => true).ToString()
                }
            };

            return View(model);
        }

        public ActionResult About()
        {
           
            return View();
        }
    }
}