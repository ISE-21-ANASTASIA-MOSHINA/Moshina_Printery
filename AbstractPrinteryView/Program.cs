
using PrinterySVC;
using PrinterySVC.BindingModel;
using PrinterySVC.ImplementationsBD;
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
            currentContainer.RegisterType<ICustomerSVC, CustomerSVCBD>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<IMaterialSVC, MaterialSVCBD>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<ITypographerSVC, TypographerSVCBD>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<IEditionSVC, EditionSVCBD>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<IRackSVC, RackSVCBD>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<IMainSVC, MainSVCBD>(new HierarchicalLifetimeManager());

            return currentContainer;
        }
    }
}
