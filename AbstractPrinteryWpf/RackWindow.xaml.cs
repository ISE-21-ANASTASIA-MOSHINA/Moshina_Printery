using PrinterySVC.BindingModel;
using PrinterySVC.Inteface;
using PrinterySVC.ViewModel;
using System;
using System.Windows;
using System.Windows.Controls;
using Unity;
using Unity.Attributes;

namespace AbstractPrinteryWpf
{
    /// <summary>
    /// Логика взаимодействия для RackWindow.xaml
    /// </summary>
    public partial class RackWindow : Window
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }

        public int ID { set { id = value; } }

        private readonly IRackSVC service;

        private int? id;

        public RackWindow(IRackSVC service)
        {
            InitializeComponent();
            Loaded += RackWindow_Load;
            this.service = service;
        }

        private void RackWindow_Load(object sender, EventArgs e)
        {
            if (id.HasValue)
            {
                try
                {
                    RackViewModel view = service.GetElement(id.Value);
                    if (view != null)
                    {
                        textBoxName.Text = view.RackName;
                        dataGridViewRack.ItemsSource = view.RackMaterial;
                        dataGridViewRack.Columns[0].Visibility = Visibility.Hidden;
                        dataGridViewRack.Columns[1].Visibility = Visibility.Hidden;
                        dataGridViewRack.Columns[2].Visibility = Visibility.Hidden;
                        dataGridViewRack.Columns[3].Width = DataGridLength.Auto;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxName.Text))
            {
                MessageBox.Show("Заполните название", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                if (id.HasValue)
                {
                    service.UpElement(new RackBindingModel
                    {
                        Number = id.Value,
                        RackName = textBoxName.Text
                    });
                }
                else
                {
                    service.AddElement(new RackBindingModel
                    {
                        RackName = textBoxName.Text
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


