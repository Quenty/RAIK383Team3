using PracticalWerewolf.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PracticalWerewolf.Models.UserInfos;
using PracticalWerewolf.Stores;

namespace PracticalWerewolf.Services
{
    public class UserInfoService : IUserInfoService
    {
        private readonly ContractorStore ContractorStore;

        public UserInfoService(ContractorStore ContractorStore)
        {
            this.ContractorStore = ContractorStore;
        }

        public ContractorInfo GetContractorInfo(Guid guid)
        {
            return ContractorStore.Single(c => c.ContractorInfoGuid == guid);
        }

        public IEnumerable<UserInfo> GetAllUsers()
        {
            throw new NotImplementedException();
        }

        public CustomerInfo GetUserContractorInfo(string id)
        {
            throw new NotImplementedException();
        }
    }
}