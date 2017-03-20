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

namespace PracticalWerewolf.Tests.Services
{
    [TestClass]
    public class ContractorServiceTest
    {
        [TestMethod]
        public void GetUnapprovedContractors_NoContractors_EmptyList()
        {
            var dbSet = new MockContractorDbSet();
            var contractorService = GetContractorServiceWithDbSet(dbSet);

            var result = contractorService.GetUnapprovedContractors();

            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count());
        }

        [TestMethod]
        public void GetUnapprovedContractors_ThreeContractors_OneUnapproved()
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
                ApprovalState = ContractorApprovalState.Approved,
                DriversLicenseId = "id",
                HomeAddress = new CivicAddressDb(),
                IsAvailable = true,
                Truck = new Truck()
            };

            var contractorInfo3 = new ContractorInfo()
            {
                ContractorInfoGuid = Guid.NewGuid(),
                ApprovalState = ContractorApprovalState.Denied,
                DriversLicenseId = "id",
                HomeAddress = new CivicAddressDb(),
                IsAvailable = true,
                Truck = new Truck()
            };

            var contractorList = new List<ContractorInfo>() { contractorInfo1, contractorInfo2, contractorInfo3 };
            var dbSet = new MockContractorDbSet();
            dbSet.AddRange(contractorList);
            var contractorService = GetContractorServiceWithDbSet(dbSet);

            var result = contractorService.GetUnapprovedContractors();

            Assert.AreEqual(1, result.Count());
            Assert.AreEqual(contractorInfo1, result.ElementAt(0));
        }

        [TestMethod]
        public void GetUnapprovedContractors_ThreeContractors_ThreeUnapproved()
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
            var dbSet = new MockContractorDbSet();
            dbSet.AddRange(contractorList);
            var contractorService = GetContractorServiceWithDbSet(dbSet);

            var result = contractorService.GetUnapprovedContractors();

            Assert.AreEqual(3, result.Count());
            Assert.IsTrue(result.Contains(contractorInfo1));
            Assert.IsTrue(result.Contains(contractorInfo2));
            Assert.IsTrue(result.Contains(contractorInfo3));
        }

        [TestMethod]
        public void SetApproval_NoContractor_BreakSomething()
        {
            var contractorService = GetContractorServiceWithDbSet(new MockContractorDbSet());

            contractorService.SetApproval(Guid.NewGuid(), ContractorApprovalState.Approved);
        }

        [TestMethod]
        public void SetApproval_OneContractor_UpdatedContractor()
        {
            var guid = Guid.NewGuid();
            var contractorInfo = new ContractorInfo()
            {
                ContractorInfoGuid = guid,
                ApprovalState = ContractorApprovalState.Pending,
                DriversLicenseId = "id",
                HomeAddress = new CivicAddressDb(),
                IsAvailable = true,
                Truck = new Truck()
            };
            var dbSet = new MockContractorDbSet();
            dbSet.Add(contractorInfo);
            var contractorService = GetContractorServiceWithDbSet(dbSet);

            contractorService.SetApproval(guid, ContractorApprovalState.Approved);

            Assert.AreEqual(ContractorApprovalState.Approved, dbSet.First().ApprovalState);
        }


        public static ContractorService GetContractorServiceWithDbSet(DbSet<ContractorInfo> dbSet)
        {
            var mockContext = new Mock<IDbSetFactory>();
            mockContext.Setup(x => x.CreateDbSet<ContractorInfo>()).Returns(dbSet);
            var store = new ContractorStore(mockContext.Object);
            var contractorService = new ContractorService(store);

            return contractorService;
        }
    }
}
