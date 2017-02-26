using System;
using PracticalWerewolf.Models.Orders;
using PracticalWerewolf.Models.UserInfos;
using PracticalWerewolf.Models.Trucks;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PracticalWerewolf.Services.Interfaces
{
    interface IEmployeeService
    {
        //See all orders
        IEnumerable<Order> ViewAllOrders();

        //See all orders
        IEnumerable<Order> SearchOrders(string query);

        //Update EmployeeInfo
        void UpdateEmployeeInfo(EmployeeInfo employeeInfo);

        //Update CustomerInfo
        void UpdateCustomerInfo(CustomerInfo customerInfo);

        //Update ContractorInfo
        void UpdateContractorInfo(ContractorInfo contractorInfo);

        //Update OrderInfo
        void UpdateOrderInfo(Order order);

        //UpdateTruckInfo
        void UpdateTruckInfo(Truck truck);

        //Approve contractor
        void ApproveContractor(Guid contractorInfoGuid);

        //View users
        IEnumerable<UserInfo> ViewUsers();

        //Search users
        //Should this be in js?
        IEnumerable<UserInfo> SearchUsers(string Query);
    }
}
