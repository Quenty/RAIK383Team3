using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PracticalWerewolf.Models;
using Microsoft.AspNet.Identity;
using PracticalWerewolf.Models.UserInfos;
using System.Device.Location;
using PracticalWerewolf.Models.Trucks;
using PracticalWerewolf.Services.Interfaces;
using PracticalWerewolf.Controllers;
using System.Web.Mvc;
using PracticalWerewolf.ViewModels.Contractor;
using System.Collections.Generic;
using static PracticalWerewolf.Controllers.ContractorController;
using System.Linq;
using PracticalWerewolf.Controllers.UnitOfWork;
using System.Security.Claims;
using PracticalWerewolf.Models.Orders;

namespace PracticalWerewolf.Tests.Controllers
{
    [TestClass]
    public class ContractorControllerTest : ControllerTest
    {
        [TestMethod]
        public void Index_ExistingUser_ViewWithViewModel()
        {
            var email = "Example@example.com";
            var contractorInfo = new ContractorInfo()
            {
                ContractorInfoGuid = Guid.NewGuid(),
                ApprovalState = ContractorApprovalState.Pending,
                DriversLicenseId = "id",
                HomeAddress = new CivicAddressDb(),
                IsAvailable = true,
                Truck = new Truck()
            };
            var contractor = new ApplicationUser() { UserName = email, ContractorInfo = contractorInfo };
            var userManager = GetMockApplicationUserManager();
            userManager.Setup(x => x.FindByIdAsync(It.IsAny<String>())).ReturnsAsync(contractor);

            var contractorService = new Mock<IContractorService>();
            var orderService = new Mock<IOrderService>();
            var routeStopService = new Mock<IRouteStopService>();
            var principal = GetMockUser(email);
            var context = GetMockControllerContext(principal);
            var unitOfWork = new Mock<IUnitOfWork>();
            var truckService = new Mock<ITruckService>();
            var routePlannerService = new Mock<IRoutePlannerService>();
            var controller = new ContractorController(userManager.Object, orderService.Object, contractorService.Object, unitOfWork.Object, routeStopService.Object, routePlannerService.Object, truckService.Object);
            controller.ControllerContext = context;


            var result = controller.Index(null).Result as ViewResult;
            var model = result.Model as ContractorIndexModel;


            Assert.IsNotNull(result);
            Assert.IsNotNull(model);
            Assert.AreEqual(contractorInfo, model.ContractorInfo);
        }

        [TestMethod]
        public void Index_NewUser_ViewWithEmptyModel()
        {
            var userManager = GetMockApplicationUserManager();

            var contractorService = new Mock<IContractorService>();
            var orderService = new Mock<IOrderService>();
            var routeStopService = new Mock<IRouteStopService>();
            var user = GetMockUserNullId();
            var context = GetMockControllerContext(user);
            var unitOfWork = new Mock<IUnitOfWork>();
            var truckService = new Mock<ITruckService>();
            var routePlannerService = new Mock<IRoutePlannerService>();
            var controller = new ContractorController(userManager.Object, orderService.Object, contractorService.Object, unitOfWork.Object, routeStopService.Object, routePlannerService.Object, truckService.Object);

            controller.ControllerContext = context;


            var result = controller.Index(ContractorMessageId.RegisterSuccess).Result as ViewResult;
            var model = result.Model as ContractorIndexModel;


            Assert.IsNotNull(result);
            Assert.IsNotNull(model);
            Assert.IsNull(model.ContractorInfo);
        }

        [TestMethod]
        public void Register_ValidContractor_RedirectToIndex()
        {
            var email = "Example@example.com";
            var contractorInfo = new ContractorInfo()
            {
                ContractorInfoGuid = Guid.NewGuid(),
                ApprovalState = ContractorApprovalState.Pending,
                DriversLicenseId = "id",
                HomeAddress = new CivicAddressDb(),
                IsAvailable = true,
                Truck = new Truck()
            };
            var user = new ApplicationUser() { UserName = email, ContractorInfo = contractorInfo };

            var userManager = GetMockApplicationUserManager();
            userManager.Setup(x => x.FindByIdAsync(It.IsAny<String>())).ReturnsAsync(user);

            var contractorService = new Mock<IContractorService>();
            var orderService = new Mock<IOrderService>();
            var principal = GetMockUser(email, new List<Claim>()
            {
                new Claim(ClaimTypes.Role, "Contractor")
            });

            var context = GetMockControllerContext(principal);
            var unitOfWork = new Mock<IUnitOfWork>();
            var routeStopService = new Mock<IRouteStopService>();
            var truckService = new Mock<ITruckService>();
            var routePlannerService = new Mock<IRoutePlannerService>();
            var controller = new ContractorController(userManager.Object, orderService.Object, contractorService.Object, unitOfWork.Object, routeStopService.Object, routePlannerService.Object, truckService.Object);

            controller.ControllerContext = context;


            var result = controller.Register() as RedirectToRouteResult;

            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.RouteValues["action"]);
        }

