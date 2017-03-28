using PracticalWerewolf.Models.UserInfos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PracticalWerewolf.Services.Interfaces
{
    public interface IUserInfoService
    {
        // Depends upon ApplicationUserManager
        IEnumerable<UserInfo> GetAllUsers();
    }
}
