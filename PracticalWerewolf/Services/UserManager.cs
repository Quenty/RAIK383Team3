using PracticalWerewolf.Repository.Interfaces;
using PracticalWerewolf.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PracticalWerewolf.Services
{
    public class UserManager : IUserManager
    {
        private IUserStore userStore;

        public UserManager(IUserStore store)
        {
            userStore = store;
        }
    }
}