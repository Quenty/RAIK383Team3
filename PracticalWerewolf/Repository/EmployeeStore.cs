using PracticalWerewolf.Models;
using PracticalWerewolf.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PracticalWerewolf.Repository
{
    public class EmployeeStore : IEmployeeStore
    {
        private ApplicationDbContext Db;
        public EmployeeStore(ApplicationDbContext dbContext)
        {
            Db = dbContext;
        }

        public void Add(IEnumerable<IEmployeeInfo> employeeInfoList)
        {
            throw new NotImplementedException();
        }

        public void Add(IEmployeeInfo employeeInfo)
        {
            throw new NotImplementedException();
        }

        public void Delete(IEnumerable<IEmployeeInfo> employeeInfoList)
        {
            throw new NotImplementedException();
        }

        public void Delete(IEmployeeInfo employeeInfo)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IEmployeeInfo> Get()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IEmployeeInfo> Get(IEnumerable<Guid> guids)
        {
            throw new NotImplementedException();
        }

        public IEmployeeInfo Get(Guid guid)
        {
            throw new NotImplementedException();
        }

        public IApplicationUser GetUser(Guid guid)
        {
            throw new NotImplementedException();
        }

        public void Update(IEnumerable<IEmployeeInfo> employeeInfoList)
        {
            throw new NotImplementedException();
        }

        public void Update(IEmployeeInfo employeeInfo)
        {
            throw new NotImplementedException();
        }
    }
}