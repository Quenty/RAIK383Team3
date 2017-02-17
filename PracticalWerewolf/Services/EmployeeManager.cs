using PracticalWerewolf.Repository.Interfaces;
using PracticalWerewolf.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PracticalWerewolf.Services
{
    public class EmployeeManager : IEmployeeManager
    {
        private IEmployeeStore EmployeeStore;

        public EmployeeManager(IEmployeeStore store)
        {
            EmployeeStore = store;
        }
    }
}