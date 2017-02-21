using PracticalWerewolf.Models;
using PracticalWerewolf.Models.UserInfos;
using PracticalWerewolf.Stores.Interfaces;
using PracticalWerewolf.Stores.Interfaces.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PracticalWerewolf.Stores
{
    public class EmployeeStore : IEmployeeStore
    {
        private IUserInfoDbContext context;
        public EmployeeStore(IUserInfoDbContext userInfoDbContext)
        {
            context = userInfoDbContext;
        }

        public void Add(IEnumerable<EmployeeInfo> employeeInfoList)
        {
            throw new NotImplementedException();
        }

        public void Add(EmployeeInfo employeeInfo)
        {
            throw new NotImplementedException();
        }

        public void Delete(IEnumerable<EmployeeInfo> employeeInfoList)
        {
            throw new NotImplementedException();
        }

        public void Delete(EmployeeInfo employeeInfo)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<EmployeeInfo> Get()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<EmployeeInfo> Get(IEnumerable<Guid> guids)
        {
            throw new NotImplementedException();
        }

        public EmployeeInfo Get(Guid guid)
        {
            throw new NotImplementedException();
        }

        public IApplicationUser GetUser(Guid guid)
        {
            throw new NotImplementedException();
        }

        public void Update(IEnumerable<EmployeeInfo> employeeInfoList)
        {
            throw new NotImplementedException();
        }

        public void Update(EmployeeInfo employeeInfo)
        {
            throw new NotImplementedException();
        }
    }
}