using PracticalWerewolf.Stores.Interfaces;
using PracticalWerewolf.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PracticalWerewolf.Models;
using PracticalWerewolf.Models.Orders;
using PracticalWerewolf.Models.UserInfos;

namespace PracticalWerewolf.Services
{
    public class ContractorService : IContractorService
    {
        private IContractorStore ContractorStore;

        public ContractorService(IContractorStore store)
        {
            ContractorStore = store;
        }

        public void ChangeContractorStatus(bool status)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Order> GetCurrentOrders(Guid contractorGuid)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Order> GetPreviousOrders(Guid contractorGuid)
        {
            throw new NotImplementedException();
        }

        public void RejectOrder(Guid orderGuid)
        {
            throw new NotImplementedException();
        }

        public void UpdateOrderTrackInfo(OrderTrackInfo orderTrackInfo)
        {
            throw new NotImplementedException();
        }

        public CustomerInfo ViewCustomerInfo(Guid customerInfoGuid)
        {
            throw new NotImplementedException();
        }
    }
}