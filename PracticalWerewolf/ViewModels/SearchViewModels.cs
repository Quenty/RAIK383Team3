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
        public Boolean IsContractor { get; set; }
        public Boolean IsEmployee { get; set; }
        public TimeSpan? BanTime { get; set; } = null;
    }

    public class SearchResultViewModel
    {
        public string Query { get; set; }
        public int Page { get; set; }
        public int TotalPages { get; set; }
        public IEnumerable<UserSearchResult> Users { get; set; }
    }
}