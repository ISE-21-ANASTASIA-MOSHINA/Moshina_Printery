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
    /// Логика взаимодействия для RacksWindow.xaml
    /// </summary>
    public partial class RacksWindow : Window
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }

        private readonly IRackSVC service;

        public RacksWindow(IRackSVC service)
        {
            InitializeComponent();
            Loaded += RacksWindow_Load;
            this.service = service;
        }

        private void RacksWindow_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                List<RackViewModel> list = service.GetList();
                if (list != null)
                {
                    dataGridViewRacks.ItemsSource = list;
                    dataGridViewRacks.Columns[0].Visibility = Visibility.Hidden;
                    dataGridViewRacks.Columns[1].Width = DataGridLength.Auto;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            var form = Container.Resolve<RackWindow>();
            if (form.ShowDialog() == true)
                LoadData();
        }

        private void buttonUpd_Click(object sender, EventArgs e)
        {
            if (dataGridViewRacks.SelectedItem != null)
            {
                var form = Container.Resolve<RackWindow>();
                form.ID = ((RackViewModel)dataGridViewRacks.SelectedItem).Number;
                if (form.ShowDialog() == true)
                {
                    LoadData();
                }
            }
        }

        private void buttonDel_Click(object sender, EventArgs e)
        {
            if (dataGridViewRacks.SelectedItem != null)
            {
                if (MessageBox.Show("Удалить запись?", "Внимание",
                    MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    int id = ((RackViewModel)dataGridViewRacks.SelectedItem).Number;
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

