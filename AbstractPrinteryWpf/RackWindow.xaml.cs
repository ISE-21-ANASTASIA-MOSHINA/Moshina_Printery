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
                    var response = APIClient.GetRequest("api/Rack/Get/" + id.Value);
                    if (response.Result.IsSuccessStatusCode)
                    {
                        var baza = APIClient.GetElement<RackViewModel>(response);
                        textBoxName.Text = baza.RackName;
                        dataGridViewRack.ItemsSource = baza.RackMaterial;
                        dataGridViewRack.Columns[0].Visibility = Visibility.Hidden;
                        dataGridViewRack.Columns[1].Visibility = Visibility.Hidden;
                        dataGridViewRack.Columns[2].Visibility = Visibility.Hidden;
                        dataGridViewRack.Columns[3].Width = DataGridLength.Auto;
                    }
                }
                catch (Exception ex)
                {
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
            try
            {
                Task<HttpResponseMessage> response;
                if (id.HasValue)
                {
                    response = APIClient.PostRequest("api/Rack/UpdElement", new RackBindingModel
                    {
                        Number = id.Value,
                        RackName = textBoxName.Text
                    });
                }
                else
                {
                    response = APIClient.PostRequest("api/Rack/AddElement", new RackBindingModel
                    {
                        RackName = textBoxName.Text
                    });
                }
                if (response.Result.IsSuccessStatusCode)
                {
                    MessageBox.Show("Сохранение прошло успешно", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information);
                    DialogResult = true;
                    Close();
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

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}