using System;

namespace PracticalWerewolf.Models
{
    public interface IContractorInfo
    {
        ITruck Truck { get; set; }
        Boolean IsAvailable { get; set; }
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
}
