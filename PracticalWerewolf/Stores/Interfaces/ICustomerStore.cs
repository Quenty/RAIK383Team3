using PracticalWerewolf.Models;
using PracticalWerewolf.Models.UserInfos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PracticalWerewolf.Stores.Interfaces
{
    public interface ICustomerStore : IEntityStore<CustomerInfo>
    {
        //Get all CustomerInfo
        IEnumerable<CustomerInfo> Get();

        //Get CustomerInfo by guid
        CustomerInfo Get(Guid guid);

        //Batch Get CustomerInfo by guid
        IEnumerable<CustomerInfo> Get(IEnumerable<Guid> guids);

        //Get User by CustomerInfo guid
        IApplicationUser GetUser(Guid guid);

        //Add CustomerInfo
        void Add(CustomerInfo customerInfo);

        //Batch Add CustomerInfo
        void Add(IEnumerable<CustomerInfo> customerInfoList);

        //Update CustomerInfo
        void Update(CustomerInfo customerInfo);

        //Batch Update CustomerInfo
        void Update(IEnumerable<CustomerInfo> customerInfoList);

        //Delete CustomerInfo
        void Delete(CustomerInfo customerInfo);

        //Batch DeleteContactorInfo
        void Delete(IEnumerable<CustomerInfo> customerInfoList);
    }
}
