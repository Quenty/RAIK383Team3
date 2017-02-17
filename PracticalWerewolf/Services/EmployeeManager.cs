using PracticalWerewolf.Repository.Interfaces;
using PracticalWerewolf.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PracticalWerewolf.Services
{
    public class EmployeeManager : UserManager, IEmployeeManager
    {
        private IEmployeeStore employeeStore;

        public EmployeeManager(IEmployeeStore store) : base(store) { }
    }
}