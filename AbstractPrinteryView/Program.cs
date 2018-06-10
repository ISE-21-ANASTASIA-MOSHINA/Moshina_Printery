
using PrinterySVC;
using PrinterySVC.BindingModel;
using PrinterySVC.ImplementationsDB;
using PrinterySVC.ImplementationsList;
using PrinterySVC.Inteface;
using PrinterySVC.ViewModel;
using System;
using System.Data.Entity;
using System.Windows.Forms;
using Unity;
using Unity.Lifetime;


namespace AbstractPrinteryView
{
    static  class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            var container = BuildUnityContainer();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(container.Resolve<FormMain>());
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
