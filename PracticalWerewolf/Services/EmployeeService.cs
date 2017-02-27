using PracticalWerewolf.Stores.Interfaces;
using PracticalWerewolf.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PracticalWerewolf.Services
{
    public class EmployeeService : IEmployeeManager
    {
        private IEmployeeStore EmployeeStore;

        public EmployeeService(IEmployeeStore store)
        {
            EmployeeStore = store;
        }
    }
}