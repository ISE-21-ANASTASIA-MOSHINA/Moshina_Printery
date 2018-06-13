using PrinterySVC.BindingModel;
using PrinterySVC.ViewModel;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace AbstractPrinteryWpf
{
    /// <summary>
    /// Логика взаимодействия для RackWindow.xaml
    /// </summary>
    public partial class RackWindow : Window
    {
        public int Id { set { id = value; } }

        private int? id;

        public RackWindow()
        {
            InitializeComponent();
            Loaded += RackWindow_Load;

        }

        private void RackWindow_Load(object sender, EventArgs e)
        {
            if (id.HasValue)
            {
                try
                {
                    var rack = Task.Run(() => APIClient.GetRequestData<RackViewModel>("api/Rack/Get/" + id.Value)).Result;
                    textBoxName.Text = rack.RackName;
                    dataGridViewRack.ItemsSource = rack.RackMaterials;
                    dataGridViewRack.Columns[0].Visibility = Visibility.Hidden;
                    dataGridViewRack.Columns[1].Visibility = Visibility.Hidden;
                    dataGridViewRack.Columns[2].Visibility = Visibility.Hidden;
                    dataGridViewRack.Columns[3].Width = DataGridLength.Auto;

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
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxName.Text))
            {
                MessageBox.Show("Заполните название", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            string name = textBoxName.Text;
            Task task;
            if (id.HasValue)
            {
                task = Task.Run(() => APIClient.PostRequestData("api/Rack/UpdElement", new RackBindingModel
                {
                    Number = id.Value,
                    RackName = name
                }));
            }
            else
            {
                task = Task.Run(() => APIClient.PostRequestData("api/Rack/AddElement", new RackBindingModel
                {
                    RackName = name
                }));
            }

            task.ContinueWith((prevTask) => MessageBox.Show("Сохранение прошло успешно. Обновите список", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information),
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

            Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}