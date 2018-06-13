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
                List<EditionViewModel> list = Task.Run(() => APIClient.GetRequestData<List<EditionViewModel>>("api/Edition/GetList")).Result;
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
                while (ex.InnerException != null)
                {
                    ex = ex.InnerException;
                }
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            var form = new EditionWindow();
            if (form.ShowDialog() == true)
                LoadData();
        }

        private void buttonUpd_Click(object sender, EventArgs e)
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

                    Task task = Task.Run(() => APIClient.PostRequestData("api/Edition/DelElement", new CustomerBindingModel { Number = id }));

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