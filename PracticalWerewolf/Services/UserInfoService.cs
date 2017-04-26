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
using NinjaNye.SearchExtensions;
using System.Linq.Expressions;

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
                    TotalPages = (int) Math.Ceiling((Decimal)users.Count() / PAGE_SIZE),
                    Users = users.OrderBy(x => x.Email.ToLower()).Skip(PAGE_SIZE * page).Take(PAGE_SIZE).ToList()
                };
            }
            else
            {
                var users = UserStore.Users.AsQueryable().Search(
                        x => x.Email, 
                        x => x.Id, 
                        x => x.UserInfo.FirstName, 
                        x => x.UserInfo.LastName,
                        x => x.ContractorInfo.HomeAddress.RawInputAddress,
                        x => (x.EmployeeInfo == null) ? "" : "employee",
                        x => (x.ContractorInfo == null) ? "" : "contractor",
                        x => (x.ContractorInfo == null) ? "" : (x.ContractorInfo.Truck == null) ? "" : "has:truck",
                        x => (x.ContractorInfo == null) ? ""
                            : x.ContractorInfo.ApprovalState == ContractorApprovalState.Approved ? "status:approved"
                            : x.ContractorInfo.ApprovalState == ContractorApprovalState.Denied ? "status:denied"
                            : x.ContractorInfo.ApprovalState == ContractorApprovalState.Pending ? "status:pending status:pending-approval pending approval"
                            : "",
                        x => (x.LockoutEnabled && x.LockoutEndDateUtc > DateTime.UtcNow) ? "banned" : "",
                        x => x.ContractorInfo.DriversLicenseId)
                    .Containing(query.Split(' ', ':', '!', '@', '.', ','))
                    .ToRanked()
                    .OrderByDescending(r => r.Hits)
                    .Select(x => x.Item);

                return new SearchResult
                {
                    Page = page,
                    TotalResults = users.Count(),
                    TotalPages = (int) Math.Ceiling((Decimal) users.Count() / PAGE_SIZE),
                    Users = users.Skip(PAGE_SIZE * page).Take(PAGE_SIZE).ToList(),
                };
            }
        }

        public int QueryCount(Expression<Func<ApplicationUser, bool>> where)
        {
            return UserStore.Users.Where(where).Count();
        }
    }
}