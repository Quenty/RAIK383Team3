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
using PracticalWerewolf.Models;

namespace PracticalWerewolf.Tests.Controllers
{
    [TestClass]
    public class TruckControllerTest
    {
        private static TruckCapacityUnit unit = new TruckCapacityUnit { TruckCapacityUnitGuid = Guid.NewGuid() };
        private static DbGeography location = null;

        private static IEnumerable<Truck> _trucks = new List<Truck>
        {
            new Truck {TruckGuid = Guid.NewGuid(), CurrentCapacity = unit, MaxCapacity = unit, Location = location, LicenseNumber = "Hergedy1"},
            new Truck {TruckGuid = Guid.NewGuid(), CurrentCapacity = unit, MaxCapacity = unit, Location = location, LicenseNumber = "Hergedy2"},
            new Truck {TruckGuid = Guid.NewGuid(), CurrentCapacity = unit, MaxCapacity = unit, Location = location, LicenseNumber = "Hergedy3"}
        };

        [TestMethod]
        public void Index_NoTrucks_TestViewModel()
        {
            var truckService = new Mock<ITruckService>();
            var contractorService = new Mock<IContractorService>();
            truckService.Setup(x => x.GetAllTrucks()).Returns(new List<Truck>());
            var context = new Mock<ApplicationDbContext>();
            var controller = new TruckController(truckService.Object, contractorService.Object, context.Object);

            var result = controller.Index() as ViewResult;
            var model = result.Model as TruckIndexViewModel;

            Assert.IsNotNull(model);
            Assert.AreEqual(0, model.Trucks.Count());
        }

        [TestMethod]
        public void Index_ThreeTrucks_TestViewModel()
        {
            var truckService = new Mock<ITruckService>();
            var contractorService = new Mock<IContractorService>();
            truckService.Setup(x => x.GetAllTrucks()).Returns(_trucks);
            var context = new Mock<ApplicationDbContext>();
            var controller = new TruckController(truckService.Object, contractorService.Object, context.Object);

            var result = controller.Index() as ViewResult;
            var model = result.Model as TruckIndexViewModel;

            Assert.IsNotNull(model);
            Assert.AreEqual(3, model.Trucks.Count());
            Assert.IsTrue(model.Trucks.Contains(_trucks.ElementAt(0)));
            Assert.IsTrue(model.Trucks.Contains(_trucks.ElementAt(1)));
            Assert.IsTrue(model.Trucks.Contains(_trucks.ElementAt(2)));
        }

        [TestMethod]
        public void Details_OneTruckValidId_ExpectedOutput()
        {
            var capacity = new TruckCapacityUnit() { Mass = 12, Volume = 12, TruckCapacityUnitGuid = Guid.NewGuid() };
            var guid = Guid.NewGuid();
            var truck = new Truck()
            {
                TruckGuid = guid,
                CurrentCapacity = capacity,
                MaxCapacity = capacity,
                LicenseNumber = "James"
            };
            var truckService = new Mock<ITruckService>();
            truckService.Setup(x => x.GetTruck(It.IsAny<Guid>())).Returns(truck);
            var contractorService = new Mock<IContractorService>();
            var context = new Mock<ApplicationDbContext>();
            var controller = new TruckController(truckService.Object, contractorService.Object, context.Object);

            var result = controller.Details(guid.ToString()) as ViewResult;
            var model = result.Model as TruckDetailsViewModel;

            Assert.IsNotNull(model);
            Assert.AreEqual(model.Guid, guid.ToString());
            Assert.AreEqual(model.MaxCapacity, capacity);
            Assert.AreEqual(model.Lat, 3.14);
            Assert.AreEqual(model.Long, 2.18);
            Assert.IsNotNull(model.AvailableCapacity);
            Assert.AreEqual("James", model.LicenseNumber);
        }

        [TestMethod]
        public void Details_NoTruck_ReturnsHttpNotFound()
        {
            var truckService = new Mock<ITruckService>();
            truckService.Setup(x => x.GetTruck(It.IsAny<Guid>())).Returns((Truck) null);
            var contractorService = new Mock<IContractorService>();
            var context = new Mock<ApplicationDbContext>();
            var controller = new TruckController(truckService.Object, contractorService.Object, context.Object);

            var result = controller.Details(Guid.Empty.ToString());

            Assert.IsInstanceOfType(result, typeof(HttpNotFoundResult));
        }

        [TestMethod]
        public void Details_NoInput_ReturnsHttpNotFound()
        {
            var truckService = new Mock<ITruckService>();
            var contractorService = new Mock<IContractorService>();
            var context = new Mock<ApplicationDbContext>();
            var controller = new TruckController(truckService.Object, contractorService.Object, context.Object);

            var result = controller.Details(String.Empty);

            Assert.IsInstanceOfType(result, typeof(HttpNotFoundResult));
        }

