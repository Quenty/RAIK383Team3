using PracticalWerewolf.Models;
using PracticalWerewolf.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PracticalWerewolf.Repository
{
    public class TruckStore : ITruckStore
    {
        private ApplicationDbContext db;

        public TruckStore(ApplicationDbContext dbContext)
        {
            this.db = dbContext;
        }
    }
}