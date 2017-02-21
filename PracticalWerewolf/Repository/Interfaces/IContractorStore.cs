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
        IEnumerable<IContractorInfo> Get();

        //Get ContractorInfo by guid
        IContractorInfo Get(Guid guid);

        //Batch Get ContractorInfo by guid
        IEnumerable<IContractorInfo> Get(IEnumerable<Guid> guids);

        //Get User by ContractorInfo guid
        IApplicationUser GetUser(Guid guid);

        //Add ContractorInfo
        void Add(IContractorInfo contractorInfo);

        //Batch Add ContractorInfo
        void Add(IEnumerable<IContractorInfo> contractorInfoList);

        //Update ContractorInfo
        void Update(IContractorInfo contractorInfo);

        //Batch Update ContractorInfo
        void Update(IEnumerable<IContractorInfo> contractorInfoList);

        //Delete ContractorInfo
        void Delete(IContractorInfo contractorInfo);

        //Batch DeleteContactorInfo
        void Delete(IEnumerable<IContractorInfo> contractorInfoList);
    }
}
