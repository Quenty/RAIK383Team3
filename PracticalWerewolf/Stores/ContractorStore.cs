using PracticalWerewolf.Models;
using PracticalWerewolf.Models.UserInfos;
using PracticalWerewolf.Stores.Interfaces;
using PracticalWerewolf.Stores.Interfaces.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PracticalWerewolf.Stores
{
    public class ContractorStore : IContractorStore
    {
        private IUserInfoDbContext context;

        public ContractorStore(IUserInfoDbContext userInfoDbContext)
        {
            context = userInfoDbContext;
        }

        public IEnumerable<ContractorInfo> GetUnapprovedContractorInfos()
        {
            return context.ContractorInfo.Where(c => !c.IsApproved);
        }
    }
}