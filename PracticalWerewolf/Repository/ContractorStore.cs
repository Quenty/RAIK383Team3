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

        public void Add(IEnumerable<IContractorInfo> contractorInfoList)
        {
            throw new NotImplementedException();
        }

        public void Add(IContractorInfo contractorInfo)
        {
            throw new NotImplementedException();
        }

        public void Delete(IEnumerable<IContractorInfo> contractorInfoList)
        {
            throw new NotImplementedException();
        }

        public void Delete(IContractorInfo contractorInfo)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IContractorInfo> Get()
        {
            throw new NotImplementedException();
        }

        public IContractorInfo Get(Guid guid)
        {
            throw new NotImplementedException();
        }

        public IApplicationUser GetUser(Guid guid)
        {
            throw new NotImplementedException();
        }

        public void Update(IEnumerable<IContractorInfo> contractorInfoList)
        {
            throw new NotImplementedException();
        }

        public void Update(IContractorInfo contractorInfo)
        {
            throw new NotImplementedException();
        }
    }
}