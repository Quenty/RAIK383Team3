using PracticalWerewolf.Models;
using PracticalWerewolf.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PracticalWerewolf.Repository
{
    public class UserStore : IUserStore
    {
        private ApplicationDbContext db;

        public UserStore(ApplicationDbContext dbContext)
        {
            this.db = dbContext;
        }
    }
}