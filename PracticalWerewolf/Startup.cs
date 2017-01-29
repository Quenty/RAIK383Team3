using Microsoft.Owin;
using Owin;

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
