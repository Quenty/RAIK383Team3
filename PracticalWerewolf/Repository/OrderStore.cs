using PracticalWerewolf.Models;
using PracticalWerewolf.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PracticalWerewolf.Repository
{
    public class OrderStore : IOrderStore
    {
        private ApplicationDbContext db;

        public OrderStore(ApplicationDbContext dbContext)
        {
            this.db = dbContext;
        }
    }
}