using PracticalWerewolf.Repository.Interfaces;
using PracticalWerewolf.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PracticalWerewolf.Models;

namespace PracticalWerewolf.Services
{
    public class ContractorManager : IContractorManager
    {
        private IContractorStore ContractorStore;

        public ContractorManager(IContractorStore store)
        {
            ContractorStore = store;
        }

        public IEnumerable<IOrder> GetCurrentOrderInfo()
        {
            throw new NotImplementedException();
        }

        public ICustomerInfo GetICustomerInfo()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IOrder> GetPreviousOrderInfo()
        {
            throw new NotImplementedException();
        }

        public void RejectOrder()
        {
            throw new NotImplementedException();
        }

        public void UpdateOrderTrackInfo(IOrderTrackInfo orderTrackInfo)
        {
            throw new NotImplementedException();
        }

        public void UpdateStatus(bool status)
        {
            throw new NotImplementedException();
        }
    }
}