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
            var controller = new TruckController(truckService.Object, contractorService.Object);

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
            var controller = new TruckController(truckService.Object, contractorService.Object);

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
            var controller = new TruckController(truckService.Object, contractorService.Object);

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
            var controller = new TruckController(truckService.Object, contractorService.Object);

            var result = controller.Details(Guid.Empty.ToString());

            Assert.IsInstanceOfType(result, typeof(HttpNotFoundResult));
        }

        [TestMethod]
        public void Details_NoInput_ReturnsHttpNotFound()
        {
            var truckService = new Mock<ITruckService>();
            var contractorService = new Mock<IContractorService>();
            var controller = new TruckController(truckService.Object, contractorService.Object);

            var result = controller.Details(String.Empty);

            Assert.IsInstanceOfType(result, typeof(HttpNotFoundResult));
        }

        [TestMethod]
        public void Update_OneTruck_ValidOutput()
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
            var controller = new TruckController(truckService.Object, contractorService.Object);

            var result = controller.Update(guid.ToString()) as ViewResult;
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
            var controller = new TruckController(truckService.Object, contractorService.Object);

            var result = controller.Update(Guid.Empty.ToString());

            Assert.IsInstanceOfType(result, typeof(HttpNotFoundResult));
        }

        [TestMethod]
        public void Update_NoInput_ReturnsHttpNotFound()
        {
            var truckService = new Mock<ITruckService>();
            var contractorService = new Mock<IContractorService>();
            var controller = new TruckController(truckService.Object, contractorService.Object);

            var result = controller.Update(String.Empty);

            Assert.IsInstanceOfType(result, typeof(HttpNotFoundResult));
        }

        [TestMethod]
        public void UpdatePostback_NullViewModel_ReturnNullViewModel()
        {
            var truckService = new Mock<ITruckService>();
            var contractorService = new Mock<IContractorService>();
            var controller = new TruckController(truckService.Object, contractorService.Object);
            var id = Guid.NewGuid().ToString();
            TruckUpdateViewModel viewModel = null;

            var result = controller.Update(id, viewModel) as ViewResult;
            var model = result.Model as TruckUpdateViewModel;

            Assert.IsNull(model);
        }


        [TestMethod]
        public void UpdatePostback_ValidViewModel_ReturnRedirectToIndex()
        {
            var truckService = new Mock<ITruckService>();
            var contractorService = new Mock<IContractorService>();
            var controller = new TruckController(truckService.Object, contractorService.Object);
            var id = Guid.NewGuid().ToString();
            TruckUpdateViewModel viewModel = new TruckUpdateViewModel() { Guid = Guid.NewGuid().ToString(), Mass = 12, Volume = 13 };

            var result = controller.Update(id, viewModel) as RedirectToRouteResult;

            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.RouteValues["action"]);
        }

        [TestMethod]
        public void Create_Nothing_TestItWorks()
        {
            var truckService = new Mock<ITruckService>();
            var contractorService = new Mock<IContractorService>();
            var controller = new TruckController(truckService.Object, contractorService.Object);

            var result = controller.Create() as ViewResult;

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void CreatePostback_ValidTruck_TestItWorks()
        {
            var capacity = new TruckCapacityUnit() { Mass = 12, Volume = 12, TruckCapacityUnitGuid = Guid.NewGuid() };
            var guid = Guid.NewGuid();
            var truck = new Truck()
            {
                TruckGuid = guid,
                CurrentCapacity = capacity,
                MaxCapacity = capacity,
                LicenseNumber = "Matt"
            };
            var truckService = new Mock<ITruckService>();
            var contractorService = new Mock<IContractorService>();
            var controller = new TruckController(truckService.Object, contractorService.Object);

            var result = controller.Create(truck) as RedirectToRouteResult;

            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.RouteValues["action"]);
        }

        [TestMethod]
        public void CreatePostback_NullTruck_HttpNotFound()
        {
            Truck truck = null;
            var truckService = new Mock<ITruckService>();
            var contractorService = new Mock<IContractorService>();
            var controller = new TruckController(truckService.Object, contractorService.Object);

            var result = controller.Create(truck);

            Assert.IsInstanceOfType(result, typeof(HttpNotFoundResult));
        }

        [TestMethod]
        public void CreatePostback_NullTruckCapacity_HttpNotFound()
        {
            var guid = Guid.NewGuid();
            var truck = new Truck()
            {
                TruckGuid = guid,
                LicenseNumber = "Jessee"
            };
            var truckService = new Mock<ITruckService>();
            var contractorService = new Mock<IContractorService>();
            var controller = new TruckController(truckService.Object, contractorService.Object);

            var result = controller.Create(truck);

            Assert.IsInstanceOfType(result, typeof(HttpNotFoundResult));
        }

        [TestMethod]
        public void CreatePostback_NullLicense_HttpNotFound()
        {
            var capacity = new TruckCapacityUnit() { Mass = 12, Volume = 12, TruckCapacityUnitGuid = Guid.NewGuid() };
            var guid = Guid.NewGuid();
            var truck = new Truck()
            {
                TruckGuid = guid,
                MaxCapacity = capacity
            };
            var truckService = new Mock<ITruckService>();
            var contractorService = new Mock<IContractorService>();
            var controller = new TruckController(truckService.Object, contractorService.Object);

            var result = controller.Create(truck);

            Assert.IsInstanceOfType(result, typeof(HttpNotFoundResult));
        }
    }
}