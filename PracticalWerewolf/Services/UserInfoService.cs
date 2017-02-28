using PracticalWerewolf.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PracticalWerewolf.Models.UserInfos;

namespace PracticalWerewolf.Services
{
    public class UserInfoService : IUserInfoService
    {
        public IEnumerable<UserInfo> GetAllUsers()
        {
            throw new NotImplementedException();
        }
    }
}