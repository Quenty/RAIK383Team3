using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PracticalWerewolf.Services.Interfaces;
using System.Collections.Generic;
using PracticalWerewolf.Models.Trucks;
using PracticalWerewolf.Controllers;
using System.Web.Mvc;
using PracticalWerewolf.ViewModels;
using System.Linq;
using System.Data.Entity.Spatial;
using PracticalWerewolf.Controllers.UnitOfWork;
using PracticalWerewolf.Tests.Stores.DbContext;
using PracticalWerewolf.Models;
using PracticalWerewolf.Models.UserInfos;
using PracticalWerewolf.Helpers;
using System.Security.Claims;

namespace PracticalWerewolf.Tests.Controllers
{
    [TestClass]
    public class TruckControllerTest : ControllerTest
    {
        private static TruckCapacityUnit unit = new TruckCapacityUnit { TruckCapacityUnitGuid = Guid.NewGuid() };
        private static DbGeography location = LocationHelper.CreatePoint(3.14, 2.18);

        private static IEnumerable<Truck> _trucks = new List<Truck>
        {
            new Truck {TruckGuid = Guid.NewGuid(), UsedCapacity = unit, MaxCapacity = unit, Location = location, LicenseNumber = "Hergedy1"},
            new Truck {TruckGuid = Guid.NewGuid(), UsedCapacity = unit, MaxCapacity = unit, Location = location, LicenseNumber = "Hergedy2"},
            new Truck {TruckGuid = Guid.NewGuid(), UsedCapacity = unit, MaxCapacity = unit, Location = location, LicenseNumber = "Hergedy3"}
        };

        [TestMethod]
        public void Index_NoTrucks_TestViewModel()
        {
            var email = "Example@example.com";
            var guid = Guid.NewGuid();
            var user = new ApplicationUser() { Id = guid.ToString(), ContractorInfo = new ContractorInfo(), UserName = email };
            var userDbSet = new MockUserDbSet();
            userDbSet.Add(user);

            var truckService = new Mock<ITruckService>();
            var contractorService = new Mock<IContractorService>();
            truckService.Setup(x => x.GetAllTrucks()).Returns(new List<Truck>());
            var unitOfWork = new Mock<IUnitOfWork>();
            var userManager = GetApplicationUserManager(userDbSet);

            var controller = new TruckController(truckService.Object, contractorService.Object, unitOfWork.Object, userManager);
            var principal = GetMockUser(email);
            var context = GetMockControllerContext(principal);
            controller.ControllerContext = context;

            var result = controller.Index() as ViewResult;
            var model = result.Model as TruckIndexViewModel;

            Assert.IsNotNull(model);
            Assert.AreEqual(0, model.Trucks.Count());
        }

        [TestMethod]
        public void Index_ThreeTrucks_TestViewModel()
        {
            var email = "Example@example.com";
            var guid = Guid.NewGuid();
            var user = new ApplicationUser() { Id = guid.ToString(), ContractorInfo = new ContractorInfo() { Truck = new Truck()}, UserName = email, };
            var userDbSet = new MockUserDbSet();
            userDbSet.Add(user);

            var truckService = new Mock<ITruckService>();
            var contractorService = new Mock<IContractorService>();
            truckService.Setup(x => x.GetAllTrucks()).Returns(_trucks);
            var unitOfWork = new Mock<IUnitOfWork>();
            var userManager = GetApplicationUserManager(userDbSet);
            var controller = new TruckController(truckService.Object, contractorService.Object, unitOfWork.Object, userManager);
            var principal = GetMockUser(email);
            var context = GetMockControllerContext(principal);
            controller.ControllerContext = context;

            var result = controller.Index() as ViewResult;
            var model = result.Model as TruckIndexViewModel;

            Assert.IsNotNull(model);
            Assert.AreEqual(3, model.Trucks.Count());
            foreach(var truck in _trucks)
            {
                var truckModel = model.Trucks.Single(x => x.Guid.Equals(truck.TruckGuid));
                Assert.IsNotNull(truckModel);
                Assert.AreEqual(truck.LicenseNumber, truckModel.LicenseNumber);
                Assert.AreEqual(truck.MaxCapacity, truckModel.MaxCapacity);
                Assert.AreEqual(truck.Location.Latitude, truckModel.Lat);
                Assert.AreEqual(truck.Location.Longitude, truckModel.Long);
            }
        }

