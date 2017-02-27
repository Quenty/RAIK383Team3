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
        //Get all ContractorInfo
        IEnumerable<ContractorInfo> Get();

        //Get ContractorInfo by guid
        ContractorInfo Get(Guid guid);

        //Batch Get ContractorInfo by guid
        IEnumerable<ContractorInfo> Get(IEnumerable<Guid> guids);

        //Get User by ContractorInfo guid
        IApplicationUser GetUser(Guid guid);

        //Add ContractorInfo
        void Add(ContractorInfo contractorInfo);

        //Batch Add ContractorInfo
        void Add(IEnumerable<ContractorInfo> contractorInfoList);

        //Update ContractorInfo
        void Update(ContractorInfo contractorInfo);

        //Batch Update ContractorInfo
        void Update(IEnumerable<ContractorInfo> contractorInfoList);

        //Delete ContractorInfo
        void Delete(ContractorInfo contractorInfo);

        //Batch DeleteContactorInfo
        void Delete(IEnumerable<ContractorInfo> contractorInfoList);
    }
}
