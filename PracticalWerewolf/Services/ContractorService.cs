using PracticalWerewolf.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PracticalWerewolf.Models.UserInfos;
using PracticalWerewolf.Stores.Interfaces;
using PracticalWerewolf.Models;
using PracticalWerewolf.Models.Trucks;

namespace PracticalWerewolf.Services
{
    public class ContractorService : IContractorService
    {
        private IContractorStore Store;

        public ContractorService(IContractorStore store)
        {
            this.Store = store;
        }

        public ContractorInfo GetContractorByTruckGuid(Guid guid)
        {
            return Store.Find(c => c.Truck.TruckGuid == guid).FirstOrDefault();
        }

        public IEnumerable<ContractorInfo> GetUnapprovedContractors()
        {
            return Store.Find(c => c.ApprovalState == ContractorApprovalState.Pending);
        }

        public void SetIsApproved(Guid contractorInfoGuid, bool isApproved)
        {
            
            throw new NotImplementedException();
        }

        public void SetIsAvailable(Guid contractorInfoGuid, bool isAvailable)
        {
            throw new NotImplementedException();
        }

        public void UpdateContractorTruck(Truck truck, ApplicationUser driver)
        {
            var contractor = Store.Find(c => c.ContractorInfoGuid == driver.ContractorInfo.ContractorInfoGuid).FirstOrDefault();
            Store.UpdateContractorTruck(contractor, truck);
        }
    }
}