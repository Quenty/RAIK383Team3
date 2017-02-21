using PracticalWerewolf.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PracticalWerewolf.Repository.Interfaces
{
    public interface IEmployeeStore
    {
        //Get all EmployeeInfo
        IEnumerable<IEmployeeInfo> Get();

        //Get EmployeeInfo by guid
        IEmployeeInfo Get(Guid guid);

        //Batch Get EmployeeInfo by guid
        IEnumerable<IEmployeeInfo> Get(IEnumerable<Guid> guids);

        //Get User by EmployeeInfo guid
        IApplicationUser GetUser(Guid guid);

        //Add EmployeeInfo
        void Add(IEmployeeInfo employeeInfo);

        //Batch Add EmployeeInfo
        void Add(IEnumerable<IEmployeeInfo> employeeInfoList);

        //Update EmployeeInfo
        void Update(IEmployeeInfo employeeInfo);

        //Batch Update EmployeeInfo
        void Update(IEnumerable<IEmployeeInfo> employeeInfoList);

        //Delete EmployeeInfo
        void Delete(IEmployeeInfo employeeInfo);

        //Batch DeleteContactorInfo
        void Delete(IEnumerable<IEmployeeInfo> employeeInfoList);
    }
}
