using PracticalWerewolf.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PracticalWerewolf.Models.UserInfos;
using PracticalWerewolf.Stores.Interfaces;

namespace PracticalWerewolf.Services
{
    public class ContractorService : IContractorService
    {
        private readonly IContractorStore _contractorStore;

        public ContractorService(IContractorStore store)
        {
            _contractorStore = store;
        }

        public IEnumerable<ContractorInfo> GetUnapprovedContractors()
        {
            return _contractorStore.Find(c => c.ApprovalState == ContractorApprovalState.Pending);
        }

        public void SetApproval(Guid contractorInfoGuid, ContractorApprovalState ApprovalState)
        {
            ContractorInfo info = _contractorStore.Single(c => c.ContractorInfoGuid == contractorInfoGuid, c => c.HomeAddress);
            info.ApprovalState = ApprovalState;
            _contractorStore.Update(info);
            
        }

        public void SetIsAvailable(Guid contractorInfoGuid, bool isAvailable)
        {
            throw new NotImplementedException();
        }
    }
}