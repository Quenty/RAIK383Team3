using PracticalWerewolf.Application;
using PracticalWerewolf.EmailTemplates;
using PracticalWerewolf.Helpers;
using PracticalWerewolf.Models.Orders;
using PracticalWerewolf.Models.Trucks;
using PracticalWerewolf.Models.UserInfos;
using RazorEngine;
using RazorEngine.Templating;
using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace PracticalWerewolf.Controllers
{
    public class DummyEmailController : Controller
    {

        private ApplicationDbContext _context { get; set; }

        public DummyEmailController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: DummyEmail
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> SendEmail()
        {
            var me = _context.Users.First(x => x.Email.Equals("jesseelzhang@gmail.com"));
            if(me == null)
            {
                return View();
            }

            

            await EmailHelper.SendOrderShippedEmail(me.CustomerInfo.OrderRequests.First(), me);

            return View(); ;
        }

        public void Setup()
        {
            var me = _context.Users.First(x => x.Email.Equals("jesseelzhang@gmail.com"));
            if(me.CustomerInfo == null)
            {
                me.CustomerInfo = new CustomerInfo()
                {
                    CustomerInfoGuid = Guid.NewGuid()
                };

                Order order = new Order()
                {
                    OrderGuid = Guid.NewGuid(),
                    RequestInfo = new OrderRequestInfo()
                    {
                        OrderRequestInfoGuid = Guid.NewGuid(),
                        DropOffAddress = new CivicAddressDb()
                        {
                            CivicAddressGuid = Guid.NewGuid(),
                            StreetNumber = "630 North 14th Street, Kauffman Hall",
                            City = "Lincoln",
                            State = "Nebraska",
                            Country = "USA",
                            ZipCode = "68508",
                            Route = "uh",
                            RawInputAddress = "630 North 14th Street, Kauffman Hall, Lincoln, NE 68508, USA"
                        },

                        PickUpAddress = new CivicAddressDb()
                        {
                            CivicAddressGuid = Guid.NewGuid(),
                            StreetNumber = "3025 North 169th Avenue",
                            City = "Omaha",
                            State = "Nebraska",
                            Country = "USA",
                            ZipCode = "68116",
                            Route = "uh",
                            RawInputAddress = "3025 North 169th Avenue, Omaha, NE 68116, USA"
                        },

                        RequestDate = DateTime.Now,
                        Requester = me.CustomerInfo,
                        Size = new TruckCapacityUnit() { TruckCapacityUnitGuid = Guid.NewGuid(), Mass = 0725, Volume = 3 }
                    },

                    TrackInfo = new OrderTrackInfo()
                    {
                        Assignee = me.ContractorInfo,
                        CurrentTruck = me.ContractorInfo.Truck,
                        OrderStatus = OrderStatus.InProgress,
                        OrderTrackInfoGuid = Guid.NewGuid()
                    }
                };
                _context.Order.Add(order);
                _context.SaveChanges();
            }
        }
    }
}