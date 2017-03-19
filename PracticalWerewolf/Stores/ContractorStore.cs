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

        private readonly ApplicationDbContext context;

        public ContractorStore(ApplicationDbContext context) : base(context.ContractorInfo)
        {
            this.context = context;
        }

        public void UpdateContractorTruck(ContractorInfo contractor, Truck truck)
        {
            context.Entry(contractor).State = EntityState.Modified;
            contractor.Truck = truck;
            
        }
    }
}