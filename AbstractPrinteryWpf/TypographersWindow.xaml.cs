using PrinterySVC.Inteface;
using PrinterySVC.ViewModel;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Unity;
using Unity.Attributes;

namespace AbstractPrinteryWpf
{
    /// <summary>
    /// Логика взаимодействия для TypographersWindow.xaml
    /// </summary>
    public partial class TypographersWindow : Window
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }

        private readonly ITypographerSVC service;

        public TypographersWindow(ITypographerSVC service)
        {
            InitializeComponent();
            Loaded += TypographersWindow_Load;
            this.service = service;
        }

        private void TypographersWindow_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                List<TypographerViewModel> list = service.GetList();
                if (list != null)
                {
                    dataGridViewTypographers.ItemsSource = list;
                    dataGridViewTypographers.Columns[0].Visibility = Visibility.Hidden;
                    dataGridViewTypographers.Columns[1].Width = DataGridLength.Auto;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            var form = Container.Resolve<TypographerWindow>();
            if (form.ShowDialog() == true)
                LoadData();
        }

        private void buttonUpd_Click(object sender, EventArgs e)
        {
            if (dataGridViewTypographers.SelectedItem != null)
            {
                var form = Container.Resolve<TypographerWindow>();
                form.ID = ((TypographerViewModel)dataGridViewTypographers.SelectedItem).Number;
                if (form.ShowDialog() == true)
                    LoadData();
            }
        }

        private void buttonDel_Click(object sender, EventArgs e)
        {
            if (dataGridViewTypographers.SelectedItem != null)
            {
                if (MessageBox.Show("Удалить запись?", "Внимание",
                    MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    int id = ((TypographerViewModel)dataGridViewTypographers.SelectedItem).Number;
                    try
                    {
                        service.DelElement(id);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    LoadData();
                }
            }
        }

        private void buttonRef_Click(object sender, EventArgs e)
        {
            LoadData();
        }
    }
}

