using Hangfire;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace PracticalWerewolf
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private BackgroundJobServer backgroundJobServer;

        protected void Application_Start()
        {
            SqlServerTypes.Utilities.LoadNativeAssemblies(Server.MapPath("~/bin"));
            

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            log4net.Config.XmlConfigurator.Configure();

            GlobalConfiguration.Configuration.UseLog4NetLogProvider()
                .UseNinjectActivator(new Ninject.Web.Common.Bootstrapper().Kernel)
                .UseSqlServerStorage(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);

            backgroundJobServer = new BackgroundJobServer();
        }
    }
}
