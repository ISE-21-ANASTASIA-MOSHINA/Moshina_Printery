using PrinterySVC.BindingModel;
using PrinterySVC.ViewModel;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;

namespace AbstractPrinteryWpf
{
    /// <summary>
    /// Логика взаимодействия для TypographerWindow.xaml
    /// </summary>
    public partial class TypographerWindow : Window
    {
        public int Id { set { id = value; } }

        private int? id;

        public TypographerWindow()
        {
            InitializeComponent();
            Loaded += TypographerWindow_Load;
        }

        private void TypographerWindow_Load(object sender, EventArgs e)
        {
            if (id.HasValue)
            {
                try
                {
                    var response = APIClient.GetRequest("api/Typographer/Get/" + id.Value);
                    if (response.Result.IsSuccessStatusCode)
                    {
                        var typographer = APIClient.GetElement<TypographerViewModel>(response);
                        textBoxFullName.Text = typographer.TypographerFIO;
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
            if (string.IsNullOrEmpty(textBoxFullName.Text))
            {
                MessageBox.Show("Заполните ФИО", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                Task<HttpResponseMessage> response;
                if (id.HasValue)
                {
                    response = APIClient.PostRequest("api/Typographer/UpdElement", new TypographerBildingModel
                    {
                        Number = id.Value,
                        TypographerFIO = textBoxFullName.Text
                    });
                }
                else
                {
                    response = APIClient.PostRequest("api/Typographer/AddElement", new TypographerBildingModel
                    {
                        TypographerFIO = textBoxFullName.Text
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
