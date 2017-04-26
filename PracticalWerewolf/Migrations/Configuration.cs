namespace PracticalWerewolf.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Models.Orders;
    using Models.UserInfos;
    using Models.Trucks;
    using System.Device.Location;
    using System.Data.Entity.Spatial;
    using Models;
    using Microsoft.AspNet.Identity;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    internal sealed class Configuration : DbMigrationsConfiguration<PracticalWerewolf.Application.ApplicationDbContext>
    {
        public Configuration()
        {
            SqlServerTypes.Utilities.LoadNativeAssemblies(AppDomain.CurrentDomain.BaseDirectory);

            AutomaticMigrationsEnabled = false;
            ContextKey = "PracticalWerewolf.Application.ApplicationDbContext";
        }

        private void MakeUser(UserStore<ApplicationUser> context, UserManager<ApplicationUser> userManager, ApplicationUser baseUser, string password)
        {
            if (!(context.Users.Any(u => u.UserName == baseUser.UserName)))
            {
                userManager.Create(baseUser, password);
            }
        }

        private CivicAddressDb generateCivicAddressDbFromString(string rawAddress)
        {
            string pattern = @"([^,]+)";
            Match m = Regex.Match(rawAddress, pattern);
            string streetAddress = m.Groups[1].Value;
            int number = streetAddress.IndexOf(' ');
            string streetNumber = streetAddress.Substring(0, number - 1);
            string streetName = streetAddress.Substring(number + 1);
            string city = m.Groups[2].Value;
            string state = m.Groups[3].Value;
            string zip = m.Groups[4].Value;

            return new CivicAddressDb
            {
                City = city,
                CivicAddressGuid = Guid.NewGuid(),
                Country = "US",
                RawInputAddress = rawAddress,
                State = state,
                StreetNumber = streetNumber,
                Route = streetName,
                ZipCode = zip
            };
        }

        private ApplicationUser MakeBaseUserData(string email)
        {
            ApplicationUser user = new ApplicationUser
            {
                UserName = email,
                Email = email,
                EmployeeInfo = new EmployeeInfo()
                {
                    EmployeeInfoGuid = Guid.NewGuid()
                },
                CustomerInfo = new CustomerInfo()
                {
                    CustomerInfoGuid = Guid.NewGuid()
                }
            };
            return user;
        }

        protected override void Seed(PracticalWerewolf.Application.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
            UserInfo employeeUser = new UserInfo { UserInfoGuid = Guid.NewGuid(), FirstName = "Bruce", LastName = "Wayne" };
            UserInfo contractorUser = new UserInfo { UserInfoGuid = Guid.NewGuid(), FirstName = "Eample", LastName = "Two" };
            UserInfo customerUser = new UserInfo { UserInfoGuid = Guid.NewGuid(), FirstName = "Person", LastName = "Three" };
            
            context.UserInfo.AddOrUpdate(
                employeeUser,
                contractorUser,
                customerUser
                );

            EmployeeInfo employee = new EmployeeInfo { EmployeeInfoGuid = Guid.NewGuid() };
            context.EmployeeInfo.AddOrUpdate(
               employee
                );

            CustomerInfo customer = new CustomerInfo { CustomerInfoGuid = Guid.NewGuid() };
            context.CustomerInfo.AddOrUpdate(
                customer
                );

            TruckCapacityUnit capacity = new TruckCapacityUnit { TruckCapacityUnitGuid = Guid.NewGuid() , Mass = 9999, Volume = 9999};
            TruckCapacityUnit used = new TruckCapacityUnit { TruckCapacityUnitGuid = Guid.NewGuid() };
            context.TruckCapacityUnit.AddOrUpdate(
                capacity,
                used
            );

            DbGeography location = DbGeography.FromText("POINT(15 15)");
            Truck truck = new Truck { TruckGuid = Guid.NewGuid(), LicenseNumber = "ABC123", Location = location, MaxCapacity = capacity, UsedCapacity = used};
            Truck truck2 = new Truck { TruckGuid = Guid.NewGuid(), LicenseNumber = "ABC123", Location = location, MaxCapacity = capacity, UsedCapacity = used };
            Truck truck3 = new Truck { TruckGuid = Guid.NewGuid(), LicenseNumber = "ABC123", Location = location, MaxCapacity = capacity, UsedCapacity = used };
            context.Truck.AddOrUpdate(
                truck,
                truck2,
                truck3
            );

            CivicAddressDb address = new CivicAddressDb { CivicAddressGuid = Guid.NewGuid(), StreetNumber = "640 N 14th St", Route = "Kauffman", City = "Lincoln", State = "NE", Country = "USA", RawInputAddress = "640 N 14h St. Lincoln NE 68508", ZipCode = "68508" };
            CivicAddressDb address2 = new CivicAddressDb { CivicAddressGuid = Guid.NewGuid(), StreetNumber = "641 N 14th St", Route = "Kauffman", City = "Lincoln", State = "NE", Country = "USA", RawInputAddress = "640 N 14h St. Lincoln NE 68508", ZipCode = "68508" };

            ContractorInfo approvedContractor = new ContractorInfo { ContractorInfoGuid = Guid.NewGuid(), ApprovalState = ContractorApprovalState.Approved, DriversLicenseId = "abc123", HomeAddress = address, IsAvailable = true, Truck = truck};
            ContractorInfo pendingContractor = new ContractorInfo { ContractorInfoGuid = Guid.NewGuid(), ApprovalState = ContractorApprovalState.Pending, DriversLicenseId = "abc1234", HomeAddress = address2, IsAvailable = false, Truck = truck2};
            ContractorInfo deniedContractor = new ContractorInfo { ContractorInfoGuid = Guid.NewGuid(), ApprovalState = ContractorApprovalState.Denied, DriversLicenseId = "bc1234", HomeAddress = address2, IsAvailable = false, Truck = truck3};

            context.ContractorInfo.AddOrUpdate(
                approvedContractor,
                pendingContractor,
                deniedContractor
            );

            OrderRequestInfo orderRequest = new OrderRequestInfo { OrderRequestInfoGuid = Guid.NewGuid(), Requester = customer, Size = used, RequestDate = DateTime.Now, DropOffAddress = address, PickUpAddress = address2};
            OrderRequestInfo orderRequest2 = new OrderRequestInfo { OrderRequestInfoGuid = Guid.NewGuid(), Requester = customer, Size = used, RequestDate = DateTime.Now, DropOffAddress = address, PickUpAddress = address2};
            OrderRequestInfo orderRequest3 = new OrderRequestInfo { OrderRequestInfoGuid = Guid.NewGuid(), Requester = customer, Size = used, RequestDate = DateTime.Now, DropOffAddress = address, PickUpAddress = address2};
            OrderRequestInfo orderRequest4 = new OrderRequestInfo { OrderRequestInfoGuid = Guid.NewGuid(), Requester = customer, Size = used, RequestDate = DateTime.Now, DropOffAddress = address, PickUpAddress = address2};
            context.OrderRequestInfo.AddOrUpdate(
                orderRequest
                );

            OrderTrackInfo orderInProgress = new OrderTrackInfo { OrderTrackInfoGuid = Guid.NewGuid(), Assignee = approvedContractor, CurrentTruck = truck, OrderStatus = OrderStatus.InProgress};
            OrderTrackInfo orderCancelled = new OrderTrackInfo { OrderTrackInfoGuid = Guid.NewGuid(), Assignee = approvedContractor, CurrentTruck = truck, OrderStatus = OrderStatus.Cancelled};
            OrderTrackInfo orderComplete = new OrderTrackInfo { OrderTrackInfoGuid = Guid.NewGuid(), Assignee = approvedContractor, CurrentTruck = truck, OrderStatus = OrderStatus.Complete};
            OrderTrackInfo orderQueued = new OrderTrackInfo { OrderTrackInfoGuid = Guid.NewGuid(), Assignee = approvedContractor, CurrentTruck = truck, OrderStatus = OrderStatus.Queued};
            context.OrderTrackInfo.AddOrUpdate(
                    orderInProgress,
                    orderCancelled,
                    orderComplete,
                    orderQueued
                );

            Order order = new Order { OrderGuid = Guid.NewGuid(), RequestInfo = orderRequest, TrackInfo = orderInProgress};
            context.Order.AddOrUpdate(
                order
                );


            List<String> rawAddresses = new List<string> { "402 E Collamer Dr, Carson, CA, 90746", "6003 O Ave, Santa Fe, TX, 77510", "1346 Elder Dr, Aurora, IL, 60506", "24699 Nobottom Olmsted Twp Rd, Olmsted Falls, OH, 44138", "1510 Flowing Springs Rd #9, Charles Town, WV, 25414", "188 Inverness Dr W, Englewood, CO, 80112", "161 Newington Rd, Greenland, NH, 03840", "12069 Jefferson Blvd, Culver City, CA, 90230", "1946 W Front St, Burlington, NC, 27215", "1421 Alamo St #B, Las Cruces, NM, 88001", "241 Middle Country Rd, Coram, NY, 11727-4414", "12600 Welch Rd, Dallas, TX, 75244-6950", "12700 Welch Rd, Dallas, TX, 75244-6950", "1 Old Sturbridge Village Rd, Sturbridge, MA, 01566-1138", "1307 W 6th St, Corona, CA, 92882-3168", "953 Woodland Ave Se, Atlanta, GA, 30316-2521", "720 3rd St Nw, Valley City, ND, 58072-2708", "1206 Baxter Rd, Bremen, GA, 30110-3949", "1306 Baxter Rd, Bremen, GA, 30110-3949", "11032 Waddell Creek Rd Sw, Olympia, WA, 98512-9341", "1441 Sweetbriar Cir, Odessa, TX, 79761-3429", "1541 Sweetbriar Cir, Odessa, TX, 79761-3429", "W1781 Washington Rd, Oconomowoc, WI, 53066-9561", "1974 Buck Daniels Rd, Culleoka, TN 38451-0000", "2602 W Illinois Ave, Dallas, TX 75233-1002", "107 Old Prestwick Ct, Prattville, AL 36066-5677", "39 Mesa St, San Francisco, CA 94129-3308", "419 W Gentry Pkwy, Tyler, TX 75702-4409", "540 Indian Home Rd, Danville, CA 94526-4308", "305 W Downie St, Alma, MI 48801-1622", "4521 Hiqhwoods Parkway, Glen Allen, VA 23060-0000", "124 S 5th St, Vandalia, IL 62471-2702", "1601 Libal St, Green Bay, WI 54301-2446", "3712 Nw 84th Dr, Gainesville, FL 32606-5663", "4819 Emperor Blvd 4th Floor, Durham, NC 27703-5420", "1950 North Arizona Avenue, Chandler, AZ 85225-7088", "406 S Main St, Hinesville, GA 31313-3258", "820 Frank Rd 1, W Chester, PA 19380-1969", "218 E 39th St, Hibbing, MN 55746-3110", "3095 Highlands Rd, Franklin, NC 28734-3512", "2101 Mountain Ave, Santa Barbara, CA 93101-4615", "4403 Clyde Dr, Jacksonville, FL 32208-1964", "1711 S 11th St, Sheboygan, WI 53081-5810" };
            List<CivicAddressDb> civicAddresses = new List<CivicAddressDb>();
            List<OrderRequestInfo> orderRequests = new List<OrderRequestInfo>();
            List<OrderTrackInfo> orderTracks = new List<OrderTrackInfo>();
            List<Order> orders = new List<Order>();
            foreach (String rawaddress in rawAddresses)
            {
                civicAddresses.Add(generateCivicAddressDbFromString(rawaddress));
            }

            int index = 0;
            foreach (CivicAddressDb civicAddress in civicAddresses)
            {
                index++;
                TruckCapacityUnit orderSize = new TruckCapacityUnit { Mass = 500 / (index + 2), TruckCapacityUnitGuid = Guid.NewGuid(), Volume = 500 / (index + 2) };
                orderRequest = new OrderRequestInfo { Size = orderSize, DropOffAddress = civicAddress, OrderRequestInfoGuid = Guid.NewGuid(), PickUpAddress = civicAddresses.ElementAt(index / 2 + 1), RequestDate = DateTime.Now, Requester = customer };
                context.OrderRequestInfo.AddOrUpdate(orderRequest);
                OrderTrackInfo orderTrack = new OrderTrackInfo();
                order = new Order { OrderGuid = Guid.NewGuid(), RequestInfo = orderRequest, TrackInfo = orderTrack };
                context.CivicAddressDb.AddOrUpdate(civicAddress);
                context.TruckCapacityUnit.AddOrUpdate(orderSize);
                context.OrderTrackInfo.AddOrUpdate(orderTrack);
                context.Order.AddOrUpdate(order);
            }
            

        var userStore = new UserStore<ApplicationUser>(context);
            var userManager = new UserManager<ApplicationUser>(userStore);

            var emails = new List<String> { "windofdances@example.com", "sorrowskeels@example.com", "placeofsnow@example.com", "waveofadventures@example.com", "anangelsheartflight@example.com", "sundrift@example.com", "sailofships@example.com", "keeldance@example.com", "windwater@example.com", "keelofoflights@example.com", "moondrift@example.com", "springstar@example.com", "mistmoons@example.com", "placeofotterdesire@example.com", "keelsail@example.com", "reefswaves@example.com", "sailshark@example.com", "starseas@example.com", "sharkaway@example.com", "dreamship@example.com", "mistofgold@example.com", "oceanship@example.com", "starssails@example.com", "morningsharkoflight@example.com", "keelbliss@example.com", "keelofwantss@example.com", "seassnow@example.com", "dreamofflights@example.com", "fivesails@example.com", "bayoflight@example.com", "mistofflights@example.com", "shadowofharmonys@example.com", "mistsmoons@example.com", "anangelsailaway@example.com", "moonofflights@example.com", "reefofflights@example.com", "dreamsstar@example.com", "twilightsotter@example.com", "windwater@example.com", "springsail@example.com", "sailship@example.com", "dreamotters@example.com", "sliverflight@example.com", "dreamofharmonys@example.com", "reefofofgolds@example.com", "morningssaphirebliss@example.com", "bayofblisss@example.com", "bayofblisss@example.com", "snowship@example.com", "bayreef@example.com", "autumnshomeadventure@example.com", "firstotterdance@example.com", "snowofdrifts@example.com", "woesail@example.com", "twilightsails@example.com", "akeel@example.com", "oceandesire@example.com", "freemists@example.com", "asaphire@example.com", "thehearts@example.com", "saphirebliss@example.com", "solitudehearts@example.com", "shadowsea@example.com", "sharksaphire@example.com", "snowsun@example.com", "morningsseaofgold@example.com", "sixwinds@example.com", "seadrift@example.com", "oceansun@example.com", "wintersaphirebliss@example.com", "waveswaters@example.com", "moonofofheavens@example.com", "obsessionsailship@example.com", "firstmoons@example.com", "waterotters@example.com", "homewaves@example.com", "bayaway@example.com", "sailbliss@example.com", "winddance@example.com", "twilightsmoondesire@example.com", "starslivers@example.com", "mourningskeelofgold@example.com", "freedomsails@example.com", "otterswater@example.com", "mourningmoons@example.com", "eveningsoceanadventure@example.com", "bayflight@example.com", "mistflight@example.com", "waterwave@example.com", "fastdreamship@example.com", "autumnsuns@example.com", "bayssaphire@example.com", "sliverofdances@example.com", "sorrowssailofgold@example.com", "sharkofgold@example.com", "anangelsdreamofgold@example.com", "sailssaphires@example.com", "obsessionstar@example.com", "mistofofgolds@example.com", "dreamofwantss@example.com", "shadowharmony@example.com", "waveshomes@example.com", "mistsheart@example.com", "winterbay@example.com", "sorrowshomeship@example.com", "reefssnow@example.com", "seabliss@example.com", "starofofgolds@example.com", "reefflight@example.com", "snowofoflights@example.com", "sharkssliver@example.com", "eveningsshadows@example.com", "reefflight@example.com", "starssaphires@example.com", "saphireshadow@example.com", "joyssharkaway@example.com", "waterofdrifts@example.com", "sailssun@example.com", "anotherreefs@example.com", "oceanofadventures@example.com", "silentstarharmony@example.com", "starofblisss@example.com", "watersotters@example.com", "shadowadventure@example.com", "threehearts@example.com", "silentsharkwants@example.com", "summermistofgold@example.com", "daysshadows@example.com", "autumnmistofgold@example.com", "shadowofdrifts@example.com", "sailofgold@example.com", "twilighthomedrift@example.com", "homeshearts@example.com", "anotherbay@example.com", "thewaves@example.com", "eveningstars@example.com", "keeldrift@example.com", "sixoceans@example.com", "afterocean@example.com", "goldenreef@example.com", "keelssail@example.com", "waveswind@example.com", "homeship@example.com", "homewaves@example.com", "keelofofheavens@example.com", "windofofgolds@example.com", "saphiresstar@example.com", "anangelsslivers@example.com", "windwater@example.com", "thereefs@example.com", "sliveradventure@example.com", "heartshomes@example.com", "moonaway@example.com", "shadowofblisss@example.com", "dreamsoceans@example.com", "freedomsoceanoflight@example.com", "sliverofflights@example.com", "snowofblisss@example.com", "moonmist@example.com", "melancholyssailadventure@example.com", "sorrowbays@example.com", "winterkeels@example.com", "sixotters@example.com", "snowssaphire@example.com", "freedomsails@example.com", "obsessionswind@example.com", "ottersstars@example.com", "staroflight@example.com", "freedomseaflight@example.com", "keelflight@example.com", "aftersailofgold@example.com", "solitudesstars@example.com", "seadesire@example.com", "homeofdrifts@example.com", "homeofheaven@example.com", "reefofgold@example.com", "reefsmist@example.com", "windofoflights@example.com", "starshadow@example.com", "springssnows@example.com", "waveofdesires@example.com", "purplesail@example.com", "waveshome@example.com", "joysheart@example.com", "sharkofoflights@example.com", "afterhomeaway@example.com", "winterssailflight@example.com", "bittersweetsliver@example.com", "mourningsharkaway@example.com", "afterreef@example.com", "seasail@example.com", "waterofgold@example.com", "sliversstar@example.com", "starsun@example.com", "bittersweetheartdesire@example.com", "freehome@example.com", "sliverssharks@example.com", "summersunofgold@example.com", "twilightswindofheaven@example.com", "sorrowsailaway@example.com" };
            foreach (var email in emails)
            {
                MakeUser(userStore, userManager, MakeBaseUserData(email), "p@ssword!");
            }

            MakeUser(userStore, userManager, MakeBaseUserData("nope@nope.com"), "p@ssword!");
        }
    }

}
