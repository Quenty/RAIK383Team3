using System;
using PracticalWerewolf.Models;
using PracticalWerewolf.Models.UserInfos;
using PracticalWerewolf.Models.Orders;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PracticalWerewolf.Models.Trucks;

namespace PracticalWerewolf.Services.Interfaces
{
    public interface IContractorService
    {
        // Depends upon IContractorStore.Update
        void SetIsAvailable(Guid contractorInfoGuid, bool isAvailable);

        // Depends upon IContractorStore.Update
        void SetApproval(Guid contractorInfoGuid, ContractorApprovalState isApproved);

        IEnumerable<ContractorInfo> GetUnapprovedContractors();

        ContractorInfo GetContractorByTruckGuid(Guid guid);

        void UpdateContractorTruck(Truck truck, ApplicationUser user);
    }
}
