using PracticalWerewolf.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PracticalWerewolf.Models.UserInfos;

namespace PracticalWerewolf.Services
{
    public class ContractorService : IContractorService
    {
        public void GetUserContractorInfo(Guid userId)
        {
            throw new NotImplementedException();
        }

        public void RegisterContractorInfo(Guid userId, ContractorInfo newContractorInfo)
        {
            throw new NotImplementedException();
        }

        public void UpdateContractorApproval(Guid contractorInfoGuid)
        {
            throw new NotImplementedException();
        }

        public void UpdateContractorIsAvailable(Guid contractorInfoGuid, bool isAvailable)
        {
            throw new NotImplementedException();
        }
        public IEnumerable<UserInfo> GetAllContractors()
        {
            throw new NotImplementedException();
        }
    }
}