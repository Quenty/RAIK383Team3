using System;
using PracticalWerewolf.Models;
using PracticalWerewolf.Models.UserInfos;
using PracticalWerewolf.Models.Orders;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PracticalWerewolf.Services.Interfaces
{
    public interface IContractorService
    {
        // Depends upon IContractorStore.Update
        void SetIsAvailable(Guid contractorInfoGuid, bool isAvailable);

        // Depends upon IContractorStore.Update
        void SetIsApproved(Guid contractorInfoGuid, bool isApproved);
        void UpdateContractorApproval(Guid contractorInfoGuid);

        // Depends upon the IContractorStore.GetUser
        void GetUserContractorInfo(Guid userId);

        // Depends upon the IContractorStore.Create
        void RegisterContractorInfo(Guid userId, ContractorInfo newContractorInfo);
        IEnumerable<UserInfo> GetAllContractors();
    }
}
