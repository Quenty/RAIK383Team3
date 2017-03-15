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
            //TODO: Make some of these inrequestscope

            //UnitOfWork
            kernel.Bind<ApplicationDbContext>().ToSelf().InSingletonScope();
            kernel.Bind<IUnitOfWork>().To<UnitOfWork>();

            //Stores
            kernel.Bind<IContractorStore>().To<ContractorStore>().;
            kernel.Bind<ICustomerStore>().To<CustomerStore>();
            kernel.Bind<IEmployeeStore>().To<EmployeeStore>();
            kernel.Bind<IOrderStore>().To<OrderStore>();
            kernel.Bind<ITruckStore>().To<TruckStore>();

            //Services
            kernel.Bind<IContractorService>().To<ContractorService>();
            kernel.Bind<ICustomerService>().To<CustomerService>();
            kernel.Bind<IEmployeeService>().To<EmployeeService>();
            kernel.Bind<IOrderRequestService>().To<OrderRequestService>();
            kernel.Bind<IOrderService>().To<OrderService>();
            kernel.Bind<IOrderTrackService>().To<OrderTrackService>();
            kernel.Bind<ITruckService>().To<TruckService>();
            kernel.Bind<IUserInfoService>().To<UserInfoService>();
        }        
    }
}
