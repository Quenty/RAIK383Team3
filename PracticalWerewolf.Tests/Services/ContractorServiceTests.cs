using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PracticalWerewolf.Services;
using PracticalWerewolf.Models.UserInfos;
using PracticalWerewolf.Stores.Interfaces;
using Moq;
using System.Data.Entity;
using PracticalWerewolf.Stores;
using PracticalWerewolf.Tests.Stores.DbContext;
using System.Linq;
using System.Device.Location;
using PracticalWerewolf.Models.Trucks;
using System.Collections.Generic;
using Microsoft.AspNet.Identity;
using PracticalWerewolf.Models;

namespace PracticalWerewolf.Tests.Services
{
    [TestClass]
    public class ContractorServiceTests
    {
        [TestMethod]
        public void GetUnapprovedContractors_NoContractors_EmptyList()
        {
            var dbSet = new MockUserDbSet();
            var contractorService = GetContractorServiceWithDbSet(dbSet);

            var result = contractorService.GetUnapprovedContractors();

            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count());
        }

        

        


        public static ContractorService GetContractorServiceWithDbSet(DbSet<ApplicationUser> dbSet)
        {
            var mockContext = new Mock<IDbSetFactory>();
            mockContext.Setup(x => x.CreateDbSet<ApplicationUser>()).Returns(dbSet);
            var store = new ContractorStore(mockContext.Object);
            var userStore = new Mock<IUserStore<ApplicationUser>>();
            var userManager = new Mock<ApplicationUserManager>(userStore.Object);
            var contractorService = new ContractorService(store, userManager.Object);

            return contractorService;
        }
    }
}
