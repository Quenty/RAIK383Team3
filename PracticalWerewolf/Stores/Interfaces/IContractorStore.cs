using PracticalWerewolf.Models;
using PracticalWerewolf.Models.UserInfos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PracticalWerewolf.Stores.Interfaces
{
    public interface IContractorStore
    {
        IEnumerable<ContractorInfo> GetUnapprovedContractorInfos();
    }
}
