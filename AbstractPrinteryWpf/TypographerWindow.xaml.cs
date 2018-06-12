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
    /// Логика взаимодействия для TypographerWindow.xaml
    /// </summary>
    public partial class TypographerWindow : Window
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }

        public int ID { set { id = value; } }

        private readonly ITypographerSVC service;

        private int? id;

        public TypographerWindow(ITypographerSVC service)
        {
            InitializeComponent();
            Loaded += TypographerWindow_Load;
            this.service = service;
        }

        private void TypographerWindow_Load(object sender, EventArgs e)
        {
            if (id.HasValue)
            {
                try
                {
                    TypographerViewModel view = service.GetElement(id.Value);
                    if (view != null)
                        textBoxFullName.Text = view.TypographerFIO;
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
                    service.UpdElement(new TypographerBildingModel
                    {
                        Number = id.Value,
                        TypographerFIO = textBoxFullName.Text
                    });
                }
                else
                {
                    service.AddElement(new TypographerBildingModel
                    {
                        TypographerFIO = textBoxFullName.Text
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