using PracticalWerewolf.Services.Interfaces;
using PracticalWerewolf.ViewModels.Paged;
using PracticalWerewolf.ViewModels.Search;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace PracticalWerewolf.Controllers
{
    [RequireHttps]
    public class SearchController : Controller
    {
        private readonly IUserInfoService UserInfoService;

        public SearchController(IUserInfoService UserInfoService, ApplicationUserManager UserManager)
        {
            this.UserInfoService = UserInfoService;
        }

        private SearchResultViewModel GetSearchResults(string query, int page = 0)
        {
            ViewBag.Query = query;

            List<UserSearchResult> userResults = new List<UserSearchResult>();
            SearchResult result = UserInfoService.Search(query, page);

            userResults.AddRange(result.Users.Select(user => new UserSearchResult
            {
                Id = user.Id,
                Email = user.Email,
                IsContractor = user.ContractorInfo != null,
                IsEmployee = user.EmployeeInfo != null,
                BanTime = (user.LockoutEnabled && user.LockoutEndDateUtc > DateTime.UtcNow) ? (user.LockoutEndDateUtc - DateTime.UtcNow) : null
            }));


            SearchResultViewModel model = new SearchResultViewModel
            {
                Query = query,
                Users = userResults,
                PagedData = new PagedDataViewModel
                {
                    Page = result.Page,
                    TotalPages = result.TotalPages,
                    TotalResults = result.TotalResults
                },
            };

            return model;
        }

        public ActionResult PartialResults(string query, int page = 0)
        {
            return PartialView("_PartialResults", GetSearchResults(query, page));
        }

        public ActionResult Results(string query, int page = 0)
        {
            return View("Results", GetSearchResults(query, page));
        }
    }
}