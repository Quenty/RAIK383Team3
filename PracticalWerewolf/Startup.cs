using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;
using PracticalWerewolf.Models;

[assembly: OwinStartupAttribute(typeof(PracticalWerewolf.Startup))]
namespace PracticalWerewolf
{
    public partial class Startup
    {

        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}