        [TestMethod]
        public void Register_UserWithNoContractorInfo_View()
        {
            var email = "Example@example.com";
            var contractor = new ApplicationUser() { UserName = email };
            var userManager = GetMockApplicationUserManager();
            userManager.Setup(x => x.FindByIdAsync(It.IsAny<String>())).ReturnsAsync(contractor);

            var contractorService = new Mock<IContractorService>();
            var orderService = new Mock<IOrderService>();
            var principal = GetMockUser(email);
            var context = GetMockControllerContext(principal);
            var unitOfWork = new Mock<IUnitOfWork>();
            var routeStopService = new Mock<IRouteStopService>();
            var truckService = new Mock<ITruckService>();
            var routePlannerService = new Mock<IRoutePlannerService>();
            var controller = new ContractorController(userManager.Object, orderService.Object, contractorService.Object, unitOfWork.Object, routeStopService.Object, routePlannerService.Object, truckService.Object);

            controller.ControllerContext = context;

            var result = controller.Register() as ViewResult;

            Assert.IsNotNull(result);
        }

        //Because the controller must be authorized, they cannot access the Register page if theyre not authenticated


        [TestMethod]
        public void Unapproved_ThreeUnapprovedContractors_ReturnViewModel()
        {
            var contractorInfo1 = new ContractorInfo()
            {
                ContractorInfoGuid = Guid.NewGuid(),
                ApprovalState = ContractorApprovalState.Pending,
                DriversLicenseId = "id",
                HomeAddress = new CivicAddressDb(),
                IsAvailable = true,
                Truck = new Truck()
            };

            var applicationUser1 = new ApplicationUser
            {
                UserName = "User1",
                Email = "User1@User.com",
                ContractorInfo = contractorInfo1
            };

            var contractorInfo2 = new ContractorInfo()
            {
                ContractorInfoGuid = Guid.NewGuid(),
                ApprovalState = ContractorApprovalState.Pending,
                DriversLicenseId = "id",
                HomeAddress = new CivicAddressDb(),
                IsAvailable = true,
                Truck = new Truck()
            };
            var applicationUser2 = new ApplicationUser
            {
                UserName = "User2",
                Email = "User2@User.com",
                ContractorInfo = contractorInfo2
            };

            var contractorInfo3 = new ContractorInfo()
            {
                ContractorInfoGuid = Guid.NewGuid(),
                ApprovalState = ContractorApprovalState.Pending,
                DriversLicenseId = "id",
                HomeAddress = new CivicAddressDb(),
                IsAvailable = true,
                Truck = new Truck()
            };

            var applicationUser3 = new ApplicationUser
            {
                UserName = "User3",
                Email = "User3@User.com",
                ContractorInfo = contractorInfo3
            };

            var contractorList = new List<ApplicationUser>() { applicationUser1, applicationUser2, applicationUser3 };

            var contractorService = new Mock<IContractorService>();
            var orderService = new Mock<IOrderService>();
            contractorService.Setup(x => x.GetUnapprovedContractors()).Returns(contractorList);
            var userManager = GetMockApplicationUserManager();
            var unitOfWork = new Mock<IUnitOfWork>();
            var routeStopService = new Mock<IRouteStopService>();
            var truckService = new Mock<ITruckService>();
            var routePlannerService = new Mock<IRoutePlannerService>();
            var controller = new ContractorController(userManager.Object, orderService.Object, contractorService.Object, unitOfWork.Object, routeStopService.Object, routePlannerService.Object, truckService.Object);



            var result = controller.Unapproved(null) as ViewResult;
            var model = result.Model as PendingContractorsModel;


            Assert.IsNotNull(result);
            Assert.IsNotNull(model);
            Assert.AreEqual(3, model.Pending.Count());
            Assert.IsNotNull(model.Pending.Where(x => x.ContractorInfo.Equals(contractorInfo1)).Single());
            Assert.IsNotNull(model.Pending.Where(x => x.ContractorInfo.Equals(contractorInfo2)).Single());
            Assert.IsNotNull(model.Pending.Where(x => x.ContractorInfo.Equals(contractorInfo3)).Single());
        }


