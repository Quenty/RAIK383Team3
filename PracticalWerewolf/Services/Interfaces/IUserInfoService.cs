using PracticalWerewolf.Models;
using PracticalWerewolf.Models.UserInfos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PracticalWerewolf.Services.Interfaces
{
    public class SearchResult
    {
        public int TotalResults { get; set; }
        public int TotalPages { get; set; }
        public int Page { get; set; }
        public IEnumerable<ApplicationUser> Users { get; set; }
    }

    public interface IUserInfoService
    {
        // Depends upon ApplicationUserManager
        IEnumerable<ApplicationUser> GetAllUsers(int page = 0);
        SearchResult Search(string Query, int page = 0);
        ContractorInfo GetContractorInfo(Guid guid);

        CustomerInfo GetUserCustomerInfo(string id);
        ContractorInfo GetUserContractorInfo(string id);
    }

}
