using PracticalWerewolf.Helpers;
using PracticalWerewolf.Models;
using PracticalWerewolf.Models.Orders;
using PracticalWerewolf.Models.Trucks;
using PracticalWerewolf.Models.UserInfos;
using System;
using System.Device.Location;
using System.Linq;

namespace PracticalWerewolf.Tests.Services
{
    public class ServiceTestUtils
    {

        private static Random random = new Random();

        public static ApplicationUser CreateUser(string email)
        {
            var license = RandomString(8);
            var address = CreateRandomAddress();
            var truck = CreateRandomTruck();

            var user = new ApplicationUser
            {
                Id = email,
                UserName = email,
                Email = email,
                ContractorInfo = new ContractorInfo
                {
                    ContractorInfoGuid = Guid.NewGuid(),
                    DriversLicenseId = license,
                    HomeAddress = address,
                    ApprovalState = ContractorApprovalState.Approved,
                    IsAvailable = true,
                    Truck = truck
                },
                CustomerInfo = new CustomerInfo
                {
                    CustomerInfoGuid = Guid.NewGuid()
                }
            };

            return user;
        }

        public static Order CreateRandomOrder(ApplicationUser user, OrderStatus status)
        {
            return new Order
            {
                OrderGuid = Guid.NewGuid(),
                RequestInfo = new OrderRequestInfo
                {
                    OrderRequestInfoGuid = Guid.NewGuid(),
                    RequestDate = DateTime.Now,
                    Requester = user.CustomerInfo,
                    DropOffAddress = CreateRandomAddress(),
                    PickUpAddress = CreateRandomAddress(),
                    Size = new TruckCapacityUnit() { TruckCapacityUnitGuid = Guid.NewGuid(), Mass = random.Next(), Volume = random.Next() }
                },
                TrackInfo = new OrderTrackInfo
                {
                    Assignee = user.ContractorInfo,
                    CurrentTruck = user.ContractorInfo.Truck,
                    OrderStatus = status,
                    OrderTrackInfoGuid = Guid.NewGuid()
                }
            };
        }

        public static CivicAddressDb CreateRandomAddress()
        {
            return new CivicAddressDb
            {
                CivicAddressGuid = Guid.NewGuid(),
                City = RandomString(8),
                State = RandomString(8),
                Country = RandomString(8),
                StreetNumber = RandomString(8),
                ZipCode = RandomString(8),
                RawInputAddress = RandomString(25),
                Route = RandomString(8)
            };
        }

        public static Order CreateOrder(ApplicationUser user, OrderStatus status, CivicAddressDb pickUpAddress, CivicAddressDb dropOffAddress, TruckCapacityUnit size)
        {
            return new Order
            {
                OrderGuid = Guid.NewGuid(),
                RequestInfo = new OrderRequestInfo
                {
                    OrderRequestInfoGuid = Guid.NewGuid(),
                    RequestDate = DateTime.Now,
                    Requester = user.CustomerInfo,
                    DropOffAddress = dropOffAddress,
                    PickUpAddress = pickUpAddress,
                    Size = size
                },
                TrackInfo = new OrderTrackInfo
                {
                    OrderStatus = status,
                    OrderTrackInfoGuid = Guid.NewGuid()
                }
            };
        }

        public static Truck CreateRandomTruck()
        {
            return new Truck()
            {
                LicenseNumber = RandomString(8),
                Location = LocationHelper.CreatePoint(random.Next(-90, 90), random.Next(-90, 90)),
                MaxCapacity = new TruckCapacityUnit() { TruckCapacityUnitGuid = Guid.NewGuid(), Mass = random.Next(), Volume = random.Next() },
                UsedCapacity = new TruckCapacityUnit() { TruckCapacityUnitGuid = Guid.NewGuid(), Mass = random.Next(), Volume = random.Next() },
                TruckGuid = Guid.NewGuid()
            };
        }

        private static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
