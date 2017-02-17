using PracticalWerewolf.Repository.Interfaces;
using PracticalWerewolf.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PracticalWerewolf.Services
{
    public class ContractorManager : UserManager, IContractorManager
    {
        private IContractorStore contractorStore;

        public ContractorManager(IContractorStore store) : base(store)
        {
            contractorStore = store;
        }
    }
}