        [TestMethod]
        public void Index_EmployeeNoTrucks_NoTrucks()
        {
            var email = "Example@example.com";
            var guid = Guid.NewGuid();
            var user = new ApplicationUser() { Id = guid.ToString(), EmployeeInfo = new EmployeeInfo(), UserName = email };
            var userDbSet = new MockUserDbSet();
            userDbSet.Add(user);

            var truckService = new Mock<ITruckService>();
            var contractorService = new Mock<IContractorService>();
            truckService.Setup(x => x.GetAllTrucks()).Returns(new List<Truck>());
            var unitOfWork = new Mock<IUnitOfWork>();
            var userManager = GetApplicationUserManager(userDbSet);

            var controller = new TruckController(truckService.Object, contractorService.Object, unitOfWork.Object, userManager);
            var principal = GetMockUser(email);
            var context = GetMockControllerContext(principal);
            controller.ControllerContext = context;

            var result = controller.Index() as ViewResult;
            var model = result.Model as TruckIndexViewModel;

            Assert.IsNotNull(model);
            Assert.AreEqual(0, model.Trucks.Count());
        }

        [TestMethod]
        public void Index_EmployeeThreeTrucks_ThreeTrucks()
        {
            var email = "Example@example.com";
            var guid = Guid.NewGuid();
            var user = new ApplicationUser() { Id = guid.ToString(), EmployeeInfo = new EmployeeInfo(), UserName = email };
            var userDbSet = new MockUserDbSet();
            userDbSet.Add(user);

            var truckService = new Mock<ITruckService>();
            var contractorService = new Mock<IContractorService>();
            truckService.Setup(x => x.GetAllTrucks()).Returns(_trucks);
            var unitOfWork = new Mock<IUnitOfWork>();
            var userManager = GetApplicationUserManager(userDbSet);
            var controller = new TruckController(truckService.Object, contractorService.Object, unitOfWork.Object, userManager);
            var principal = GetMockUser(email);
            var context = GetMockControllerContext(principal);
            controller.ControllerContext = context;

            var result = controller.Index() as ViewResult;
            var model = result.Model as TruckIndexViewModel;

            Assert.IsNotNull(model);
            Assert.AreEqual(3, model.Trucks.Count());
            foreach (var truck in _trucks)
            {
                var truckModel = model.Trucks.Single(x => x.Guid.Equals(truck.TruckGuid));
                Assert.IsNotNull(truckModel);
                Assert.AreEqual(truck.LicenseNumber, truckModel.LicenseNumber);
                Assert.AreEqual(truck.MaxCapacity, truckModel.MaxCapacity);
                Assert.AreEqual(truck.Location.Latitude, truckModel.Lat);
                Assert.AreEqual(truck.Location.Longitude, truckModel.Long);
            }
        }

        [TestMethod]
        public void Details_OneTruckValidId_ExpectedOutput()
        {
            var location = LocationHelper.CreatePoint(3.14, 2.18);
            var capacity = new TruckCapacityUnit() { Mass = 12, Volume = 12, TruckCapacityUnitGuid = Guid.NewGuid() };
            var guid = Guid.NewGuid();
            var truck = new Truck()
            {
                TruckGuid = guid,
                UsedCapacity = capacity,
                MaxCapacity = capacity,
                LicenseNumber = "James",
                Location = location
            };
            var truckService = new Mock<ITruckService>();
            truckService.Setup(x => x.GetTruck(It.IsAny<Guid>())).Returns(truck);
            var contractorService = new Mock<IContractorService>();
            var unitOfWork = new Mock<IUnitOfWork>();
            var userManager = GetMockApplicationUserManager();
            var controller = new TruckController(truckService.Object, contractorService.Object, unitOfWork.Object, userManager.Object);

            var result = controller.Details(guid.ToString()) as ViewResult;
            var model = result.Model as TruckDetailsViewModel;

            Assert.IsNotNull(model);
            Assert.AreEqual(model.Guid, guid);
            Assert.AreEqual(model.MaxCapacity, capacity);
            Assert.AreEqual(model.Lat, 3.14);
            Assert.AreEqual(model.Long, 2.18);
            Assert.AreEqual("James", model.LicenseNumber);
            Assert.IsNotNull(model.AvailableCapacity);
        }

        [TestMethod]
        public void Details_NoTruck_ReturnsHttpNotFound()
        {
            var truckService = new Mock<ITruckService>();
            truckService.Setup(x => x.GetTruck(It.IsAny<Guid>())).Returns((Truck) null);
            var contractorService = new Mock<IContractorService>();
            var unitOfWork = new Mock<IUnitOfWork>();
            var userManager = GetMockApplicationUserManager();
            var controller = new TruckController(truckService.Object, contractorService.Object, unitOfWork.Object, userManager.Object);

            var result = controller.Details(Guid.Empty.ToString());

            Assert.IsInstanceOfType(result, typeof(ActionResult));
        }

