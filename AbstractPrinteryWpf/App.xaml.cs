using PrinterySVC;
using PrinterySVC.ImplementationsDB;
using PrinterySVC.Inteface;
using System;
using System.Data.Entity;
using System.Windows;
using Unity;
using Unity.Lifetime;

namespace AbstractPrinteryWpf
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        [STAThread]
        static void Main()
        {
            var container = BuildUnityContainer();

            var application = new App();
            application.Run(container.Resolve<MainWindow>());
        }

        public static IUnityContainer BuildUnityContainer()
        {
            var currentContainer = new UnityContainer();
            currentContainer.RegisterType<DbContext, AbstractDbContext>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<ICustomerSVC, CustomerSVCDB>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<IMaterialSVC, MaterialSVCDB>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<ITypographerSVC, TypographerSVCDB>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<IEditionSVC, EditionSVCDB>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<IRackSVC, RackSVCDB>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<IMainSVC, MainSVCDB>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<IReportSVC, ReportSVCBD>(new HierarchicalLifetimeManager());

            return currentContainer;
        }
    }
}
