using PracticalWerewolf.Models;
using PracticalWerewolf.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PracticalWerewolf.Repository
{
    public class CustomerStore : ICustomerStore
    {
        private ApplicationDbContext Db;

        public CustomerStore(ApplicationDbContext dbContext)
        {
            Db = dbContext;
        }
    }
}