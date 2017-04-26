using PracticalWerewolf.Models.UserInfos;
using PracticalWerewolf.ViewModels.Paged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PracticalWerewolf.ViewModels.Search
{
    public class UserSearchResult
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public Boolean IsEmployee { get; set; }

        public TimeSpan? BanTime { get; set; } = null;

        public Boolean IsContractor { get; set; }
        public Guid? TruckGuid { get; set; }
        public ContractorApprovalState? ContractorApprovalState { get; set; }
    }

    public class SearchResultViewModel
    {
        public string Query { get; set; }
        public PagedDataViewModel PagedData { get; set; }
        public IEnumerable<UserSearchResult> Users { get; set; }
    }
}