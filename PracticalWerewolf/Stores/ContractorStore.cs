using PracticalWerewolf.Application;
using PracticalWerewolf.Models;
using PracticalWerewolf.Models.Trucks;
using PracticalWerewolf.Models.UserInfos;
using PracticalWerewolf.Stores.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace PracticalWerewolf.Stores
{
    public class ContractorStore : EntityStore<ContractorInfo>, IContractorStore
    {
        public ContractorStore(IDbSetFactory context) : base(context)
        {
        }

        ContractorInfo IEntityStore<ContractorInfo>.Single(System.Linq.Expressions.Expression<Func<ContractorInfo, bool>> where, params System.Linq.Expressions.Expression<Func<ContractorInfo, object>>[] includeProperties)
        {
            return base.Single(where, includeProperties);
        }

        void IEntityStore<ContractorInfo>.Update(ContractorInfo entity)
        {
            base.Update(entity);
        }

        public void UpdateContractorTruck(ContractorInfo contractor, Truck truck)
        {
            base.Update(contractor);
            contractor.Truck = truck;
        }

        public IQueryable<ContractorInfo> getAvailableContractorsQuery()
        {
            return Find(c => c.ApprovalState == ContractorApprovalState.Approved).AsQueryable()
                .Where(c => c.IsAvailable)
                .Where(c => c.Truck.AvailableCapacity.Volume > 0)
                .Where(c => c.Truck.AvailableCapacity.Mass > 0);
        }
    }
}