using System;

namespace PracticalWerewolf.Models
{
    public interface IContractorInfo
    {
    }

    public interface ICustomerInfo
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
