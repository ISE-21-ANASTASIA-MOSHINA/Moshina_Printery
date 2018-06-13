using PrinterySVC.BindingModel;
using PrinterySVC.ViewModel;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;

namespace AbstractPrinteryWpf
{
    /// <summary>
    /// Логика взаимодействия для MaterialWindow.xaml
    /// </summary>
    public partial class MaterialWindow : Window
    {
        public int Id { set { id = value; } }

        private int? id;

        public MaterialWindow()
        {
            InitializeComponent();
            Loaded += MaterialWindow_Load;
        }

        private void MaterialWindow_Load(object sender, EventArgs e)
        {
            if (id.HasValue)
            {
                try
                {
                    var response = APIClient.GetRequest("api/Material/Get/" + id.Value);
                    if (response.Result.IsSuccessStatusCode)
                    {
                        var ingredient = APIClient.GetElement<MaterialViewModel>(response);
                        textBoxName.Text = ingredient.MaterialName;
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
                    response = APIClient.PostRequest("api/Material/UpdElement", new MaterialBindingModel
                    {
                        Number = id.Value,
                        MaterialName = textBoxName.Text
                    });
                }
                else
                {
                    response = APIClient.PostRequest("api/Material/AddElement", new MaterialBindingModel
                    {
                        MaterialName = textBoxName.Text
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
