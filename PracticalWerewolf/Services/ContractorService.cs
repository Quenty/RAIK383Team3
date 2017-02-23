﻿using PracticalWerewolf.Stores.Interfaces;
using PracticalWerewolf.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PracticalWerewolf.Services
{
    public class ContractorService : IContractorService
    {
        private IContractorStore ContractorStore;

        public ContractorService(IContractorStore store)
        {
            ContractorStore = store;
        }
    }
}