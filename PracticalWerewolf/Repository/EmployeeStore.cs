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
    }
}