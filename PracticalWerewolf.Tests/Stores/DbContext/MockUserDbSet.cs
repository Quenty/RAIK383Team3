using PracticalWerewolf.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PracticalWerewolf.Tests.Stores.DbContext
{
    class MockUserDbSet : MockDbSet<ApplicationUser>
    {
        public override ApplicationUser Find(params object[] keyValues)
        {
            foreach (var user in _data)
            {
                if (keyValues.Contains(user.Email))
                {
                    return user;
                }
            }
            return null;
        }
    }
}
