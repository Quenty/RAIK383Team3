using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PracticalWerewolf.Controllers.UnitOfWork
{
    public interface IUnitOfWork
    {

        void SaveChanges();
      
    }
}