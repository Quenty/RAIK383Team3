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

    internal sealed class Configuration : DbMigrationsConfiguration<PracticalWerewolf.Application.ApplicationDbContext>
    {
        public Configuration()
        {
            SqlServerTypes.Utilities.LoadNativeAssemblies(AppDomain.CurrentDomain.BaseDirectory);

            AutomaticMigrationsEnabled = false;
            ContextKey = "PracticalWerewolf.Application.ApplicationDbContext";
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

            TruckCapacityUnit capacity = new TruckCapacityUnit { TruckCapacityUnitGuid = Guid.NewGuid() };
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

            if (!(context.Users.Any(u => u.UserName == "nope@nope.com")))
            {
                var userStore = new UserStore<ApplicationUser>(context);
                var userManager = new UserManager<ApplicationUser>(userStore);
                var userToInsert = new ApplicationUser {
                    UserName = "nope@nope.com",
                    Email = "nope@nope.com",
                    EmployeeInfo = new EmployeeInfo(),
                };
                userManager.Create(userToInsert, "p@ssword!");
            }
        }
    }
}
