using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PracticalWerewolf.ViewModels.Paged
{
    public class PagedDataViewModel
    {
        public int Page { get; set; }
        public int TotalPages { get; set; }
        public int TotalResults { get; set; }
    }
}