        [TestMethod]
        public void Unapproved_NoUnapprovedContractors_ReturnViewModel()
        {
            var contractorList = new List<ApplicationUser>();

            var contractorService = new Mock<IContractorService>();
            var orderService = new Mock<IOrderService>();
            contractorService.Setup(x => x.GetUnapprovedContractors()).Returns(contractorList);
            var userManager = GetMockApplicationUserManager();
            var unitOfWork = new Mock<IUnitOfWork>();
            var routeStopService = new Mock<IRouteStopService>();
            var truckService = new Mock<ITruckService>();
            var routePlannerService = new Mock<IRoutePlannerService>();
            var controller = new ContractorController(userManager.Object, orderService.Object, contractorService.Object, unitOfWork.Object, routeStopService.Object, routePlannerService.Object, truckService.Object);


            var result = controller.Unapproved(null) as ViewResult;
            var model = result.Model as PendingContractorsModel;


            Assert.IsNotNull(result);
            Assert.IsNotNull(model);
            Assert.AreEqual(0, model.Pending.Count());
        }


        [TestMethod]
        public void Approve_Guid_Redirect()
        {
            var context = GetMockApplicationUserManager();
            var contractorService = new Mock<IContractorService>();
            var orderService = new Mock<IOrderService>();
            var unitOfWork = new Mock<IUnitOfWork>();
            var mockContext = context.Object;
            var routeStopService = new Mock<IRouteStopService>();
            var truckService = new Mock<ITruckService>();
            var routePlannerService = new Mock<IRoutePlannerService>();
            var controller = new ContractorController(context.Object, orderService.Object, contractorService.Object, unitOfWork.Object, routeStopService.Object, routePlannerService.Object, truckService.Object);

            var result = controller.Approve(Guid.NewGuid(), true) as RedirectToRouteResult;

            Assert.IsNotNull(result);
            Assert.AreEqual("Unapproved", result.RouteValues["action"]);
            Assert.AreEqual(ContractorMessageId.ApprovedSuccess, result.RouteValues["message"]);
        }

        [TestMethod]
        public void Register_UserWithContractorInfo_RedirectToIndex()
        {
            var email = "Example@example.com";
            var contractorInfo = new ContractorInfo()
            {
                ContractorInfoGuid = Guid.NewGuid(),
                ApprovalState = ContractorApprovalState.Pending,
                DriversLicenseId = "id",
                HomeAddress = new CivicAddressDb(),
                IsAvailable = true,
                Truck = new Truck()
            };
            var contractor = new ApplicationUser() { UserName = email, ContractorInfo = contractorInfo };

            var mockUser = GetMockUser(email);
            var mockContext = GetMockControllerContext(mockUser);

            var userManager = GetMockApplicationUserManager();
            userManager.Setup(x => x.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(contractor);
            var contractorService = new Mock<IContractorService>();
            var orderService = new Mock<IOrderService>();
            var unitOfWork = new Mock<IUnitOfWork>();
            var routeStopService = new Mock<IRouteStopService>();
            var truckService = new Mock<ITruckService>();
            var routePlannerService = new Mock<IRoutePlannerService>();
            var controller = new ContractorController(userManager.Object, orderService.Object, contractorService.Object, unitOfWork.Object, routeStopService.Object, routePlannerService.Object, truckService.Object);

            controller.ControllerContext = mockContext;

            var result = controller.Register(new ContractorRegisterModel()).Result as RedirectToRouteResult;

            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.RouteValues["action"]);
            Assert.AreEqual(ContractorMessageId.AlreadyRegisteredError, result.RouteValues["message"]);
        }