        [TestMethod]
        public void Details_NoInput_ReturnsToView()
        {
            var truckService = new Mock<ITruckService>();
            var contractorService = new Mock<IContractorService>();
            var unitOfWork = new Mock<IUnitOfWork>();
            var userManager = GetMockApplicationUserManager();
            var controller = new TruckController(truckService.Object, contractorService.Object, unitOfWork.Object, userManager.Object);

            var result = controller.Details(String.Empty);
            Assert.IsInstanceOfType(result, typeof(ActionResult));
        }

        [TestMethod]
        public void Edit_OneTruck_ValidOutput()
        {
            var location = LocationHelper.CreatePoint(3.14, 2.18);
            var capacity = new TruckCapacityUnit() { Mass = 12, Volume = 12, TruckCapacityUnitGuid = Guid.NewGuid() };
            var guid = Guid.NewGuid();
            var truck = new Truck()
            {
                TruckGuid = guid,
                UsedCapacity = capacity,
                MaxCapacity = capacity,
                LicenseNumber = "Abbie",
                Location = location
            };
            var truckService = new Mock<ITruckService>();
            truckService.Setup(x => x.GetTruck(It.IsAny<Guid>())).Returns(truck);
            var contractorService = new Mock<IContractorService>();
            var unitOfWork = new Mock<IUnitOfWork>();
            var userManager = GetMockApplicationUserManager();


            var controller = new TruckController(truckService.Object, contractorService.Object, unitOfWork.Object, userManager.Object);
            var principal = GetMockUser("nope@nawp.com", new List<Claim>() { new Claim(ClaimTypes.Role, "Employee") });
            var context = GetMockControllerContext(principal);
            controller.ControllerContext = context;

            var baseResult = controller.Edit(guid.ToString());
            Assert.IsNotNull(baseResult);
            Assert.IsInstanceOfType(baseResult, typeof(ViewResult));

            var result = baseResult as ViewResult;
            Assert.IsNotNull(result);

            var model = result.Model as TruckUpdateViewModel;

            Assert.IsNotNull(model);
            Assert.AreEqual(model.Guid, guid);
            Assert.AreEqual(model.Volume, capacity.Volume);
            Assert.AreEqual(model.Mass, capacity.Mass);
            Assert.AreEqual("Abbie", model.LicenseNumber);
        }


        [TestMethod]
        public void Update_NoTruck_ReturnsHttpNotFound()
        {
            var truckService = new Mock<ITruckService>();
            truckService.Setup(x => x.GetTruck(It.IsAny<Guid>())).Returns((Truck)null);
            var contractorService = new Mock<IContractorService>();
            var unitOfWork = new Mock<IUnitOfWork>();
            var userManager = GetMockApplicationUserManager();
            var controller = new TruckController(truckService.Object, contractorService.Object, unitOfWork.Object, userManager.Object);

            var result = controller.Edit(Guid.Empty.ToString());

            Assert.IsInstanceOfType(result, typeof(ActionResult));
        }

        [TestMethod]
        public void Edit_NoInput_ReturnsHttpNotFound()
        {
            var truckService = new Mock<ITruckService>();
            var contractorService = new Mock<IContractorService>();
            var unitOfWork = new Mock<IUnitOfWork>();
            var userManager = GetMockApplicationUserManager();
            var controller = new TruckController(truckService.Object, contractorService.Object, unitOfWork.Object, userManager.Object);

            var result = controller.Edit(String.Empty);

            Assert.IsInstanceOfType(result, typeof(ActionResult));
        }

        [TestMethod]
        public void EditPostback_NullViewModel_ReturnNullViewModel()
        {
            var truckService = new Mock<ITruckService>();
            var contractorService = new Mock<IContractorService>();
            var unitOfWork = new Mock<IUnitOfWork>();
            var userManager = GetMockApplicationUserManager();
            var controller = new TruckController(truckService.Object, contractorService.Object, unitOfWork.Object, userManager.Object);
            var id = Guid.NewGuid().ToString();
            TruckUpdateViewModel viewModel = null;

            var result = controller.Edit(id, viewModel) as ViewResult;
            var model = result.Model as TruckUpdateViewModel;

            Assert.IsNull(model);
        }


