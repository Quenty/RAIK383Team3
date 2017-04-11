using PracticalWerewolf.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using PracticalWerewolf.Models.UserInfos;
using PracticalWerewolf.Stores.Interfaces;
using PracticalWerewolf.Models;
using PracticalWerewolf.Models.Trucks;

namespace PracticalWerewolf.Services
{
    public class ContractorService : IContractorService
    {
        private readonly IContractorStore _contractorStore;
        private readonly ApplicationUserManager _userManager;

        public ContractorService(IContractorStore store, ApplicationUserManager userManager)
        {
            _userManager = userManager;
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

        public IEnumerable<ContractorInfo> GetUnapprovedContractors()
        {
            return _contractorStore.Find(c => c.ApprovalState == ContractorApprovalState.Pending);
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

        public void UpdateContractorTruck(Truck truck, ApplicationUser driver)
        {
            driver.ContractorInfo.Truck = truck;
            var result = _userManager.UpdateAsync(driver);
            result.Wait(); // TODO: Make this async
        }
    }
}