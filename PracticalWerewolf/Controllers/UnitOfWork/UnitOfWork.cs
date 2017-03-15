using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PracticalWerewolf.Models;

namespace PracticalWerewolf.Controllers.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext context;

        public void Commit()
        {
            context.SaveChanges();
        }
    }
}