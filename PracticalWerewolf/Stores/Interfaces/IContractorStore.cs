using PracticalWerewolf.Models;
using PracticalWerewolf.Models.Trucks;
using PracticalWerewolf.Models.UserInfos;
using PracticalWerewolf.Stores.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PracticalWerewolf.Stores.Interfaces
{
    public interface IContractorStore : IEntityStore<ContractorInfo>
    {
        void UpdateContractorTruck(ContractorInfo contractor, Truck truck);
        IQueryable<ContractorInfo> getAvailableContractorsQuery()
    }
}
