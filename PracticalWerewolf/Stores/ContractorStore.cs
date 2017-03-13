using PracticalWerewolf.Models;
using PracticalWerewolf.Models.UserInfos;
using PracticalWerewolf.Stores.Interfaces;
using PracticalWerewolf.Stores.Interfaces.Contexts;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace PracticalWerewolf.Stores
{
    public class ContractorStore : EntityStore<ContractorInfo>, IContractorStore
    {
        private IUserInfoDbContext context;

        public ContractorStore(IUserInfoDbContext userInfoDbContext) : base(userInfoDbContext.ContractorInfo)
        {
            context = userInfoDbContext;
        }
    }
}