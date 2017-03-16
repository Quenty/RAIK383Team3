using PracticalWerewolf.Models;
using PracticalWerewolf.Models.UserInfos;
using PracticalWerewolf.Stores.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PracticalWerewolf.Stores
{
    public class ContractorStore : IContractorStore
    {
        private ApplicationDbContext context;

        public ContractorStore(ApplicationDbContext context)
        {
            this.context = context;
        }

        public IEnumerable<ContractorInfo> GetUnapprovedContractorInfos()
        {
            return context.ContractorInfo.Where(c => !c.IsApproved);
        }
    }
}