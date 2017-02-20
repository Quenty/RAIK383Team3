using PracticalWerewolf.Models;
using PracticalWerewolf.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PracticalWerewolf.Repository
{
    public class ContractorStore : IContractorStore
    {
        private ApplicationDbContext Db;
        public ContractorStore(ApplicationDbContext dbContext)
        {
            Db = dbContext;
        }
    }
}