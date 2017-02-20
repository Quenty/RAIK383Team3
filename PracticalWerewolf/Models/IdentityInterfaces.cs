using System;

namespace PracticalWerewolf.Models
{
    public interface IApplicationUser
    {
        IContractorInfo ContractorInfo { get; set; } 
        ICustomerInfo CustomerInfo { get; set; }
        IEmployeeInfo EmployeeInfo { get; set; }
        IUserInfo UserInfo { get; set; }
    }

    public interface IContractorInfo
    {
        ITruck Truck { get; set; }
        Boolean isAvailable { get; set; }
    }

    public interface ICustomerInfo
    {
        IBillingInfo BillingInfo { get; set; }
    }

    public interface IBillingInfo
    {
    }

    public interface IEmployeeInfo
    {
        
    }
    public interface IUserInfo
    {
        string FirstName { get; set; }
        string LastName { get; set; }        
    }

    public interface IPermission
    {
        Boolean Read { get; set; }
        Boolean Write { get; set; }
    }
}
