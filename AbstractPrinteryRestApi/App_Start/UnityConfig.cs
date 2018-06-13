using PrinterySVC;
using PrinterySVC.ImplementationsDB;
using PrinterySVC.Inteface;
using System;
using System.Data.Entity;
using Unity;
using Unity.Lifetime;

namespace AbstractPrinteryRestApi
{
    /// <summary>
    /// Specifies the Unity configuration for the main container.
    /// </summary>
    public static class UnityConfig
    {
        #region Unity Container
        private static Lazy<IUnityContainer> container =
          new Lazy<IUnityContainer>(() =>
          {
              var container = new UnityContainer();
              RegisterTypes(container);
              return container;
          });

        /// <summary>
        /// Configured Unity Container.
        /// </summary>
        public static IUnityContainer Container => container.Value;
        #endregion

        /// <summary>
        /// Registers the type mappings with the Unity container.
        /// </summary>
        /// <param name="container">The unity container to configure.</param>
        /// <remarks>
        /// There is no need to register concrete types such as controllers or
        /// API controllers (unless you want to change the defaults), as Unity
        /// allows resolving a concrete type even if it was not previously
        /// registered.
        /// </remarks>
        public static void RegisterTypes(IUnityContainer container)
        {
            // NOTE: To load from web.config uncomment the line below.
            // Make sure to add a Unity.Configuration to the using statements.
            // container.LoadConfiguration();

            // TODO: Register your type's mappings here.
            // container.RegisterType<IProductRepository, ProductRepository>();

            container.RegisterType<DbContext, AbstractDbContext>(new HierarchicalLifetimeManager());
            container.RegisterType<ICustomerSVC, CustomerSVCDB>(new HierarchicalLifetimeManager());
            container.RegisterType<IMaterialSVC, MaterialSVCDB>(new HierarchicalLifetimeManager());
            container.RegisterType<ITypographerSVC, TypographerSVCDB>(new HierarchicalLifetimeManager());
            container.RegisterType<IEditionSVC, EditionSVCDB>(new HierarchicalLifetimeManager());
            container.RegisterType<IRackSVC, RackSVCDB>(new HierarchicalLifetimeManager());
            container.RegisterType<IMainSVC, MainSVCDB>(new HierarchicalLifetimeManager());
            container.RegisterType<IReportSVC, ReportSVCBD>(new HierarchicalLifetimeManager());
        }
    }
}