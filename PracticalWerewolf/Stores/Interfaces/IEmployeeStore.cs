using PracticalWerewolf.Models;
using PracticalWerewolf.Models.UserInfos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PracticalWerewolf.Stores.Interfaces
{
    public interface IEmployeeStore
    {
        //Get all EmployeeInfo
        IEnumerable<EmployeeInfo> Get();

        //Get EmployeeInfo by guid
        EmployeeInfo Get(Guid guid);

        //Batch Get EmployeeInfo by guid
        IEnumerable<EmployeeInfo> Get(IEnumerable<Guid> guids);

        //Get User by EmployeeInfo guid
        IApplicationUser GetUser(Guid guid);

        //Add EmployeeInfo
        void Add(EmployeeInfo employeeInfo);

        //Batch Add EmployeeInfo
        void Add(IEnumerable<EmployeeInfo> employeeInfoList);

        //Update EmployeeInfo
        void Update(EmployeeInfo employeeInfo);

        //Batch Update EmployeeInfo
        void Update(IEnumerable<EmployeeInfo> employeeInfoList);

        //Delete EmployeeInfo
        void Delete(EmployeeInfo employeeInfo);

        //Batch DeleteContactorInfo
        void Delete(IEnumerable<EmployeeInfo> employeeInfoList);
    }
}
