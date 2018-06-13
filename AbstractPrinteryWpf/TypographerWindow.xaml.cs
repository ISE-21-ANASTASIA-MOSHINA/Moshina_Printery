using PrinterySVC.BindingModel;
using PrinterySVC.ViewModel;
using System;
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
                    var typographer = Task.Run(() => APIClient.GetRequestData<TypographerViewModel>("api/Typographer/Get/" + id.Value)).Result;
                    textBoxFullName.Text = typographer.TypographerFIO;
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
            if (string.IsNullOrEmpty(textBoxFullName.Text))
            {
                MessageBox.Show("Заполните ФИО", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            string fio = textBoxFullName.Text;
            Task task;
            if (id.HasValue)
            {
                task = Task.Run(() => APIClient.PostRequestData("api/Typographer/UpdElement", new TypographerBildingModel
                {
                    Number = id.Value,
                    TypographerFIO = fio
                }));
            }
            else
            {
                task = Task.Run(() => APIClient.PostRequestData("api/Typographer/AddElement", new TypographerBildingModel
                {
                    TypographerFIO = fio
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
