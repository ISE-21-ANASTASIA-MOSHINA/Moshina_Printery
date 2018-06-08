using PrinterySVC.ImplementationsList;
using PrinterySVC.Inteface;
using System;
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
        public static void Main()
        {
            var container = BuildUnityContainer();

            var application = new App();
            application.Run(container.Resolve<MainWindow>());
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
