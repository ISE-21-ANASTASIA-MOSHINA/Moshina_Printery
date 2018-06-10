
using PrinterySVC.BindingModel;
using PrinterySVC.ImplementationsList;
using PrinterySVC.Inteface;
using PrinterySVC.ViewModel;
using System;
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
            currentContainer.RegisterType<ICustomerSVC, CustomerSVClist>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<IMaterialSVC, MaterialSVClist>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<ITypographerSVC, TypographerSVClist>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<IEditionSVC, EditionSVClist>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<IRackSVC, RackSVClist>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<IMainSVC, MainSVClist>(new HierarchicalLifetimeManager());

            return currentContainer;
        }
    }
}
