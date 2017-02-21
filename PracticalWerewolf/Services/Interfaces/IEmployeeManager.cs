using System;
using PracticalWerewolf.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PracticalWerewolf.Services.Interfaces
{
    interface IEmployeeManager
    {
        //See all orders
        IEnumerable<IOrder> GetOrders();

        //Update EmployeeInfo
        void UpdateEmployeeInfo(IEmployeeInfo employeeInfo);

        //Update CustomerInfo
        void UpdateCustomerInfo(ICustomerInfo customerInfo);

        //Update ContractorInfo
        void UpdateContractorInfo(IContractorInfo contractorInfo);

        //Update OrderInfo
        void UpdateOrderInfo(IOrder order);

        //UpdateTruckInfo
        void UpdateTruckInfo(ITruck truck);

        //Approve contractor
        void ApproveContractor();

        //Search for users
        IEnumerable<IUserInfo> GetUser();

    }
}
