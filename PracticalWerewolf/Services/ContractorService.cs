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
        private IContractorStore Store;

        public ContractorService(IContractorStore store)
        {
            this.Store = store;
        }

        public ICollection<ContractorInfo> GetUnapprovedContractors()
        {
            throw new NotImplementedException();
        }

        public void SetIsApproved(Guid contractorInfoGuid, bool isApproved)
        {
            
            throw new NotImplementedException();
        }

        public void SetIsAvailable(Guid contractorInfoGuid, bool isAvailable)
        {
            throw new NotImplementedException();
        }
    }
}