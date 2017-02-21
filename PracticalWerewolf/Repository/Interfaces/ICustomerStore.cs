using PracticalWerewolf.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PracticalWerewolf.Repository.Interfaces
{
    public interface ICustomerStore
    {
        //Get all CustomerInfo
        IEnumerable<ICustomerInfo> Get();

        //Get CustomerInfo by guid
        ICustomerInfo Get(Guid guid);

        //Batch Get CustomerInfo by guid
        IEnumerable<ICustomerInfo> Get(IEnumerable<Guid> guids);

        //Get User by CustomerInfo guid
        IApplicationUser GetUser(Guid guid);

        //Add CustomerInfo
        void Add(ICustomerInfo customerInfo);

        //Batch Add CustomerInfo
        void Add(IEnumerable<ICustomerInfo> customerInfoList);

        //Update CustomerInfo
        void Update(ICustomerInfo customerInfo);

        //Batch Update CustomerInfo
        void Update(IEnumerable<ICustomerInfo> customerInfoList);

        //Delete CustomerInfo
        void Delete(ICustomerInfo customerInfo);

        //Batch DeleteContactorInfo
        void Delete(IEnumerable<ICustomerInfo> customerInfoList);
    }
}
