using PracticalWerewolf.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PracticalWerewolf.Models.UserInfos;
using PracticalWerewolf.Stores;
using PracticalWerewolf.Models;
using Microsoft.AspNet.Identity.EntityFramework;

namespace PracticalWerewolf.Services
{
    public class UserInfoService : IUserInfoService
    {
        private readonly ContractorStore ContractorStore;
        private readonly UserStore<ApplicationUser> UserStore;

        public UserInfoService(ContractorStore ContractorStore, UserStore<ApplicationUser> UserStore)
        {
            this.ContractorStore = ContractorStore;
            this.UserStore = UserStore;
        }

        public ContractorInfo GetContractorInfo(Guid guid)
        {
            return ContractorStore.Single(c => c.ContractorInfoGuid == guid);
        }

        public IEnumerable<UserInfo> GetAllUsers()
        {
            throw new NotImplementedException();
        }

        public ContractorInfo GetUserContractorInfo(string id)
        {
            return UserStore.Users.Where(x => x.Id == id).Select(x => x.ContractorInfo).Single();
        }

        public CustomerInfo GetUserCustomerInfo(string id)
        {
            return UserStore.Users.Where(x => x.Id == id).Select(x => x.CustomerInfo).Single();
        }
    }
}