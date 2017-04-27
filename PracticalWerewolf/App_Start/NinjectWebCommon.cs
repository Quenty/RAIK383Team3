[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(PracticalWerewolf.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(PracticalWerewolf.App_Start.NinjectWebCommon), "Stop")]

namespace PracticalWerewolf.App_Start
{
    using System;
    using System.Web;

    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;
    using Models;
    using Controllers.UnitOfWork;
    using Stores.Interfaces;
    using Stores;
    using Services.Interfaces;
    using Services;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Microsoft.AspNet.Identity.Owin;
    using System.Data.Entity;
    using Application;
    using System.Reflection;
    using Microsoft.Owin.Security;
    using Microsoft.Owin.Security.DataProtection;
    using Hangfire;
    using System.Linq;
    using Helpers.Interfaces;
    using Helpers;
    public static class NinjectWebCommon 
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start() 
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }
        
        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }
        
        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

                RegisterServices(kernel);
                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }


        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            //Stores
            kernel.Bind<IContractorStore>().To<ContractorStore>();
            kernel.Bind<ICustomerStore>().To<CustomerStore>();
            kernel.Bind<IEmployeeStore>().To<EmployeeStore>();
            kernel.Bind<IOrderStore>().To<OrderStore>();
            kernel.Bind<ITruckStore>().To<TruckStore>();
            kernel.Bind<IOrderTrackInfoStore>().To<OrderTrackInfoStore>();

            //Services
            kernel.Bind<IContractorService>().To<ContractorService>();
            kernel.Bind<ICustomerService>().To<CustomerService>();
            kernel.Bind<IEmployeeService>().To<EmployeeService>();
            kernel.Bind<IOrderRequestService>().To<OrderRequestService>();
            kernel.Bind<IOrderService>().To<OrderService>();
            kernel.Bind<IOrderTrackService>().To<OrderTrackService>();
            kernel.Bind<ITruckService>().To<TruckService>();
            kernel.Bind<IUserInfoService>().To<UserInfoService>();

            kernel.Bind<IRouteStopStore>().To<RouteStopStore>();
            kernel.Bind<IRouteStopService>().To<RouteStopService>();
            kernel.Bind<IRoutePlannerService, RoutePlannerService>().To<RoutePlannerService>();

            //Helpers
            kernel.Bind<ILocationHelper>().To<LocationHelper>();
            kernel.Bind<ICostCalculationHelper>().To<CostCalculationHelper>();

            kernel.Bind<IUserStore<ApplicationUser>, UserStore<ApplicationUser>>().To<UserStore<ApplicationUser>>().InRequestScope();
            kernel.Bind<ApplicationSignInManager>().ToSelf().InRequestScope();
            kernel.Bind<ApplicationUserManager>().ToSelf().InRequestScope();
            kernel.Bind<IAuthenticationManager>().ToMethod((context) =>
            {
                var cbase = new HttpContextWrapper(HttpContext.Current);
                return cbase.GetOwinContext().Authentication;
            }).InRequestScope();
            
            kernel.Bind<DbContext, IdentityDbContext<ApplicationUser>>().To<ApplicationDbContext>().InNamedOrBackgroundJobScope(context => context.Kernel.Components.GetAll<INinjectHttpApplicationPlugin>().Select(c => c.GetRequestScope(context)).FirstOrDefault(s => s != null));
            kernel.Bind<IUnitOfWork, IDbSetFactory, ApplicationContextAdapter>().To<ApplicationContextAdapter>().InNamedOrBackgroundJobScope(context => context.Kernel.Components.GetAll<INinjectHttpApplicationPlugin>().Select(c => c.GetRequestScope(context)).FirstOrDefault(s => s != null));
            kernel.Bind<SmsService>().To<SmsService>();
        }        
    }
}
