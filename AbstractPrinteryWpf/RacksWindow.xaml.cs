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
    /// Логика взаимодействия для RacksWindow.xaml
    /// </summary>
    public partial class RacksWindow : Window
    {
        public RacksWindow()
        {
            InitializeComponent();
            Loaded += RacksWindow_Load;
        }

        private void RacksWindow_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                List<RackViewModel> list = Task.Run(() => APIClient.GetRequestData<List<RackViewModel>>("api/Rack/GetList")).Result;
                if (list != null)
                {
                    dataGridViewRacks.ItemsSource = list;
                    dataGridViewRacks.Columns[0].Visibility = Visibility.Hidden;
                    dataGridViewRacks.Columns[1].Width = DataGridLength.Auto;
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
            var form = new RackWindow();
            if (form.ShowDialog() == true)
                LoadData();
        }

        private void buttonUpd_Click(object sender, EventArgs e)
        {
            if (dataGridViewRacks.SelectedItem != null)
            {
                var form = new RackWindow();
                form.Id = ((RackViewModel)dataGridViewRacks.SelectedItem).Number;
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
                    Task task = Task.Run(() => APIClient.PostRequestData("api/Rack/DelElement", new CustomerBindingModel { Number = id }));

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