        [TestMethod]
        public void Edit_OneTruck_ValidOutput()
        {
            var capacity = new TruckCapacityUnit() { Mass = 12, Volume = 12, TruckCapacityUnitGuid = Guid.NewGuid() };
            var guid = Guid.NewGuid();
            var truck = new Truck()
            {
                TruckGuid = guid,
                CurrentCapacity = capacity,
                MaxCapacity = capacity,
                LicenseNumber = "Abbie"
            };
            var truckService = new Mock<ITruckService>();
            truckService.Setup(x => x.GetTruck(It.IsAny<Guid>())).Returns(truck);
            var contractorService = new Mock<IContractorService>();
            var context = new Mock<ApplicationDbContext>();
            var controller = new TruckController(truckService.Object, contractorService.Object, context.Object);

            var result = controller.Edit(guid.ToString()) as ViewResult;
            var model = result.Model as TruckUpdateViewModel;

            Assert.IsNotNull(model);
            Assert.AreEqual(model.Guid, guid.ToString());
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
            var context = new Mock<ApplicationDbContext>();
            var controller = new TruckController(truckService.Object, contractorService.Object, context.Object);

            var result = controller.Edit(Guid.Empty.ToString());

            Assert.IsInstanceOfType(result, typeof(HttpNotFoundResult));
        }

        [TestMethod]
        public void Edit_NoInput_ReturnsHttpNotFound()
        {
            var truckService = new Mock<ITruckService>();
            var contractorService = new Mock<IContractorService>();
            var context = new Mock<ApplicationDbContext>();
            var controller = new TruckController(truckService.Object, contractorService.Object, context.Object);

            var result = controller.Edit(String.Empty);

            Assert.IsInstanceOfType(result, typeof(HttpNotFoundResult));
        }

        [TestMethod]
        public void EditPostback_NullViewModel_ReturnNullViewModel()
        {
            var truckService = new Mock<ITruckService>();
            var contractorService = new Mock<IContractorService>();
            var context = new Mock<ApplicationDbContext>();
            var controller = new TruckController(truckService.Object, contractorService.Object, context.Object);
            var id = Guid.NewGuid().ToString();
            TruckUpdateViewModel viewModel = null;

            var result = controller.Edit(id, viewModel) as ViewResult;
            var model = result.Model as TruckUpdateViewModel;

            Assert.IsNull(model);
        }


        [TestMethod]
        public void EditPostback_ValidViewModel_ReturnRedirectToIndex()
        {
            var truckService = new Mock<ITruckService>();
            var contractorService = new Mock<IContractorService>();
            var context = new Mock<ApplicationDbContext>();
            var controller = new TruckController(truckService.Object, contractorService.Object, context.Object);
            var id = Guid.NewGuid().ToString();
            TruckUpdateViewModel viewModel = new TruckUpdateViewModel() { Guid = Guid.NewGuid().ToString(), Mass = 12, Volume = 13 };

            var result = controller.Edit(id, viewModel) as RedirectToRouteResult;

            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.RouteValues["action"]);
        }

        [TestMethod]
        public void Create_Nothing_TestItWorks()
        {
            var truckService = new Mock<ITruckService>();
            var contractorService = new Mock<IContractorService>();
            var context = new Mock<ApplicationDbContext>();
            var controller = new TruckController(truckService.Object, contractorService.Object, context.Object);

            var result = controller.Create() as ViewResult;

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void CreatePostback_ValidTruck_TestItWorks()
        {
            var guid = Guid.NewGuid();

            TruckCreateViewModel createModel = new TruckCreateViewModel()
            {
                Guid = guid.ToString(),
                LicenseNumber = "Matt",
                Mass = 12,
                Volume = 12,
                Lat = 3.14,
                Long = 2.18
            };
            var truckService = new Mock<ITruckService>();
            var contractorService = new Mock<IContractorService>();
            var context = new Mock<ApplicationDbContext>();
            var controller = new TruckController(truckService.Object, contractorService.Object, context.Object);

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
            var context = new Mock<ApplicationDbContext>();
            var controller = new TruckController(truckService.Object, contractorService.Object, context.Object);

            var result = controller.Create(truck);

            Assert.IsInstanceOfType(result, typeof(HttpNotFoundResult));
        }

        [TestMethod]
        public void CreatePostback_NullTruckCapacity_HttpNotFound()
        {
            var guid = Guid.NewGuid();
            var truck = new TruckCreateViewModel()
            {
                Guid = guid.ToString(),
                LicenseNumber = "Jessee"
            };
            var truckService = new Mock<ITruckService>();
            var contractorService = new Mock<IContractorService>();
            var context = new Mock<ApplicationDbContext>();
            var controller = new TruckController(truckService.Object, contractorService.Object, context.Object);

            var result = controller.Create(truck);

            Assert.IsInstanceOfType(result, typeof(HttpNotFoundResult));
        }

        [TestMethod]
        public void CreatePostback_NullLicense_HttpNotFound()
        {
            var guid = Guid.NewGuid();
            var truck = new TruckCreateViewModel()
            {
                Guid = guid.ToString(),
                Lat = 1,
                Long = 2,
                Mass = 3,
                Volume = 4
            };
            var truckService = new Mock<ITruckService>();
            var contractorService = new Mock<IContractorService>();
            var context = new Mock<ApplicationDbContext>();
            var controller = new TruckController(truckService.Object, contractorService.Object, context.Object);

            var result = controller.Create(truck);

            Assert.IsInstanceOfType(result, typeof(HttpNotFoundResult));
        }
    }
}