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

        private static readonly string DEFAULT_PASSWORD = "p@ssword!";

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
                orderRequest,
                orderRequest2,
                orderRequest3,
                orderRequest4
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
            Order order2 = new Order { OrderGuid = Guid.NewGuid(), RequestInfo = orderRequest2, TrackInfo = orderCancelled };
            Order order3 = new Order { OrderGuid = Guid.NewGuid(), RequestInfo = orderRequest3, TrackInfo = orderComplete };
            Order order4 = new Order { OrderGuid = Guid.NewGuid(), RequestInfo = orderRequest4, TrackInfo = orderQueued };
            context.Order.AddOrUpdate(
                order,
                order2,
                order3,
                order4
                );


            var userStore = new UserStore<ApplicationUser>(context);
            var userManager = new UserManager<ApplicationUser>(userStore);

            var emails = new List<String> { "windofdances@example.com", "sorrowskeels@example.com", "placeofsnow@example.com", "waveofadventures@example.com", "anangelsheartflight@example.com", "sundrift@example.com", "sailofships@example.com", "keeldance@example.com", "windwater@example.com", "keelofoflights@example.com", "moondrift@example.com", "springstar@example.com", "mistmoons@example.com", "placeofotterdesire@example.com", "keelsail@example.com", "reefswaves@example.com", "sailshark@example.com", "starseas@example.com", "sharkaway@example.com", "dreamship@example.com", "mistofgold@example.com", "oceanship@example.com", "starssails@example.com", "morningsharkoflight@example.com", "keelbliss@example.com", "keelofwantss@example.com", "seassnow@example.com", "dreamofflights@example.com", "fivesails@example.com", "bayoflight@example.com", "mistofflights@example.com", "shadowofharmonys@example.com", "mistsmoons@example.com", "anangelsailaway@example.com", "moonofflights@example.com", "reefofflights@example.com", "dreamsstar@example.com", "twilightsotter@example.com", "windwater@example.com", "springsail@example.com", "sailship@example.com", "dreamotters@example.com", "sliverflight@example.com", "dreamofharmonys@example.com", "reefofofgolds@example.com", "morningssaphirebliss@example.com", "bayofblisss@example.com", "bayofblisss@example.com", "snowship@example.com", "bayreef@example.com", "autumnshomeadventure@example.com", "firstotterdance@example.com", "snowofdrifts@example.com", "woesail@example.com", "twilightsails@example.com", "akeel@example.com", "oceandesire@example.com", "freemists@example.com", "asaphire@example.com", "thehearts@example.com", "saphirebliss@example.com", "solitudehearts@example.com", "shadowsea@example.com", "sharksaphire@example.com", "snowsun@example.com", "morningsseaofgold@example.com", "sixwinds@example.com", "seadrift@example.com", "oceansun@example.com", "wintersaphirebliss@example.com", "waveswaters@example.com", "moonofofheavens@example.com", "obsessionsailship@example.com", "firstmoons@example.com", "waterotters@example.com", "homewaves@example.com", "bayaway@example.com", "sailbliss@example.com", "winddance@example.com", "twilightsmoondesire@example.com", "starslivers@example.com", "mourningskeelofgold@example.com", "freedomsails@example.com", "otterswater@example.com", "mourningmoons@example.com", "eveningsoceanadventure@example.com", "bayflight@example.com", "mistflight@example.com", "waterwave@example.com", "fastdreamship@example.com", "autumnsuns@example.com", "bayssaphire@example.com", "sliverofdances@example.com", "sorrowssailofgold@example.com", "sharkofgold@example.com", "anangelsdreamofgold@example.com", "sailssaphires@example.com", "obsessionstar@example.com", "mistofofgolds@example.com", "dreamofwantss@example.com", "shadowharmony@example.com", "waveshomes@example.com", "mistsheart@example.com", "winterbay@example.com", "sorrowshomeship@example.com", "reefssnow@example.com", "seabliss@example.com", "starofofgolds@example.com", "reefflight@example.com", "snowofoflights@example.com", "sharkssliver@example.com", "eveningsshadows@example.com", "reefflight@example.com", "starssaphires@example.com", "saphireshadow@example.com", "joyssharkaway@example.com", "waterofdrifts@example.com", "sailssun@example.com", "anotherreefs@example.com", "oceanofadventures@example.com", "silentstarharmony@example.com", "starofblisss@example.com", "watersotters@example.com", "shadowadventure@example.com", "threehearts@example.com", "silentsharkwants@example.com", "summermistofgold@example.com", "daysshadows@example.com", "autumnmistofgold@example.com", "shadowofdrifts@example.com", "sailofgold@example.com", "twilighthomedrift@example.com", "homeshearts@example.com", "anotherbay@example.com", "thewaves@example.com", "eveningstars@example.com", "keeldrift@example.com", "sixoceans@example.com", "afterocean@example.com", "goldenreef@example.com", "keelssail@example.com", "waveswind@example.com", "homeship@example.com", "homewaves@example.com", "keelofofheavens@example.com", "windofofgolds@example.com", "saphiresstar@example.com", "anangelsslivers@example.com", "windwater@example.com", "thereefs@example.com", "sliveradventure@example.com", "heartshomes@example.com", "moonaway@example.com", "shadowofblisss@example.com", "dreamsoceans@example.com", "freedomsoceanoflight@example.com", "sliverofflights@example.com", "snowofblisss@example.com", "moonmist@example.com", "melancholyssailadventure@example.com", "sorrowbays@example.com", "winterkeels@example.com", "sixotters@example.com", "snowssaphire@example.com", "freedomsails@example.com", "obsessionswind@example.com", "ottersstars@example.com", "staroflight@example.com", "freedomseaflight@example.com", "keelflight@example.com", "aftersailofgold@example.com", "solitudesstars@example.com", "seadesire@example.com", "homeofdrifts@example.com", "homeofheaven@example.com", "reefofgold@example.com", "reefsmist@example.com", "windofoflights@example.com", "starshadow@example.com", "springssnows@example.com", "waveofdesires@example.com", "purplesail@example.com", "waveshome@example.com", "joysheart@example.com", "sharkofoflights@example.com", "afterhomeaway@example.com", "winterssailflight@example.com", "bittersweetsliver@example.com", "mourningsharkaway@example.com", "afterreef@example.com", "seasail@example.com", "waterofgold@example.com", "sliversstar@example.com", "starsun@example.com", "bittersweetheartdesire@example.com", "freehome@example.com", "sliverssharks@example.com", "summersunofgold@example.com", "twilightswindofheaven@example.com", "sorrowsailaway@example.com" };

            Random random = new Random(("TheWorstIsYetToCome").GetHashCode());
            foreach (var email in emails)
            {
                var user = MakeBaseUserData(email);
                if (random.NextDouble() > 0.25)
                {
                    user.EmployeeInfo = null;
                }

                MakeUser(userStore, userManager, user, DEFAULT_PASSWORD);
            }

            MakeUser(userStore, userManager, MakeBaseUserData("nope@nope.com"), DEFAULT_PASSWORD);


            {
                CivicAddressDb addresss = new CivicAddressDb()
                {
                    Route = "3025",
                    State = "NE",
                    ZipCode = "68116",
                    StreetNumber = "North 169th Avenue",
                    City = "Omaha",
                    Country = "USA",
                    CivicAddressGuid = Guid.NewGuid(),
                    RawInputAddress = "3025 North 169th Avenue, Omaha, NE 68116"
                };

                var user = MakeBaseUserData("darkness@darkknight.com");
                user.EmployeeInfo = null;
                user.ContractorInfo = new ContractorInfo
                {
                    ApprovalState = ContractorApprovalState.Approved,
                    ContractorInfoGuid = Guid.NewGuid(),
                    DriversLicenseId = "WAYNE",
                    HomeAddress = address,
                    IsAvailable = true,
                    Truck = truck
                };

                MakeUser(userStore, userManager, user, DEFAULT_PASSWORD);
            }

            {
                CivicAddressDb addresss = new CivicAddressDb()
                {
                    Route = "3025",
                    State = "NE",
                    ZipCode = "68116",
                    StreetNumber = "North 169th Avenue",
                    City = "Omaha",
                    Country = "USA",
                    CivicAddressGuid = Guid.NewGuid(),
                    RawInputAddress = "3025 North 169th Avenue, Omaha, NE 68116"
                };

                var user = MakeBaseUserData("darkness_edgy@darkknight.com");
                user.EmployeeInfo = null;
                user.ContractorInfo = new ContractorInfo
                {
                    ApprovalState = ContractorApprovalState.Pending,
                    ContractorInfoGuid = Guid.NewGuid(),
                    DriversLicenseId = "BRUCE",
                    HomeAddress = address,
                    IsAvailable = false,
                    Truck = truck2
                };

                MakeUser(userStore, userManager, user, DEFAULT_PASSWORD);
            }

            {
                CivicAddressDb addresss = new CivicAddressDb()
                {
                    Route = "3025",
                    State = "NE",
                    ZipCode = "68116",
                    StreetNumber = "North 169th Avenue",
                    City = "Omaha",
                    Country = "USA",
                    CivicAddressGuid = Guid.NewGuid(),
                    RawInputAddress = "3025 North 169th Avenue, Omaha, NE 68116"
                };

                var user = MakeBaseUserData("xXx_DARKNESS_xXx@darkknight.com");
                user.EmployeeInfo = null;
                user.ContractorInfo = new ContractorInfo
                {
                    ApprovalState = ContractorApprovalState.Denied,
                    ContractorInfoGuid = Guid.NewGuid(),
                    DriversLicenseId = "BATMAN",
                    HomeAddress = address,
                    IsAvailable = false,
                    Truck = truck3
                };

                MakeUser(userStore, userManager, user, DEFAULT_PASSWORD);
            }
        }
    }
}
