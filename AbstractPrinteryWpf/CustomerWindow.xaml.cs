using PrinterySVC.BindingModel;
using PrinterySVC.Inteface;
using PrinterySVC.ViewModel;
using System;
using System.Windows;
using Unity;
using Unity.Attributes;

namespace AbstractPrinteryWpf
{
    /// <summary>
    /// Логика взаимодействия для CustomerWindow.xaml
    /// </summary>
    public partial class CustomerWindow : Window
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }

        public int ID { set { id = value; } }

        private readonly ICustomerSVC service;

        private int? id;

        public CustomerWindow(ICustomerSVC service)
        {
            InitializeComponent();
            Loaded += CustomerWindow_Load;
            this.service = service;
        }

        private void CustomerWindow_Load(object sender, EventArgs e)
        {
            if (id.HasValue)
            {
                try
                {
                    CustomerVievModel view = service.GetElement(id.Value);
                    if (view != null)
                        textBoxFullName.Text = view.CustomerFIO;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxFullName.Text))
            {
                MessageBox.Show("Заполните ФИО", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                if (id.HasValue)
                {
                    service.UpdElement(new CustomerBindingModel
                    {
                        Number = id.Value,
                        CustomerFIO = textBoxFullName.Text
                    });
                }
                else
                {
                    service.AddElement(new CustomerBindingModel
                    {
                        CustomerFIO = textBoxFullName.Text
                    });
                }
                MessageBox.Show("Сохранение прошло успешно", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
