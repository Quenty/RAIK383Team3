using PracticalWerewolf.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using PracticalWerewolf.Models.UserInfos;
using PracticalWerewolf.Stores.Interfaces;
using PracticalWerewolf.Models;
using PracticalWerewolf.Models.Trucks;
using PracticalWerewolf.Models.Orders;
using System.Data.Entity;
using Microsoft.AspNet.Identity;

namespace PracticalWerewolf.Services
{
    public class ContractorService : IContractorService
    {
        private readonly IContractorStore _contractorStore;
        private readonly ApplicationUserManager UserManager;

        public ContractorService(IContractorStore store, ApplicationUserManager userManager)
        {
            UserManager = userManager;
            _contractorStore = store;
        }

        public ContractorInfo GetContractor(Guid guid)
        {
            return _contractorStore.Single(contractorInfo => contractorInfo.ContractorInfoGuid == guid);        
        }

        public ContractorInfo GetContractorByTruckGuid(Guid guid)
        {
            return _contractorStore.Find(c => c.Truck.TruckGuid == guid).FirstOrDefault();
        }

        public IEnumerable<ApplicationUser> GetUnapprovedContractors()
        {
            return UserManager.Users.Where(x => x.ContractorInfo.ApprovalState == ContractorApprovalState.Pending);
        }

        public void SetApproval(Guid contractorInfoGuid, ContractorApprovalState ApprovalState)
        {
            ContractorInfo info = _contractorStore.Single(c => c.ContractorInfoGuid == contractorInfoGuid, c => c.HomeAddress);

            if(info != null)
            {
                info.ApprovalState = ApprovalState;
                _contractorStore.Update(info);
            }
        }

        public void SetIsAvailable(Guid contractorInfoGuid, bool isAvailable)
        {
            ContractorInfo info = _contractorStore.Single(c => c.ContractorInfoGuid == contractorInfoGuid, c => c.HomeAddress);
            if (info != null)
            {
                info.IsAvailable = isAvailable;
                _contractorStore.Update(info);
            }
        }

        public IQueryable<ContractorInfo> GetAvailableContractorQuery()
        {
            return UserManager.Users.Select(c => c.ContractorInfo)
                .Where(c => c.ApprovalState == ContractorApprovalState.Approved).AsQueryable()
                .Where(c => c.IsAvailable)
                .Where(c => c.Truck != null)
                .Include(c => c.Truck)
                .Include(c => c.Truck.UsedCapacity);
        }

        public void UpdateContractorTruck(Truck truck, ApplicationUser driver)
        {
            var contractor = driver.ContractorInfo;
            contractor.Truck = truck;
            contractor.HomeAddress = contractor.HomeAddress;
            _contractorStore.Update(contractor);
        }

        public ApplicationUser GetUserByContractorInfo(ContractorInfo contractor)
        {
            return UserManager.Users.SingleOrDefault(x => x.ContractorInfo.ContractorInfoGuid == contractor.ContractorInfoGuid);
        }

        public ContractorInfo GetContractorInfo(string userId)
        {
            return UserManager.Users.Where(x => x.Id == userId).Select(x => x.ContractorInfo).FirstOrDefault();
        }

        public void RemoveOrderFromContractor(Order order, ContractorInfo contractor)
        {
            contractor.AssignedOrders.Remove(order.TrackInfo);
            _contractorStore.Update(contractor);
        }
    }
}