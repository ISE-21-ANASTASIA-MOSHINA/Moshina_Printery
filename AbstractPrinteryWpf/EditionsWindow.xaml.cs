using PrinterySVC.BindingModel;
using PrinterySVC.Inteface;
using PrinterySVC.ViewModel;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace AbstractPrinteryWpf
{
    /// <summary>
    /// Логика взаимодействия для EditionsWindow.xaml
    /// </summary>
    public partial class EditionsWindow : Window
    {
        public EditionsWindow()
        {
            InitializeComponent();
            Loaded += EditionsWindow_Load;
        }

        private void EditionsWindow_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                var response = APIClient.GetRequest("api/Edition/GetList");
                if (response.Result.IsSuccessStatusCode)
                {
                    List<EditionViewModel> list = APIClient.GetElement<List<EditionViewModel>>(response);
                    if (list != null)
                    {
                        dataGridViewEditions.ItemsSource = list;
                        dataGridViewEditions.Columns[0].Visibility = Visibility.Hidden;
                        dataGridViewEditions.Columns[1].Width = DataGridLength.Auto;
                        dataGridViewEditions.Columns[3].Visibility = Visibility.Hidden;
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
            var form = new EditionWindow();
            if (form.ShowDialog() == true)
                LoadData();
        }

        private void buttonUp_Click(object sender, EventArgs e)
        {
            if (dataGridViewEditions.SelectedItem != null)
            {
                var form = new EditionWindow();
                form.Id = ((EditionViewModel)dataGridViewEditions.SelectedItem).Number;
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
                        var response = APIClient.PostRequest("api/Edition/DelElement", new CustomerBindingModel { Number = id });
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