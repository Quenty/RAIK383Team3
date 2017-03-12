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

namespace PracticalWerewolf.Tests.Controllers
{
    [TestClass]
    public class TruckControllerTest
    {
        private static TruckCapacityUnit unit = new TruckCapacityUnit { TruckCapacityUnitGuid = Guid.NewGuid() };
        private static System.Data.Entity.Spatial.DbGeography location = null;

        private static IEnumerable<Truck> _trucks = new List<Truck>
        {
            new Truck {TruckGuid = Guid.NewGuid(), CurrentCapacity = unit, MaxCapacity = unit, Location = location},
            new Truck {TruckGuid = Guid.NewGuid(), CurrentCapacity = unit, MaxCapacity = unit, Location = location},
            new Truck {TruckGuid = Guid.NewGuid(), CurrentCapacity = unit, MaxCapacity = unit, Location = location}
        };

        [TestMethod]
        public void Index_NoTrucks_TestViewModel()
        {
            var truckService = new Mock<ITruckService>();
            truckService.Setup(x => x.GetAllTrucks()).Returns(new List<Truck>());
            var controller = new TruckController(truckService.Object);

            var result = controller.Index() as ViewResult;
            var model = result.Model as TruckIndexViewModel;

            Assert.IsNotNull(model);
            Assert.AreEqual(0, model.Trucks.Count());
        }

        [TestMethod]
        public void Index_ThreeTrucks_TestViewModel()
        {
            var truckService = new Mock<ITruckService>();
            truckService.Setup(x => x.GetAllTrucks()).Returns(_trucks);
            var controller = new TruckController(truckService.Object);

            var result = controller.Index() as ViewResult;
            var model = result.Model as TruckIndexViewModel;

            Assert.IsNotNull(model);
            Assert.AreEqual(3, model.Trucks.Count());
            Assert.IsTrue(model.Trucks.Contains(_trucks.ElementAt(0)));
            Assert.IsTrue(model.Trucks.Contains(_trucks.ElementAt(1)));
            Assert.IsTrue(model.Trucks.Contains(_trucks.ElementAt(2)));
        }
    }
}
