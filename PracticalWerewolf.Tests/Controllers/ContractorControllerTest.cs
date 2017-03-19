using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PracticalWerewolf.Models;
using Microsoft.AspNet.Identity;
using System.Security.Principal;
using PracticalWerewolf.Models.UserInfos;
using System.Device.Location;
using PracticalWerewolf.Models.Trucks;
using System.Threading.Tasks;
using PracticalWerewolf.Services.Interfaces;
using PracticalWerewolf.Controllers;
using System.Web.Mvc;
using PracticalWerewolf.ViewModels.Contractor;
using System.Web;
using System.Security.Claims;
using System.Collections.Generic;
using static PracticalWerewolf.Controllers.ContractorController;
using System.Linq;

namespace PracticalWerewolf.Tests.Controllers
{
    [TestClass]
    public class ContractorControllerTest
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
            var userStore = new Mock<IUserStore<ApplicationUser>>();
            var userManager = new Mock<ApplicationUserManager>(userStore.Object);
            userManager.Setup(x => x.FindByIdAsync(It.IsAny<String>())).ReturnsAsync(contractor);

            var contractorService = new Mock<IContractorService>();
            var principal = GetMockUser(email);
            var context = GetMockControllerContext(principal);
            var controller = new ContractorController(userManager.Object, contractorService.Object);
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
            var userStore = new Mock<IUserStore<ApplicationUser>>();
            var userManager = new Mock<ApplicationUserManager>(userStore.Object);

            var contractorService = new Mock<IContractorService>();
            var user = GetMockUserNullId();
            var context = GetMockControllerContext(user);
            var controller = new ContractorController(userManager.Object, contractorService.Object);
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
            var contractor = new ApplicationUser() { UserName = email, ContractorInfo = contractorInfo };
            var userStore = new Mock<IUserStore<ApplicationUser>>();
            var userManager = new Mock<ApplicationUserManager>(userStore.Object);
            userManager.Setup(x => x.FindByIdAsync(It.IsAny<String>())).ReturnsAsync(contractor);

            var contractorService = new Mock<IContractorService>();
            var principal = GetMockUser(email);
            var context = GetMockControllerContext(principal);
            var controller = new ContractorController(userManager.Object, contractorService.Object);
            controller.ControllerContext = context;


            var result = controller.Register().Result as RedirectToRouteResult;

            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.RouteValues["action"]);
        }

        [TestMethod]
        public void Register_UserWithNoContractorInfo_View()
        {
            var email = "Example@example.com";
            var contractor = new ApplicationUser() { UserName = email};
            var userStore = new Mock<IUserStore<ApplicationUser>>();
            var userManager = new Mock<ApplicationUserManager>(userStore.Object);
            userManager.Setup(x => x.FindByIdAsync(It.IsAny<String>())).ReturnsAsync(contractor);

            var contractorService = new Mock<IContractorService>();
            var principal = GetMockUser(email);
            var context = GetMockControllerContext(principal);
            var controller = new ContractorController(userManager.Object, contractorService.Object);
            controller.ControllerContext = context;

            var result = controller.Register().Result as ViewResult;

            Assert.IsNotNull(result);
        }

        //Because the controller must be authorized, they cannot access the Register page if theyre not authenticated


        [TestMethod]
        public void Approve_ThreeUnapprovedContractors_ReturnViewModel()
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
            contractorService.Setup(x => x.GetUnapprovedContractors()).Returns(contractorList);
            var userStore = new Mock<IUserStore<ApplicationUser>>();
            var userManager = new Mock<ApplicationUserManager>(userStore.Object);
            var controller = new ContractorController(userManager.Object, contractorService.Object);


            var result = controller.Approve() as ViewResult;
            var model = result.Model as PendingContractorsModel;


            Assert.IsNotNull(result);
            Assert.IsNotNull(model);
            Assert.AreEqual(3, model.Pending.Count());
            Assert.IsTrue(model.Pending.Contains(contractorInfo1));
            Assert.IsTrue(model.Pending.Contains(contractorInfo2));
            Assert.IsTrue(model.Pending.Contains(contractorInfo3));
        }


        [TestMethod]
        public void Approve_NoUnapprovedContractors_ReturnViewModel()
        {
            var contractorList = new List<ContractorInfo>();

            var contractorService = new Mock<IContractorService>();
            contractorService.Setup(x => x.GetUnapprovedContractors()).Returns(contractorList);
            var userStore = new Mock<IUserStore<ApplicationUser>>();
            var userManager = new Mock<ApplicationUserManager>(userStore.Object);
            var controller = new ContractorController(userManager.Object, contractorService.Object);


            var result = controller.Approve() as ViewResult;
            var model = result.Model as PendingContractorsModel;


            Assert.IsNotNull(result);
            Assert.IsNotNull(model);
            Assert.AreEqual(0, model.Pending.Count());
        }



        public static GenericPrincipal GetMockUser(string id)
        {
            var identity = new GenericIdentity("");
            List<Claim> claims = new List<Claim>(){
                    new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name", id),
                    new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", id)
            };
            identity.AddClaims(claims);
            var principal = new GenericPrincipal(identity, new string[0]);

            return principal;
        }

        public static IPrincipal GetMockUserNullId()
        {
            var identity = new Mock<IIdentity>();
            var user = new Mock<IPrincipal>();
            user.Setup(x => x.Identity).Returns(identity.Object);

            return user.Object;
        }

        public static ControllerContext GetMockControllerContext(IPrincipal principal)
        {
            var httpContext = new Mock<HttpContextBase>();
            httpContext.Setup(x => x.User).Returns(principal);
            var context = new Mock<ControllerContext>();
            context.Setup(x => x.HttpContext).Returns(httpContext.Object);

            return context.Object;
        }
    }
}
