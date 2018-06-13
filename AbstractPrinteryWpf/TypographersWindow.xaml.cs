using PrinterySVC.BindingModel;
using PrinterySVC.ViewModel;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace AbstractPrinteryWpf
{
    /// <summary>
    /// Логика взаимодействия для TypographersWindow.xaml
    /// </summary>
    public partial class TypographersWindow : Window
    {
        public TypographersWindow()
        {
            InitializeComponent();
            Loaded += TypographersWindow_Load;
        }

        private void TypographersWindow_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                var response = APIClient.GetRequest("api/Typographer/GetList");
                if (response.Result.IsSuccessStatusCode)
                {
                    List<TypographerViewModel> list = APIClient.GetElement<List<TypographerViewModel>>(response);
                    if (list != null)
                    {
                        dataGridViewTypographers.ItemsSource = list;
                        dataGridViewTypographers.Columns[0].Visibility = Visibility.Hidden;
                        dataGridViewTypographers.Columns[1].Width = DataGridLength.Auto;
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
            var form = new TypographerWindow();
            if (form.ShowDialog() == true)
                LoadData();
        }

        private void buttonUpd_Click(object sender, EventArgs e)
        {
            if (dataGridViewTypographers.SelectedItem != null)
            {
                var form = new TypographerWindow();
                form.Id = ((TypographerViewModel)dataGridViewTypographers.SelectedItem).Number;
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
                        var response = APIClient.PostRequest("api/Typographer/DelElement", new CustomerBindingModel { Number = id });
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

