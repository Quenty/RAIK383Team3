using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PracticalWerewolf;
using PracticalWerewolf.Controllers;
using PracticalWerewolf.Services.Interfaces;
using Moq;
using System.Linq.Expressions;
using PracticalWerewolf.Models.Orders;
using PracticalWerewolf.Models.Home;
using PracticalWerewolf.Models;

namespace PracticalWerewolf.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        public HomeController GetHomeController()
        {
            var os = new Mock<IOrderService>();
            os.Setup(m => m.QueryCount(It.IsAny<Expression<Func<Order, bool>>>())).Returns(5);
            os.Setup(m => m.QueryCount(It.Is<Expression<Func<Order, bool>>>(x => x.ToString().Contains("Complete")))).Returns(100);
            os.Setup(m => m.QueryCount(It.Is<Expression<Func<Order, bool>>>(x => x.ToString().Contains("InProgress")))).Returns(50);

            var ui = new Mock<IUserInfoService>();
            ui.Setup(m => m.QueryCount(It.IsAny<Expression<Func<ApplicationUser, bool>>>())).Returns(5);
            ui.Setup(m => m.QueryCount(It.Is<Expression<Func<ApplicationUser, bool>>>(x => x.ToString().Contains("Customer")))).Returns(25);
            ui.Setup(m => m.QueryCount(It.Is<Expression<Func<ApplicationUser, bool>>>(x => x.ToString().Contains("Employee")))).Returns(37);

            HomeController controller = new HomeController(os.Object, ui.Object);

            return controller;
        }

        [TestMethod]
        public void Index()
        {
            // Arrange
            HomeController controller = GetHomeController();

            // Act
            ViewResult result = controller.Index() as ViewResult;
            

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result.Model, typeof(Statistics));
            Assert.IsTrue((result.Model as Statistics).PackageStateChart.Data.Contains(50));
            Assert.IsTrue((result.Model as Statistics).UsersChart.Data.Contains(37));
        }

        [TestMethod]
        public void About()
        {
            // Arrange
            HomeController controller = GetHomeController();

            // Act
            ViewResult result = controller.About() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }
    }
}
