using System;
using System.Windows;

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
            APIClient.Connect();
            var application = new App();
            application.Run(new MainWindow());
        }
    }
}
