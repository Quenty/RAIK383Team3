using PracticalWerewolf.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PracticalWerewolf.Models.UserInfos;
using PracticalWerewolf.Stores;
using PracticalWerewolf.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using PracticalWerewolf.Utility;

namespace PracticalWerewolf.Services
{
    public class UserInfoService : IUserInfoService
    {
   
        private readonly ContractorStore ContractorStore;
        private readonly UserStore<ApplicationUser> UserStore;
        private readonly int PAGE_SIZE = 10;

        public UserInfoService(ContractorStore ContractorStore, UserStore<ApplicationUser> UserStore)
        {
            this.ContractorStore = ContractorStore;
            this.UserStore = UserStore;
        }

        public ContractorInfo GetContractorInfo(Guid guid)
        {
            return ContractorStore.Single(c => c.ContractorInfoGuid == guid);
        }

        public IEnumerable<ApplicationUser> GetAllUsers(int page)
        {
            return UserStore.Users.AsQueryable().Skip(PAGE_SIZE * page).Take(PAGE_SIZE).ToList();
        }

        public ContractorInfo GetUserContractorInfo(string id)
        {
            return UserStore.Users.Where(x => x.Id == id).Select(x => x.ContractorInfo).Single();
        }

        public CustomerInfo GetUserCustomerInfo(string id)
        {
            return UserStore.Users.Where(x => x.Id == id).Select(x => x.CustomerInfo).Single();
        }



        public SearchResult Search(string query, int page)
        {
           
            if (String.IsNullOrEmpty(query))
            {
                var users = UserStore.Users.AsQueryable();

                return new SearchResult
                {
                    Page = page,
                    TotalPages = (int)(users.Count() / PAGE_SIZE) + 1,
                    Users = users.OrderBy(x => x.Email.ToLower()).Skip(PAGE_SIZE * page).Take(PAGE_SIZE).ToList()
                };
            }
            else
            {
                Levenshtein levenshtein = new Levenshtein(query.ToLower());
                IEnumerable<ApplicationUser> users = UserStore.Users.AsEnumerable()
                    .OrderBy(x => levenshtein.Score(x.Email.ToLower()));

                return new SearchResult
                {
                    Page = page,
                    TotalPages = (int) (users.Count() / PAGE_SIZE) + 1,
                    Users = users.Skip(PAGE_SIZE * page)
                        .Take(PAGE_SIZE)
                        .ToList()
                };
            }
        }
    }
}