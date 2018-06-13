using PrinterySVC.BindingModel;
using PrinterySVC.ViewModel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
                List<TypographerViewModel> list = Task.Run(() => APIClient.GetRequestData<List<TypographerViewModel>>("api/Typographer/GetList")).Result;
                if (list != null)
                {
                    dataGridViewTypographers.ItemsSource = list;
                    dataGridViewTypographers.Columns[0].Visibility = Visibility.Hidden;
                    dataGridViewTypographers.Columns[1].Width = DataGridLength.Auto;
                }
            }
            catch (Exception ex)
            {
                while (ex.InnerException != null)
                {
                    ex = ex.InnerException;
                }
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
                    Task task = Task.Run(() => APIClient.PostRequestData("api/Typographer/DelElement", new CustomerBindingModel { Number = id }));

                    task.ContinueWith((prevTask) => MessageBox.Show("Запись удалена. Обновите список", "Успех", MessageBoxButton.OK, MessageBoxImage.Information),
                    TaskContinuationOptions.OnlyOnRanToCompletion);

                    task.ContinueWith((prevTask) =>
                    {
                        var ex = (Exception)prevTask.Exception;
                        while (ex.InnerException != null)
                        {
                            ex = ex.InnerException;
                        }
                        MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }, TaskContinuationOptions.OnlyOnFaulted);
                }
            }
        }

        private void buttonRef_Click(object sender, EventArgs e)
        {
            LoadData();
        }
    }
}

