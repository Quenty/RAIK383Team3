using Microsoft.AspNet.Identity;
using Moq;
using PracticalWerewolf.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace PracticalWerewolf.Tests.Controllers
{
    public abstract class ControllerTest
    {
        public static Mock<ApplicationUserManager> GetMockApplicationUserManager()
        {
            var userStore = new Mock<IUserStore<ApplicationUser>>();
            return new Mock<ApplicationUserManager>(userStore.Object);
        }

        public static Mock<ApplicationUserManager> GetMockApplicationUserManager(DbSet<ApplicationUser> dbSet)
        {
            var userStore = new Mock<IUserStore<ApplicationUser>>();
            userStore.Setup(x => x.FindByNameAsync(It.IsAny<string>()))
                .Returns<string>(
                (s) => {
                    return Task.FromResult(dbSet.SingleOrDefault(a => a.UserName.Equals(s)));
                } );
            var userManager = new Mock<ApplicationUserManager>(userStore.Object);
            userManager.Setup(x => x.Users).Returns(dbSet);
            return userManager;
        }

        //Use this one if you are calling ApplicationUserManager.FindByName(). For some reason we can't mock that.
        public static ApplicationUserManager GetApplicationUserManager(DbSet<ApplicationUser> dbSet)
        {
            var userStore = new Mock<IUserStore<ApplicationUser>>();
            userStore.Setup(x => x.FindByNameAsync(It.IsAny<string>()))
               .Returns<string>(
               (s) => {
                   return Task.FromResult(dbSet.SingleOrDefault(a => a.UserName.Equals(s)));
               });

            return new ApplicationUserManager(userStore.Object);
        }

        //This is the only way I could get mocking the user to work (getting the name, id, email, etc)
        public static GenericPrincipal GetMockUser(string id, List<Claim> claimsToAdd = null)
        {
            var identity = new GenericIdentity(id);
            List<Claim> claims = new List<Claim>(){
                    new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name", id),
                    new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", id)
            };

            if (claimsToAdd != null)
            {
                claims.AddRange(claimsToAdd);
            }
            
            identity.AddClaims(claims);
            var principal = new GenericPrincipal(identity, new string[0]);

            return principal;
        }

        public static IPrincipal GetMockUserNullId()
        {
            var identity = new Mock<IIdentity>();
            var user = new Mock<IPrincipal>();
            user.Setup(x => x.Identity).Returns(identity.Object);

            return user.Object;
        }

        public static ControllerContext GetMockControllerContext(IPrincipal principal)
        {
            var httpContext = new Mock<HttpContextBase>();
            httpContext.Setup(x => x.User).Returns(principal);
            var context = new Mock<ControllerContext>();
            context.Setup(x => x.HttpContext).Returns(httpContext.Object);

            return context.Object;
        }
    }
}
