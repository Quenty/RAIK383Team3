using PracticalWerewolf.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PracticalWerewolf.Repository.Interfaces
{
    public interface IContractorStore
    {
        //Get all ContractorInfo
        List<IContractorInfo> Get();

        //Get ContractorInfo by guid
        IContractorInfo Get(Guid guid);



    }
}
