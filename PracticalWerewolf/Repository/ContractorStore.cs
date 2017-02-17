using PracticalWerewolf.Models;
using PracticalWerewolf.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PracticalWerewolf.Repository
{
    public class ContractorStore : UserStore, IContractorStore
    {
        public ContractorStore(ApplicationDbContext dbContext) : base(dbContext){ }
    }
}