using PracticalWerewolf.Services.Interfaces;
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
        private readonly ApplicationUserManager UserManager;

        public SearchController(IUserInfoService UserInfoService, ApplicationUserManager UserManager)
        {
            this.UserInfoService = UserInfoService;
        }

        public ActionResult Results(string query, int page=0)
        {
            ViewBag.Query = query;
             
            List<UserSearchResult> userResults = new List<UserSearchResult>();
            SearchResult result = UserInfoService.Search(query, page);

            userResults.AddRange(result.Users.Select(user => new UserSearchResult {
                Id = user.Id,
                Email = user.Email,
                IsContractor = user.ContractorInfo != null,
                IsEmployee = user.EmployeeInfo != null,
                BanTime = (user.LockoutEnabled && user.LockoutEndDateUtc > DateTime.Now) ? (user.LockoutEndDateUtc - DateTime.Now) : null
            }));


            SearchResultViewModel model = new SearchResultViewModel
            {
                Query = query,
                Page = result.Page,
                TotalPages = result.TotalPages,
                Users = userResults
            };

            return View(model);
        }
    }
}