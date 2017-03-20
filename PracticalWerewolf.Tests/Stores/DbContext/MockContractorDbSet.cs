using PracticalWerewolf.Models.UserInfos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PracticalWerewolf.Tests.Stores.DbContext
{
    public class MockContractorDbSet : MockDbSet<ContractorInfo>
    {
        public override ContractorInfo Find(params object[] keyValues)
        {
            foreach (var contractorInfo in _data)
            {
                if (keyValues.Contains(contractorInfo.ContractorInfoGuid))
                {
                    return contractorInfo;
                }
            }
            return null;
        }
    }
}