        [TestMethod]
        public void EditPostback_ValidViewModel_ReturnRedirectToIndex()
        {
            var truck = _trucks.ElementAt(0);
            var truckService = new Mock<ITruckService>();
            truckService.Setup(x => x.GetTruck(It.IsAny<Guid>())).Returns(truck);
            var contractorService = new Mock<IContractorService>();
            var unitOfWork = new Mock<IUnitOfWork>();
            var userManager = GetMockApplicationUserManager();
            var controller = new TruckController(truckService.Object, contractorService.Object, unitOfWork.Object, userManager.Object);
            TruckUpdateViewModel viewModel = new TruckUpdateViewModel() { Guid = truck.TruckGuid, Mass = 12, Volume = 13 , LicenseNumber = "123321"};

            var result = controller.Edit(truck.TruckGuid.ToString(), viewModel) as RedirectToRouteResult;

            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.RouteValues["action"]);
        }

        [TestMethod]
        public void Create_Nothing_TestItWorks()
        {
            var truckService = new Mock<ITruckService>();
            var contractorService = new Mock<IContractorService>();
            var unitOfWork = new Mock<IUnitOfWork>();
            var userManager = GetMockApplicationUserManager();
            var controller = new TruckController(truckService.Object, contractorService.Object, unitOfWork.Object, userManager.Object);

            var result = controller.Create() as ViewResult;

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void CreatePostback_ValidTruck_TestItWorks()
        {
            var location = LocationHelper.CreatePoint(3.14, 2.18);
            var capacity = new TruckCapacityUnit() { Mass = 12, Volume = 12, TruckCapacityUnitGuid = Guid.NewGuid() };
            var guid = Guid.NewGuid();

            TruckCreateViewModel createModel = new TruckCreateViewModel()
            {
                Guid = guid,
                LicenseNumber = "Matt",
                Mass = 12,
                Volume = 12,
                Lat = 3.14,
                Long = 2.18
            };
            var truckService = new Mock<ITruckService>();
            var contractorService = new Mock<IContractorService>();
            var unitOfWork = new Mock<IUnitOfWork>();
            var userManager = GetMockApplicationUserManager();
            var controller = new TruckController(truckService.Object, contractorService.Object, unitOfWork.Object, userManager.Object);
            var email = "Example@example.com";
            var principal = GetMockUser(email);
            var context = GetMockControllerContext(principal);
            controller.ControllerContext = context;

            var result = controller.Create(createModel) as RedirectToRouteResult;

            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.RouteValues["action"]);
        }

        [TestMethod]
        public void CreatePostback_NullTruckViewModel_HttpNotFound()
        {
            TruckCreateViewModel truck = null;
            var truckService = new Mock<ITruckService>();
            var contractorService = new Mock<IContractorService>();
            var unitOfWork = new Mock<IUnitOfWork>();
            var userManager = GetMockApplicationUserManager();
            var controller = new TruckController(truckService.Object, contractorService.Object, unitOfWork.Object, userManager.Object);
            var email = "Example@example.com";
            var principal = GetMockUser(email);
            var context = GetMockControllerContext(principal);
            controller.ControllerContext = context;

            var result = controller.Create(truck) as RedirectToRouteResult;

            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.RouteValues["action"]); ;
        }

        [TestMethod]
        public void CreatePostback_InvalidTruckCapacity_HttpNotFound()
        {
            var guid = Guid.NewGuid();
            var truck = new TruckCreateViewModel()
            {
                Guid = guid,
                LicenseNumber = "Jessee",
                Mass = -2,
                Volume = -7
            };
            var truckService = new Mock<ITruckService>();
            var contractorService = new Mock<IContractorService>();
            var unitOfWork = new Mock<IUnitOfWork>();
            var userManager = GetMockApplicationUserManager();
            var controller = new TruckController(truckService.Object, contractorService.Object, unitOfWork.Object, userManager.Object);
            var email = "Example@example.com";
            var principal = GetMockUser(email);
            var context = GetMockControllerContext(principal);
            controller.ControllerContext = context;

            var result = controller.Create(truck) as RedirectToRouteResult;

            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.RouteValues["action"]);
        }

        [TestMethod]
        public void CreatePostback_NullLicense_HttpNotFound()
        {
            var guid = Guid.NewGuid();
            var truck = new TruckCreateViewModel()
            {
                Guid = guid,
                Lat = 1,
                Long = 2,
                Mass = 3,
                Volume = 4
            };
            var truckService = new Mock<ITruckService>();
            var contractorService = new Mock<IContractorService>();
            var unitOfWork = new Mock<IUnitOfWork>();
            var userManager = GetMockApplicationUserManager();
            var controller = new TruckController(truckService.Object, contractorService.Object, unitOfWork.Object, userManager.Object);
            var email = "Example@example.com";
            var principal = GetMockUser(email);
            var context = GetMockControllerContext(principal);
            controller.ControllerContext = context;

            var result = controller.Create(truck) as RedirectToRouteResult;

            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.RouteValues["action"]);
        }
    }
}