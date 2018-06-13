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
                List<MaterialViewModel> list = Task.Run(() => APIClient.GetRequestData<List<MaterialViewModel>>("api/Material/GetList")).Result;
                if (list != null)
                {
                    dataGridViewElements.ItemsSource = list;
                    dataGridViewElements.Columns[0].Visibility = Visibility.Hidden;
                    dataGridViewElements.Columns[1].Width = DataGridLength.Auto;
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
                    Task task = Task.Run(() => APIClient.PostRequestData("api/Material/DelElement", new CustomerBindingModel { Number = id }));

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