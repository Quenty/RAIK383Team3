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
            var principal = GetMockUser(email);
            var context = GetMockControllerContext(principal);
            var unitOfWork = new Mock<IUnitOfWork>();
            var controller = new ContractorController(userManager.Object, orderService.Object, contractorService.Object, unitOfWork.Object);
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
            var user = GetMockUserNullId();
            var context = GetMockControllerContext(user);
            var unitOfWork = new Mock<IUnitOfWork>();
            var controller = new ContractorController(userManager.Object, orderService.Object, contractorService.Object, unitOfWork.Object);
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
            var controller = new ContractorController(userManager.Object, orderService.Object, contractorService.Object, unitOfWork.Object);
            controller.ControllerContext = context;


            var result = controller.Register() as RedirectToRouteResult;

            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.RouteValues["action"]);
        }

        [TestMethod]
        public void Register_UserWithNoContractorInfo_View()
        {
            var email = "Example@example.com";
            var contractor = new ApplicationUser() { UserName = email};
            var userManager = GetMockApplicationUserManager();
            userManager.Setup(x => x.FindByIdAsync(It.IsAny<String>())).ReturnsAsync(contractor);

            var contractorService = new Mock<IContractorService>();
            var orderService = new Mock<IOrderService>();
            var principal = GetMockUser(email);
            var context = GetMockControllerContext(principal);
            var unitOfWork = new Mock<IUnitOfWork>();
            var controller = new ContractorController(userManager.Object, orderService.Object, contractorService.Object, unitOfWork.Object);
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

            var contractorInfo2 = new ContractorInfo()
            {
                ContractorInfoGuid = Guid.NewGuid(),
                ApprovalState = ContractorApprovalState.Pending,
                DriversLicenseId = "id",
                HomeAddress = new CivicAddressDb(),
                IsAvailable = true,
                Truck = new Truck()
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

            var contractorList = new List<ContractorInfo>() { contractorInfo1, contractorInfo2, contractorInfo3 };

            var contractorService = new Mock<IContractorService>();
            var orderService = new Mock<IOrderService>();
            contractorService.Setup(x => x.GetUnapprovedContractors()).Returns(contractorList);
            var userManager = GetMockApplicationUserManager();
            var unitOfWork = new Mock<IUnitOfWork>();
            var controller = new ContractorController(userManager.Object, orderService.Object, contractorService.Object, unitOfWork.Object);


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
            var contractorList = new List<ContractorInfo>();

            var contractorService = new Mock<IContractorService>();
            var orderService = new Mock<IOrderService>();
            contractorService.Setup(x => x.GetUnapprovedContractors()).Returns(contractorList);
            var userManager = GetMockApplicationUserManager();
            var unitOfWork = new Mock<IUnitOfWork>();
            var controller = new ContractorController(userManager.Object, orderService.Object, contractorService.Object, unitOfWork.Object);


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
            var contractorController = new ContractorController(context.Object, orderService.Object, contractorService.Object, unitOfWork.Object);
            
            var result = contractorController.Approve(Guid.NewGuid(), true) as RedirectToRouteResult;

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

            var context = GetMockApplicationUserManager();
            context.Setup(x => x.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(contractor);
            var contractorService = new Mock<IContractorService>();
            var orderService = new Mock<IOrderService>();
            var unitOfWork = new Mock<IUnitOfWork>();
            var controller = new ContractorController(context.Object, orderService.Object, contractorService.Object, unitOfWork.Object);
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

            var context = GetMockApplicationUserManager();
            context.Setup(x => x.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(contractor);
            context.Setup(x => x.UpdateAsync(It.IsAny<ApplicationUser>())).ReturnsAsync(IdentityResult.Success);

            var contractorService = new Mock<IContractorService>();
            var unitOfWork = new Mock<IUnitOfWork>();
            var orderService = new Mock<IOrderService>();
            var controller = new ContractorController(context.Object, orderService.Object, contractorService.Object, unitOfWork.Object);
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

            var context = GetMockApplicationUserManager();
            context.Setup(x => x.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(contractor);
            context.Setup(x => x.UpdateAsync(It.IsAny<ApplicationUser>())).ReturnsAsync(IdentityResult.Failed());

            var contractorService = new Mock<IContractorService>();
            var orderService = new Mock<IOrderService>();
            var unitOfWork = new Mock<IUnitOfWork>();
            var controller = new ContractorController(context.Object, orderService.Object, contractorService.Object, unitOfWork.Object);
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

    }
}
