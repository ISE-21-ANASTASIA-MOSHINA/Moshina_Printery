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
    /// Логика взаимодействия для EditionsWindow.xaml
    /// </summary>
    public partial class EditionsWindow : Window
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }

        private readonly IEditionSVC service;

        public EditionsWindow(IEditionSVC service)
        {
            InitializeComponent();
            Loaded += EditionsWindow_Load;
            this.service = service;
        }

        private void EditionsWindow_Load(object sender, EventArgs e)
        {
          
           LoadData();
        }

        private void LoadData()
        {
            try
            {
                List<EditionViewModel> list = service.GetList();
                if (list != null)
                {
                    dataGridViewEditions.ItemsSource = list;
                    dataGridViewEditions.Columns[0].Visibility = Visibility.Hidden;
                    dataGridViewEditions.Columns[1].Width = DataGridLength.Auto;
                    dataGridViewEditions.Columns[3].Visibility = Visibility.Hidden;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void buttonAdd_Click(object sender, EventArgs e)
        {
            var form = Container.Resolve<EditionWindow>();
            if (form.ShowDialog() == true)
                LoadData();
        }

        private void buttonUp_Click(object sender, EventArgs e)
        {
            if (dataGridViewEditions.SelectedItem != null)
            {
                var form = Container.Resolve<EditionWindow>();
                form.ID = ((EditionViewModel)dataGridViewEditions.SelectedItem).Number;
                if (form.ShowDialog() == true)
                    LoadData();
            }
        }

        private void buttonDel_Click(object sender, EventArgs e)
        {
            if (dataGridViewEditions.SelectedItem != null)
            {
                if (MessageBox.Show("Удалить запись?", "Внимание",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {

                    int id = ((EditionViewModel)dataGridViewEditions.SelectedItem).Number;
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



