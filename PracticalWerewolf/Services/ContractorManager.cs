using PracticalWerewolf.Repository.Interfaces;
using PracticalWerewolf.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PracticalWerewolf.Services
{
    public class ContractorManager : IContractorManager
    {
        private IContractorStore ContractorStore;

        public ContractorManager(IContractorStore store)
        {
            ContractorStore = store;
        }
    }
}