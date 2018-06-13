using PrinterySVC.BindingModel;
using PrinterySVC.ViewModel;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace AbstractPrinteryWpf
{
    /// <summary>
    /// Логика взаимодействия для MaterialsWindow.xaml
    /// </summary>
    public partial class MaterialsWindow : Window
    {
        public MaterialsWindow()
        {
            InitializeComponent();
            Loaded += MaterialsWindow_Load;
        }

        private void MaterialsWindow_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                var response = APIClient.GetRequest("api/Material/GetList");
                if (response.Result.IsSuccessStatusCode)
                {
                    List<MaterialViewModel> list = APIClient.GetElement<List<MaterialViewModel>>(response);
                    if (list != null)
                    {
                        dataGridViewElements.ItemsSource = list;
                        dataGridViewElements.Columns[0].Visibility = Visibility.Hidden;
                        dataGridViewElements.Columns[1].Width = DataGridLength.Auto;
                    }
                }
                else
                {
                    throw new Exception(APIClient.GetError(response));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            var form = new MaterialWindow();
            if (form.ShowDialog() == true)
                LoadData();
        }

        private void buttonUpd_Click(object sender, EventArgs e)
        {
            if (dataGridViewElements.SelectedItem != null)
            {
                var form = new MaterialWindow();
                form.Id = ((MaterialViewModel)dataGridViewElements.SelectedItem).Number;
                if (form.ShowDialog() == true)
                    LoadData();
            }
        }

        private void buttonDel_Click(object sender, EventArgs e)
        {
            if (dataGridViewElements.SelectedItem != null)
            {
                if (MessageBox.Show("Удалить запись?", "Внимание",
                    MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    int id = ((MaterialViewModel)dataGridViewElements.SelectedItem).Number;
                    try
                    {
                        var response = APIClient.PostRequest("api/Material/DelElement", new CustomerBindingModel { Number = id });
                        if (!response.Result.IsSuccessStatusCode)
                        {
                            throw new Exception(APIClient.GetError(response));
                        }
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