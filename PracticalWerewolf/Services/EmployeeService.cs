using PracticalWerewolf.Stores.Interfaces;
using PracticalWerewolf.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PracticalWerewolf.Models;

namespace PracticalWerewolf.Services
{
    public class EmployeeService : IEmployeeManager
    {
        private IEmployeeStore EmployeeStore;

        public EmployeeService(IEmployeeStore store)
        {
            EmployeeStore = store;
        }

        public void ApproveContractor()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IOrder> GetOrders()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IUserInfo> GetUser()
        {
            throw new NotImplementedException();
        }

        public void UpdateContractorInfo(IContractorInfo contractorInfo)
        {
            throw new NotImplementedException();
        }

        public void UpdateCustomerInfo(ICustomerInfo customerInfo)
        {
            throw new NotImplementedException();
        }

        public void UpdateEmployeeInfo(IEmployeeInfo employeeInfo)
        {
            throw new NotImplementedException();
        }

        public void UpdateOrderInfo(IOrder order)
        {
            throw new NotImplementedException();
        }

        public void UpdateTruckInfo(ITruck truck)
        {
            throw new NotImplementedException();
        }
    }
}