        [TestMethod]
        public void Register_UserWithNoContractorInfo_RedirectToIndex()
        {
            var email = "Example@example.com";
            var contractor = new ApplicationUser() { UserName = email };

            var mockUser = GetMockUser(email);
            var mockContext = GetMockControllerContext(mockUser);

            var userManager = GetMockApplicationUserManager();
            userManager.Setup(x => x.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(contractor);
            userManager.Setup(x => x.UpdateAsync(It.IsAny<ApplicationUser>())).ReturnsAsync(IdentityResult.Success);

            var contractorService = new Mock<IContractorService>();
            var unitOfWork = new Mock<IUnitOfWork>();
            var orderService = new Mock<IOrderService>();
            var routeStopService = new Mock<IRouteStopService>();
            var truckService = new Mock<ITruckService>();
            var routePlannerService = new Mock<IRoutePlannerService>();
            var controller = new ContractorController(userManager.Object, orderService.Object, contractorService.Object, unitOfWork.Object, routeStopService.Object, routePlannerService.Object, truckService.Object);

            controller.ControllerContext = mockContext;

            var contractorRegisterModel = new ContractorRegisterModel()
            {
                Address = new CivicAddressDb(),
                DriversLicenseId = "@JuicyJames",
                RawAddressString = "630 North 14th Street, Kauffman Hall",
                TermsOfService = false
            };

            var result = controller.Register(contractorRegisterModel).Result as RedirectToRouteResult;

            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.RouteValues["action"]);
            Assert.AreEqual(ContractorMessageId.RegisterSuccess, result.RouteValues["message"]);
        }

        [TestMethod]
        public void Register_UserWithNoContractorInfoFailure_RedirectToIndex()
        {
            var email = "Example@example.com";
            var contractor = new ApplicationUser() { UserName = email };

            var mockUser = GetMockUser(email);
            var mockContext = GetMockControllerContext(mockUser);

            var userManager = GetMockApplicationUserManager();
            userManager.Setup(x => x.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(contractor);
            userManager.Setup(x => x.UpdateAsync(It.IsAny<ApplicationUser>())).ReturnsAsync(IdentityResult.Failed());

            var contractorService = new Mock<IContractorService>();
            var orderService = new Mock<IOrderService>();
            var unitOfWork = new Mock<IUnitOfWork>();
            var routeStopService = new Mock<IRouteStopService>();
            var truckService = new Mock<ITruckService>();
            var routePlannerService = new Mock<IRoutePlannerService>();
            var controller = new ContractorController(userManager.Object, orderService.Object, contractorService.Object, unitOfWork.Object, routeStopService.Object, routePlannerService.Object, truckService.Object);

            controller.ControllerContext = mockContext;

            var contractorRegisterModel = new ContractorRegisterModel()
            {
                Address = new CivicAddressDb(),
                DriversLicenseId = "@JuicyJames",
                RawAddressString = "630 North 14th Street, Kauffman Hall",
                TermsOfService = false
            };

            var result = controller.Register(contractorRegisterModel).Result as RedirectToRouteResult;

            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.RouteValues["action"]);
            Assert.AreEqual(ContractorMessageId.Error, result.RouteValues["message"]);
        }

