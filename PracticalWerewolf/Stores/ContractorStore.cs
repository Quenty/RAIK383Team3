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

        public void Add(IEnumerable<ContractorInfo> contractorInfoList)
        {
            throw new NotImplementedException();
        }

        public void Add(ContractorInfo contractorInfo)
        {
            throw new NotImplementedException();
        }

        public void Delete(IEnumerable<ContractorInfo> contractorInfoList)
        {
            throw new NotImplementedException();
        }

        public void Delete(ContractorInfo contractorInfo)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ContractorInfo> Get()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ContractorInfo> Get(IEnumerable<Guid> guids)
        {
            throw new NotImplementedException();
        }

        public ContractorInfo Get(Guid guid)
        {
            throw new NotImplementedException();
        }

        public IApplicationUser GetUser(Guid guid)
        {
            throw new NotImplementedException();
        }

        public void Update(IEnumerable<ContractorInfo> contractorInfoList)
        {
            throw new NotImplementedException();
        }

        public void Update(ContractorInfo contractorInfo)
        {
            throw new NotImplementedException();
        }
    }
}