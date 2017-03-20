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

    internal sealed class Configuration : DbMigrationsConfiguration<PracticalWerewolf.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "PracticalWerewolf.Models.ApplicationDbContext";
        }

        protected override void Seed(PracticalWerewolf.Models.ApplicationDbContext context)
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

            if (!context.Roles.Any())
            {
                context.Roles.AddOrUpdate(
                        r => r.Name,
                        new IdentityRole { Name = "Employee" },
                        new IdentityRole { Name = "Customer" },
                        new IdentityRole { Name = "Contractor" }
                    );
            }

            UserInfo user = new UserInfo { UserInfoGuid = Guid.NewGuid(), FirstName = "Bruce", LastName = "Wayne" };
            context.UserInfo.AddOrUpdate(
                user
                );

            EmployeeInfo employee = new EmployeeInfo { EmployeeInfoGuid = Guid.NewGuid() };
            context.EmployeeInfo.AddOrUpdate(
               employee
                );

            CustomerInfo customer = new CustomerInfo { CustomerInfoGuid = Guid.NewGuid() };
            context.CustomerInfo.AddOrUpdate(
                customer
                );

            TruckCapacityUnit truckCapacityUnit = new TruckCapacityUnit { TruckCapacityUnitGuid = Guid.NewGuid() };
            TruckCapacityUnit truckCapacityUnit2 = new TruckCapacityUnit { TruckCapacityUnitGuid = Guid.NewGuid() };
            context.TruckCapacityUnit.AddOrUpdate(
                truckCapacityUnit,
                truckCapacityUnit2
            );

            DbGeography location = DbGeography.FromText("POINT(15 15)");
            Truck truck = new Truck { TruckGuid = Guid.NewGuid(), LicenseNumber = "ABC123", Location = location, MaxCapacity = truckCapacityUnit, UsedCapacity = truckCapacityUnit2 };
            context.Truck.AddOrUpdate(
                truck
                );



            CivicAddressDb address = new CivicAddressDb { CivicAddressGuid = Guid.NewGuid(), StreetNumber = "640 N 14th St", Route = "Kauffman", City = "Lincoln", State = "NE", Country = "USA", RawInputAddress = "640 N 14h St. Lincoln NE 68508", ZipCode = "68508" };
            ContractorInfo contractor = new ContractorInfo { ContractorInfoGuid = Guid.NewGuid(), ApprovalState = ContractorApprovalState.Approved, DriversLicenseId = "abc123", HomeAddress = address, IsAvailable = true, Truck = truck};

            context.ContractorInfo.AddOrUpdate(
                contractor
            );

            OrderRequestInfo orderRequest = new OrderRequestInfo { OrderRequestInfoGuid = Guid.NewGuid(), Requester = customer, Size = truckCapacityUnit, RequestDate = DateTime.Now, DropOffAddress = address, PickUpAddress = address };
            context.OrderRequestInfo.AddOrUpdate(
                orderRequest
                );

            OrderTrackInfo orderTrack = new OrderTrackInfo { OrderTrackInfoGuid = Guid.NewGuid(), Assignee = contractor, CurrentTruck = truck, OrderStatus = OrderStatus.InProgress };
            context.OrderTrackInfo.AddOrUpdate(
                    orderTrack
                );

            Order order = new Order { OrderGuid = Guid.NewGuid(), RequestInfo = orderRequest, TrackInfo = orderTrack };
            context.Order.AddOrUpdate(
                order
                );
        }
    }
}