        [TestMethod]
        public void Pending_ValidUser_ReturnsPartialView()
        {
            var email = "Example@example.com";
            var contractor = new ApplicationUser() { UserName = email, ContractorInfo = new ContractorInfo() };
            var mockUser = GetMockUser(email);
            var mockContext = GetMockControllerContext(mockUser);
            var userManager = GetMockApplicationUserManager();
            userManager.Setup(x => x.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(contractor);

            var contractorService = new Mock<IContractorService>();
            var orderService = new Mock<IOrderService>();
            var routeStopService = new Mock<IRouteStopService>();
            orderService.Setup(x => x.GetQueuedOrders(It.IsAny<ContractorInfo>())).Returns(new List<Order>());
            var unitOfWork = new Mock<IUnitOfWork>();
            var truckService = new Mock<ITruckService>();
            var routePlannerService = new Mock<IRoutePlannerService>();
            var controller = new ContractorController(userManager.Object, orderService.Object, contractorService.Object, unitOfWork.Object, routeStopService.Object, routePlannerService.Object, truckService.Object);

            controller.ControllerContext = mockContext;


            var result = controller.Pending().Result as PartialViewResult;
            var viewModel = result.Model as PagedOrderListViewModel;


            Assert.IsNotNull(viewModel);
            Assert.AreEqual(0, viewModel.Orders.Count());
        }

        [TestMethod]
        public void Pending_ValidUser_ReturnsPartialViewWithOrders()
        {
            var email = "Example@example.com";
            var contractor = new ApplicationUser() { UserName = email, ContractorInfo = new ContractorInfo() };
            var mockUser = GetMockUser(email);
            var mockContext = GetMockControllerContext(mockUser);
            var userManager = GetMockApplicationUserManager();
            userManager.Setup(x => x.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(contractor);

            var orders = new List<Order>()
            {
                new Order(),
                new Order(),
                new Order()
            };

            var contractorService = new Mock<IContractorService>();
            var routeStopService = new Mock<IRouteStopService>();
            var orderService = new Mock<IOrderService>();
            orderService.Setup(x => x.GetInprogressOrdersNoTruck(It.IsAny<ContractorInfo>())).Returns(orders);
            var unitOfWork = new Mock<IUnitOfWork>();
            var truckService = new Mock<ITruckService>();
            var routePlannerService = new Mock<IRoutePlannerService>();
            var controller = new ContractorController(userManager.Object, orderService.Object, contractorService.Object, unitOfWork.Object, routeStopService.Object, routePlannerService.Object, truckService.Object);

            controller.ControllerContext = mockContext;


            var result = controller.Pending().Result as PartialViewResult;
            var viewModel = result.Model as PagedOrderListViewModel;


            Assert.IsNotNull(viewModel);
            Assert.AreEqual(orders, viewModel.Orders);
        }

        [TestMethod]
        public void Pending_InvalidUser_ReturnsView()
        {
            var mockUser = GetMockUserNullId();
            var mockContext = GetMockControllerContext(mockUser);
            var userManager = GetMockApplicationUserManager();

            var contractorService = new Mock<IContractorService>();
            var orderService = new Mock<IOrderService>();
            var unitOfWork = new Mock<IUnitOfWork>();
            var routeStopService = new Mock<IRouteStopService>();
            var truckService = new Mock<ITruckService>();
            var routePlannerService = new Mock<IRoutePlannerService>();
            var controller = new ContractorController(userManager.Object, orderService.Object, contractorService.Object, unitOfWork.Object, routeStopService.Object, routePlannerService.Object, truckService.Object);

            controller.ControllerContext = mockContext;


            var result = controller.Pending().Result as ViewResult;


            Assert.IsNull(result.Model);
        }

        [TestMethod]
        public void Current_ValidUser_ReturnsPartialView()
        {
            var email = "Example@example.com";
            var contractor = new ApplicationUser() { UserName = email, ContractorInfo = new ContractorInfo() };
            var mockUser = GetMockUser(email);
            var mockContext = GetMockControllerContext(mockUser);
            var userManager = GetMockApplicationUserManager();
            userManager.Setup(x => x.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(contractor);

            var contractorService = new Mock<IContractorService>();
            var orderService = new Mock<IOrderService>();
            orderService.Setup(x => x.GetInprogressOrders(It.IsAny<ContractorInfo>())).Returns(new List<Order>());
            var unitOfWork = new Mock<IUnitOfWork>();
            var routeStopService = new Mock<IRouteStopService>();
            var truckService = new Mock<ITruckService>();
            var routePlannerService = new Mock<IRoutePlannerService>();
            var controller = new ContractorController(userManager.Object, orderService.Object, contractorService.Object, unitOfWork.Object, routeStopService.Object, routePlannerService.Object, truckService.Object);

            controller.ControllerContext = mockContext;


            var result = controller.Current().Result as PartialViewResult;
            var viewModel = result.Model as PagedOrderListViewModel;


            Assert.IsNotNull(viewModel);
            Assert.AreEqual(0, viewModel.Orders.Count());
        }

        [TestMethod]
        public void Current_ValidUser_ReturnsPartialViewWithOrders()
        {
            var email = "Example@example.com";
            var contractor = new ApplicationUser() { UserName = email, ContractorInfo = new ContractorInfo() };
            var mockUser = GetMockUser(email);
            var mockContext = GetMockControllerContext(mockUser);
            var userManager = GetMockApplicationUserManager();
            userManager.Setup(x => x.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(contractor);

            var orders = new List<Order>()
            {
                new Order(),
                new Order(),
                new Order()
            };

            var contractorService = new Mock<IContractorService>();
            var orderService = new Mock<IOrderService>();
            var routeStopService = new Mock<IRouteStopService>();
            orderService.Setup(x => x.GetInprogressOrders(It.IsAny<ContractorInfo>())).Returns(orders);
            var unitOfWork = new Mock<IUnitOfWork>();
            var truckService = new Mock<ITruckService>();
            var routePlannerService = new Mock<IRoutePlannerService>();
            var controller = new ContractorController(userManager.Object, orderService.Object, contractorService.Object, unitOfWork.Object, routeStopService.Object, routePlannerService.Object, truckService.Object);

            controller.ControllerContext = mockContext;


            var result = controller.Current().Result as PartialViewResult;
            var viewModel = result.Model as PagedOrderListViewModel;


            Assert.IsNotNull(viewModel);
            Assert.AreEqual(orders, viewModel.Orders);
        }

        [TestMethod]
        public void Current_InvalidUser_ReturnsView()
        {
            var mockUser = GetMockUserNullId();
            var mockContext = GetMockControllerContext(mockUser);
            var userManager = GetMockApplicationUserManager();

            var contractorService = new Mock<IContractorService>();
            var orderService = new Mock<IOrderService>();
            var unitOfWork = new Mock<IUnitOfWork>();
            var routeStopService = new Mock<IRouteStopService>();
            var truckService = new Mock<ITruckService>();
            var routePlannerService = new Mock<IRoutePlannerService>();
            var controller = new ContractorController(userManager.Object, orderService.Object, contractorService.Object, unitOfWork.Object, routeStopService.Object, routePlannerService.Object, truckService.Object);

            controller.ControllerContext = mockContext;


            var result = controller.Current().Result as ViewResult;


            Assert.IsNull(result.Model);
        }

        [TestMethod]
        public void Delivered_ValidUser_ReturnsPartialView()
        {
            var email = "Example@example.com";
            var contractor = new ApplicationUser() { UserName = email, ContractorInfo = new ContractorInfo() };
            var mockUser = GetMockUser(email);
            var mockContext = GetMockControllerContext(mockUser);
            var userManager = GetMockApplicationUserManager();
            var routeStopService = new Mock<IRouteStopService>();
            userManager.Setup(x => x.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(contractor);

            var contractorService = new Mock<IContractorService>();
            var orderService = new Mock<IOrderService>();
            orderService.Setup(x => x.GetDeliveredOrders(It.IsAny<ContractorInfo>())).Returns(new List<Order>());
            var unitOfWork = new Mock<IUnitOfWork>();
            var truckService = new Mock<ITruckService>();
            var routePlannerService = new Mock<IRoutePlannerService>();
            var controller = new ContractorController(userManager.Object, orderService.Object, contractorService.Object, unitOfWork.Object, routeStopService.Object, routePlannerService.Object, truckService.Object);

            controller.ControllerContext = mockContext;


            var result = controller.Delivered().Result as PartialViewResult;
            var viewModel = result.Model as PagedOrderListViewModel;


            Assert.IsNotNull(viewModel);
            Assert.AreEqual(0, viewModel.Orders.Count());
        }

        [TestMethod]
        public void Delivered_ValidUser_ReturnsPartialViewWithOrders()
        {
            var email = "Example@example.com";
            var contractor = new ApplicationUser() { UserName = email, ContractorInfo = new ContractorInfo() };
            var mockUser = GetMockUser(email);
            var routeStopService = new Mock<IRouteStopService>();
            var mockContext = GetMockControllerContext(mockUser);
            var userManager = GetMockApplicationUserManager();
            userManager.Setup(x => x.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(contractor);

            var orders = new List<Order>()
            {
                new Order(),
                new Order(),
                new Order()
            };

            var contractorService = new Mock<IContractorService>();
            var orderService = new Mock<IOrderService>();
            orderService.Setup(x => x.GetDeliveredOrders(It.IsAny<ContractorInfo>())).Returns(orders);
            var unitOfWork = new Mock<IUnitOfWork>();
            var truckService = new Mock<ITruckService>();
            var routePlannerService = new Mock<IRoutePlannerService>();
            var controller = new ContractorController(userManager.Object, orderService.Object, contractorService.Object, unitOfWork.Object, routeStopService.Object, routePlannerService.Object, truckService.Object);

            controller.ControllerContext = mockContext;


            var result = controller.Delivered().Result as PartialViewResult;
            var viewModel = result.Model as PagedOrderListViewModel;


            Assert.IsNotNull(viewModel);
            Assert.AreEqual(orders, viewModel.Orders);
        }

        [TestMethod]
        public void Delivered_InvalidUser_ReturnsView()
        {
            var mockUser = GetMockUserNullId();
            var mockContext = GetMockControllerContext(mockUser);
            var userManager = GetMockApplicationUserManager();

            var routeStopService = new Mock<IRouteStopService>();
            var contractorService = new Mock<IContractorService>();
            var orderService = new Mock<IOrderService>();
            var unitOfWork = new Mock<IUnitOfWork>();
            var truckService = new Mock<ITruckService>();
            var routePlannerService = new Mock<IRoutePlannerService>();
            var controller = new ContractorController(userManager.Object, orderService.Object, contractorService.Object, unitOfWork.Object, routeStopService.Object, routePlannerService.Object, truckService.Object);

            controller.ControllerContext = mockContext;


            var result = controller.Delivered().Result as ViewResult;


            Assert.IsNull(result.Model);
        }
    }
}