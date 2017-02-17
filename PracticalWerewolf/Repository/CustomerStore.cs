using PracticalWerewolf.Models;
using PracticalWerewolf.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PracticalWerewolf.Repository
{
    public class CustomerStore : UserStore, ICustomerStore
    {
        public CustomerStore(ApplicationDbContext dbContext) : base(dbContext) { }
    }
}