using PracticalWerewolf.Stores.Interfaces;
using PracticalWerewolf.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PracticalWerewolf.Models;
using PracticalWerewolf.Models.Orders;
using PracticalWerewolf.Models.Trucks;
using PracticalWerewolf.Models.UserInfos;

namespace PracticalWerewolf.Services
{
    public class EmployeeService : IEmployeeService
    {
        private IEmployeeStore EmployeeStore;

        public EmployeeService(IEmployeeStore store)
        {
            EmployeeStore = store;
        }

        public void ApproveContractor(Guid contractorInfoGuid)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Order> SearchOrders(string query)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<UserInfo> SearchUsers(string Query)
        {
            throw new NotImplementedException();
        }

        public void UpdateContractorInfo(ContractorInfo contractorInfo)
        {
            throw new NotImplementedException();
        }

        public void UpdateCustomerInfo(CustomerInfo customerInfo)
        {
            throw new NotImplementedException();
        }

        public void UpdateEmployeeInfo(EmployeeInfo employeeInfo)
        {
            throw new NotImplementedException();
        }

        public void UpdateOrderInfo(Order order)
        {
            throw new NotImplementedException();
        }

        public void UpdateTruckInfo(Truck truck)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Order> ViewAllOrders()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<UserInfo> ViewUsers()
        {
            throw new NotImplementedException();
        }
    }
}